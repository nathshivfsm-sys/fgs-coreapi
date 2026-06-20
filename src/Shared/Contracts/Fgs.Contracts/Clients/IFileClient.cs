using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal/public HTTP client for file operations owned by FileService.
/// </summary>
public interface IFileClient
{
    [Post("/api/v1/files/upload-url")]
    Task<Fgs.Contracts.Api.ApiResponse<CreateFileUploadUrlResponse>> CreateUploadUrlAsync(
        [Body] CreateFileUploadUrlRequest request,
        CancellationToken cancellationToken = default);

    [Post("/api/v1/files/{fileId}/complete")]
    Task<Fgs.Contracts.Api.ApiResponse<FileVariantSetDto>> CompleteUploadAsync(
        long fileId,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/files/by-entity")]
    Task<Fgs.Contracts.Api.ApiResponse<CompanyLogoDto>> GetFilesByEntityAsync(
        [Query] string entityType,
        [Query] long entityId,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/files/{fileId}")]
    Task<Fgs.Contracts.Api.ApiResponse<FileMetadataDto>> GetFileAsync(
        long fileId,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/files/{fileId}/content")]
    Task<Fgs.Contracts.Api.ApiResponse<FileContentUrlResponse>> GetFileContentUrlAsync(
        long fileId,
        CancellationToken cancellationToken = default);
}

public sealed record CreateFileUploadUrlRequest(
    string FileName,
    string ContentType,
    long FileSizeBytes,
    string EntityType,
    long EntityId,
    string RequestedVariant,
    string? Description,
    IReadOnlyList<string>? Tags);

public sealed record CreateFileUploadUrlResponse(
    long FileId,
    string UploadUrl,
    string UploadMethod,
    IReadOnlyDictionary<string, string> RequiredHeaders,
    string ObjectKey,
    DateTimeOffset ExpiresAt);

public sealed record FileVariantInfoDto(
    long FileId,
    string ContentUrl,
    string? DownloadUrl,
    string ContentType,
    long FileSizeBytes);

public sealed record FileVariantSetDto(
    long SourceFileId,
    IReadOnlyDictionary<string, FileVariantInfoDto> Variants);

public sealed record FileMetadataDto(
    long FileId,
    long TenantId,
    long CompanyId,
    string EntityType,
    long EntityId,
    string OriginalFileName,
    string ContentType,
    long FileSizeBytes,
    string[]? Tags,
    string? Description,
    DateTimeOffset CreatedOn);

public sealed record FileContentUrlResponse(
    long FileId,
    string DownloadUrl,
    string ContentType,
    DateTimeOffset ExpiresAt);

public sealed record CompanyLogoDto(
    string EntityType,
    long EntityId,
    IReadOnlyDictionary<string, FileVariantInfoDto?> Variants);
