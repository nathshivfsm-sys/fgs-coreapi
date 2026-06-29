namespace Fgs.File.Application.Abstractions.Storage;

public interface IFileStorageService
{
    Task UploadAsync(
        StorageObjectRef location,
        Stream content,
        StorageUploadOptions options,
        CancellationToken cancellationToken = default);

    Task<StorageObjectReadResult> OpenReadAsync(
        StorageObjectRef location,
        StorageByteRange? range = null,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        StorageObjectRef location,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        StorageObjectRef location,
        CancellationToken cancellationToken = default);
}

public sealed record StorageObjectRef(string Bucket, string ObjectKey);

public sealed record StorageUploadOptions(string ContentType, long ContentLength);

public sealed record StorageByteRange(long Start, long? End);

public sealed class StorageObjectReadResult : IAsyncDisposable
{
    public required Stream Content { get; init; }

    public required string ContentType { get; init; }

    public required long ContentLength { get; init; }

    public string? ETag { get; init; }

    public DateTimeOffset? LastModified { get; init; }

    public async ValueTask DisposeAsync() => await Content.DisposeAsync();
}
