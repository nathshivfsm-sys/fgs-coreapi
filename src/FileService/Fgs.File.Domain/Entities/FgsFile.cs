using Fgs.Kernel.Entities;

namespace Fgs.File.Domain.Entities;

/// <summary>
/// Object-storage file metadata scoped to a tenant company and associated business entity.
/// </summary>
public class FgsFile : FgsTenantCompanySetupEntityBase<long>
{
    public string EntityType { get; set; } = null!;

    public long EntityId { get; set; }

    public string BucketName { get; set; } = null!;

    public string ObjectKey { get; set; } = null!;

    public string? ThumbnailObjectKey { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string StoredFileName { get; set; } = null!;

    public string? ContentType { get; set; }

    public string? FileExtension { get; set; }

    public long FileSizeBytes { get; set; }

    public string? Description { get; set; }

    public string[]? Tags { get; set; }

    public bool IsVisibleToCustomer { get; set; } = true;

    public bool IsVisibleToFieldTechnician { get; set; } = true;

    public long? UploadedByUserId { get; set; }

    public string UploadedByName { get; set; } = null!;

    public string UploadedByType { get; set; } = null!;
}
