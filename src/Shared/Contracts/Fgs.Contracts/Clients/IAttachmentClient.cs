using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal/public HTTP client for attachment operations owned by FileService.
/// </summary>
public interface IAttachmentClient
{
    [Multipart]
    [Post("/api/v1/attachment")]
    Task<Fgs.Contracts.Api.ApiResponse<AttachmentMetadataDto>> UploadAsync(
        StreamPart file,
        [AliasAs("entityType")] string entityType,
        [AliasAs("entityId")] long entityId,
        [AliasAs("category")] string category,
        [AliasAs("description")] string? description,
        [AliasAs("tags")] string? tags,
        [AliasAs("isVisibleToCustomer")] bool isVisibleToCustomer,
        [AliasAs("isVisibleToFieldTechnician")] bool isVisibleToFieldTechnician,
        [AliasAs("logoVariant")] string? logoVariant,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/attachment/{entityType}/{attachmentId}/metadata")]
    Task<Fgs.Contracts.Api.ApiResponse<AttachmentMetadataDto>> GetMetadataAsync(
        string entityType,
        long attachmentId,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/attachment")]
    Task<Fgs.Contracts.Api.ApiResponse<AttachmentPagedResultDto<AttachmentMetadataDto>>> ListAsync(
        [Query] int page,
        [Query] int pageSize,
        [Query] string? sortBy,
        [Query] string? sortDirection,
        [Query] string? search,
        [Query] string? entityType,
        [Query] long? entityId,
        [Query] bool? isVisibleToCustomer,
        [Query] bool? isVisibleToFieldTechnician,
        [Query] string? category,
        [Query] string? contentType,
        [Query] string? extension,
        [Query] string? fileName,
        [Query] string? uploadedBy,
        [Query] long? uploadedByUserId,
        [Query] string? tags,
        CancellationToken cancellationToken = default);

    [Delete("/api/v1/attachment/{attachmentId}")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> DeleteAsync(
        long attachmentId,
        CancellationToken cancellationToken = default);

    [Delete("/api/v1/attachment/by-entity")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> BulkDeleteByEntityAsync(
        [Query] string entityType,
        [Query] long entityId,
        [Query] string? category,
        CancellationToken cancellationToken = default);
}

public sealed record AttachmentPagedResultDto<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    long TotalCount);

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

public sealed record CompanyLogoDto(
    string EntityType,
    long EntityId,
    IReadOnlyDictionary<string, AttachmentVariantInfoDto?> Variants);

public sealed record AttachmentVariantInfoDto(
    long AttachmentId,
    string DownloadUrl,
    string ThumbnailUrl,
    string ContentType,
    long FileSizeBytes);

public sealed record AttachmentVariantSetDto(
    long SourceAttachmentId,
    IReadOnlyDictionary<string, AttachmentVariantInfoDto> Variants);
