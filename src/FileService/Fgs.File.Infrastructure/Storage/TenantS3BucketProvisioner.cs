using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Fgs.File.Application.Abstractions.Provisioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Fgs.Credentials.Options;

namespace Fgs.File.Infrastructure.Storage;

public sealed class TenantS3BucketProvisioner(
    AmazonS3Client s3Client,
    IOptions<AwsCredentialsOptions> awsOptions,
    ILogger<TenantS3BucketProvisioner> logger) : ITenantS3BucketProvisioner
{
    private readonly AwsCredentialsOptions _options = awsOptions.Value;

    public async Task<string> EnsureTenantBucketAsync(
        long tenantId,
        string? existingBucketName,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(existingBucketName))
        {
            if (await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, existingBucketName))
            {
                logger.LogInformation("S3 bucket {Bucket} already exists for tenant {TenantId}", existingBucketName, tenantId);
                return existingBucketName;
            }

            logger.LogWarning(
                "Recorded bucket {Bucket} not found for tenant {TenantId}; creating a new bucket",
                existingBucketName,
                tenantId);
        }

        var bucketName = BuildBucketName(tenantId);

        var putBucketRequest = new PutBucketRequest
        {
            BucketName = bucketName,
            ObjectOwnership = ObjectOwnership.BucketOwnerEnforced
        };

        if (!string.Equals(_options.Region, "us-east-1", StringComparison.OrdinalIgnoreCase))
        {
            putBucketRequest.BucketRegion = S3Region.FindValue(_options.Region);
        }

        await s3Client.PutBucketAsync(putBucketRequest, cancellationToken);

        await s3Client.PutPublicAccessBlockAsync(new PutPublicAccessBlockRequest
        {
            BucketName = bucketName,
            PublicAccessBlockConfiguration = new PublicAccessBlockConfiguration
            {
                BlockPublicAcls = true,
                IgnorePublicAcls = true,
                BlockPublicPolicy = true,
                RestrictPublicBuckets = true
            }
        }, cancellationToken);

        var encryptionRule = new ServerSideEncryptionRule { BucketKeyEnabled = true };
        if (!string.IsNullOrWhiteSpace(_options.KmsKeyArn))
        {
            encryptionRule.ServerSideEncryptionByDefault = new ServerSideEncryptionByDefault
            {
                ServerSideEncryptionAlgorithm = ServerSideEncryptionMethod.AWSKMS,
                ServerSideEncryptionKeyManagementServiceKeyId = _options.KmsKeyArn
            };
        }
        else
        {
            encryptionRule.ServerSideEncryptionByDefault = new ServerSideEncryptionByDefault
            {
                ServerSideEncryptionAlgorithm = ServerSideEncryptionMethod.AES256
            };
        }

        await s3Client.PutBucketEncryptionAsync(new PutBucketEncryptionRequest
        {
            BucketName = bucketName,
            ServerSideEncryptionConfiguration = new ServerSideEncryptionConfiguration
            {
                ServerSideEncryptionRules = [encryptionRule]
            }
        }, cancellationToken);

        await s3Client.PutBucketVersioningAsync(new PutBucketVersioningRequest
        {
            BucketName = bucketName,
            VersioningConfig = new S3BucketVersioningConfig { Status = VersionStatus.Enabled }
        }, cancellationToken);

        await TryConfigureBucketCorsAsync(bucketName, tenantId, cancellationToken);

        logger.LogInformation("Created S3 bucket {Bucket} for tenant {TenantId}", bucketName, tenantId);
        return bucketName;
    }

    private async Task TryConfigureBucketCorsAsync(
        string bucketName,
        long tenantId,
        CancellationToken cancellationToken)
    {
        try
        {
            await s3Client.PutCORSConfigurationAsync(new PutCORSConfigurationRequest
            {
                BucketName = bucketName,
                Configuration = new CORSConfiguration
                {
                    Rules =
                    [
                        new CORSRule
                        {
                            AllowedMethods = ["GET", "PUT", "HEAD"],
                            AllowedOrigins = ["*"],
                            AllowedHeaders = ["*"],
                            MaxAgeSeconds = 3600
                        }
                    ]
                }
            }, cancellationToken);
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogWarning(
                ex,
                "Skipped S3 CORS configuration for bucket {Bucket} (tenant {TenantId}). "
                + "Bucket provisioning will continue; grant s3:PutBucketCORS if browser uploads require CORS.",
                bucketName,
                tenantId);
        }
    }

    public async Task InitializeFolderStructureAsync(
        string bucketName,
        long tenantId,
        IReadOnlyList<long> companyIds,
        CancellationToken cancellationToken = default)
    {
        var prefixes = new List<string> { S3ObjectKeyBuilder.TenantAssetsRoot };

        foreach (var companyId in companyIds)
        {
            prefixes.Add(S3ObjectKeyBuilder.CompanyAssetsPrefix(companyId));
            prefixes.Add(S3ObjectKeyBuilder.CompanyGeneralPrefix(companyId));
        }

        foreach (var prefix in prefixes)
        {
            await s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                Key = prefix,
                ContentBody = string.Empty
            }, cancellationToken);

            logger.LogDebug(
                "Initialized S3 prefix {Prefix} in bucket {Bucket} for tenant {TenantId}",
                prefix,
                bucketName,
                tenantId);
        }
    }

    private string BuildBucketName(long tenantId)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var suffix = string.Create(4, chars, static (span, alphabet) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = alphabet[Random.Shared.Next(alphabet.Length)];
            }
        });
        return $"{_options.BucketNamePrefix}-{tenantId}-{suffix}";
    }
}
