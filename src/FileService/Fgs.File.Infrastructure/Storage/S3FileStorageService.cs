using Amazon.S3;
using Amazon.S3.Model;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using S3ByteRange = Amazon.S3.Model.ByteRange;

namespace Fgs.File.Infrastructure.Storage;

public sealed class S3FileStorageService(
    AmazonS3Client s3Client,
    IOptions<AwsCredentialsOptions> awsOptions) : IFileStorageService
{
    private readonly AwsCredentialsOptions _awsOptions = awsOptions.Value;

    public async Task UploadAsync(
        StorageObjectRef location,
        Stream content,
        StorageUploadOptions options,
        CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = location.Bucket,
            Key = location.ObjectKey,
            InputStream = content,
            ContentType = options.ContentType
        };

        if (!string.IsNullOrWhiteSpace(_awsOptions.KmsKeyArn))
        {
            request.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AWSKMS;
            request.ServerSideEncryptionKeyManagementServiceKeyId = _awsOptions.KmsKeyArn;
        }

        await s3Client.PutObjectAsync(request, cancellationToken);
    }

    public async Task<StorageObjectReadResult> OpenReadAsync(
        StorageObjectRef location,
        StorageByteRange? range = null,
        CancellationToken cancellationToken = default)
    {
        var request = new GetObjectRequest
        {
            BucketName = location.Bucket,
            Key = location.ObjectKey
        };

        if (range is not null)
        {
            request.ByteRange = range.End.HasValue
                ? new S3ByteRange($"bytes={range.Start}-{range.End.Value}")
                : new S3ByteRange($"bytes={range.Start}-");
        }

        var response = await s3Client.GetObjectAsync(request, cancellationToken);
        return new StorageObjectReadResult
        {
            Content = response.ResponseStream,
            ContentType = response.Headers.ContentType ?? "application/octet-stream",
            ContentLength = response.Headers.ContentLength,
            ETag = response.ETag,
            LastModified = response.LastModified
        };
    }

    public Task DeleteAsync(
        StorageObjectRef location,
        CancellationToken cancellationToken = default) =>
        s3Client.DeleteObjectAsync(location.Bucket, location.ObjectKey, cancellationToken);

    public async Task<bool> ExistsAsync(
        StorageObjectRef location,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await s3Client.GetObjectMetadataAsync(location.Bucket, location.ObjectKey, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}
