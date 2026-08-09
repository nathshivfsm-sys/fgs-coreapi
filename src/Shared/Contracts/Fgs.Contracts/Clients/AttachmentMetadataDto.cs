namespace Fgs.Contracts.Clients;

/// <summary>
/// Attachment metadata returned by FileService attachment endpoints.
/// </summary>
public sealed record AttachmentMetadataDto(
    long AttachmentId,
    long TenantId,
    long CompanyId,
    string EntityType,
    long EntityId,
    string? Category,
    string OriginalFileName,
    string StoredFileName,
    string ContentType,
    string? FileExtension,
    long FileSizeBytes,
    string[]? Tags,
    string? Description,
    bool IsVisibleToCustomer,
    bool IsVisibleToFieldTechnician,
    long? UploadedByUserId,
    string UploadedByName,
    string UploadedByType,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy,
    DateTimeOffset UploadedOn,
    bool IsActive,
    string DownloadUrl,
    string ThumbnailUrl,
    string MetadataUrl);
