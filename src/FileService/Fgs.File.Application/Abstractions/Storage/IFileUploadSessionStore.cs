namespace Fgs.File.Application.Abstractions.Storage;

public sealed class FileUploadSession
{
    public Guid UploadId { get; init; }

    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public string EntityType { get; init; } = null!;

    public long EntityId { get; init; }

    public string BucketName { get; init; } = null!;

    public string ObjectKey { get; init; } = null!;

    public string OriginalFileName { get; init; } = null!;

    public string StoredFileName { get; init; } = null!;

    public string ContentType { get; init; } = null!;

    public long FileSizeBytes { get; init; }

    public string[] BaseTags { get; init; } = [];

    public string[] RequestedVariants { get; init; } = [];

    public string? Description { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}

public interface IFileUploadSessionStore
{
    void Save(FileUploadSession session);

    FileUploadSession? Get(Guid uploadId);

    void Remove(Guid uploadId);
}
