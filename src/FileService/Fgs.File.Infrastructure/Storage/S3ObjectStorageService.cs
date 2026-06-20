using Amazon.S3;
using Amazon.S3.Model;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace Fgs.File.Infrastructure.Storage;

public sealed class S3ObjectStorageService(
    AmazonS3Client s3Client,
    IOptions<AwsCredentialsOptions> awsOptions) : IS3ObjectStorageService
{
    private readonly AwsCredentialsOptions _awsOptions = awsOptions.Value;

    public Task<PresignedUploadRequest> CreateUploadUrlAsync(
        string bucketName,
        string objectKey,
        string contentType,
        TimeSpan expiry,
        CancellationToken cancellationToken = default)
    {
        // Bucket default encryption (SSE-KMS) applies automatically — do not add SSE headers
        // to presigned URLs; they require SigV4 header signing and break Postman/browser clients.
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiry),
            ContentType = contentType,
            Protocol = Protocol.HTTPS
        };

        var url = s3Client.GetPreSignedURL(request);
        var requiredHeaders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Type"] = contentType
        };

        return Task.FromResult(new PresignedUploadRequest(url, requiredHeaders));
    }

    public Task<string> CreateDownloadUrlAsync(
        string bucketName,
        string objectKey,
        TimeSpan expiry,
        CancellationToken cancellationToken = default)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.Add(expiry),
            Protocol = Protocol.HTTPS
        };

        return Task.FromResult(s3Client.GetPreSignedURL(request));
    }

    public async Task<bool> ObjectExistsAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await s3Client.GetObjectMetadataAsync(bucketName, objectKey, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<S3ObjectPayload> GetObjectAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        var response = await s3Client.GetObjectAsync(bucketName, objectKey, cancellationToken);
        return new S3ObjectPayload
        {
            Content = response.ResponseStream,
            ContentType = response.Headers.ContentType ?? "application/octet-stream",
            ContentLength = response.Headers.ContentLength
        };
    }

    public Task PutObjectAsync(
        string bucketName,
        string objectKey,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            InputStream = content,
            ContentType = contentType
        };

        if (!string.IsNullOrWhiteSpace(_awsOptions.KmsKeyArn))
        {
            request.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AWSKMS;
            request.ServerSideEncryptionKeyManagementServiceKeyId = _awsOptions.KmsKeyArn;
        }

        return s3Client.PutObjectAsync(request, cancellationToken);
    }

    public Task DeleteObjectAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default) =>
        s3Client.DeleteObjectAsync(bucketName, objectKey, cancellationToken);
}
