using Amazon.S3;
using Amazon.S3.Model;
using Fgs.File.Application.Abstractions.Storage;

namespace Fgs.File.Infrastructure.Storage;

public sealed class S3ObjectStorageService(AmazonS3Client s3Client) : IS3ObjectStorageService
{
    public Task<string> CreateUploadUrlAsync(
        string bucketName,
        string objectKey,
        string contentType,
        TimeSpan expiry,
        CancellationToken cancellationToken = default)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiry),
            ContentType = contentType
        };

        return Task.FromResult(s3Client.GetPreSignedURL(request));
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
            Expires = DateTime.UtcNow.Add(expiry)
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
        CancellationToken cancellationToken = default) =>
        s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            InputStream = content,
            ContentType = contentType
        }, cancellationToken);

    public Task DeleteObjectAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default) =>
        s3Client.DeleteObjectAsync(bucketName, objectKey, cancellationToken);
}
