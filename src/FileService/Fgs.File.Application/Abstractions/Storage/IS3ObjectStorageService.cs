namespace Fgs.File.Application.Abstractions.Storage;

public interface IS3ObjectStorageService
{
    Task<PresignedUploadRequest> CreateUploadUrlAsync(
        string bucketName,
        string objectKey,
        string contentType,
        TimeSpan expiry,
        CancellationToken cancellationToken = default);

    Task<string> CreateDownloadUrlAsync(
        string bucketName,
        string objectKey,
        TimeSpan expiry,
        CancellationToken cancellationToken = default);

    Task<bool> ObjectExistsAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default);

    Task<S3ObjectPayload> GetObjectAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default);

    Task PutObjectAsync(
        string bucketName,
        string objectKey,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default);

    Task DeleteObjectAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken = default);
}

public sealed record PresignedUploadRequest(
    string Url,
    IReadOnlyDictionary<string, string> RequiredHeaders);

public sealed class S3ObjectPayload : IAsyncDisposable
{
    public required Stream Content { get; init; }

    public required string ContentType { get; init; }

    public required long ContentLength { get; init; }

    public async ValueTask DisposeAsync()
    {
        await Content.DisposeAsync();
    }
}
