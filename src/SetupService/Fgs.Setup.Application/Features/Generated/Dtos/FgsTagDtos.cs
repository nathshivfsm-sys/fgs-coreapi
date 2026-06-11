namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsTag</summary>
public sealed record FgsTagSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TagCode</summary>
    string? TagCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>NormalizedName</summary>
    string? NormalizedName,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BackgroundColor</summary>
    string? BackgroundColor,
    /// <summary>TextColor</summary>
    string? TextColor,
    /// <summary>IconFileId</summary>
    long? IconFileId,
    /// <summary>UsageCount</summary>
    int UsageCount,
    /// <summary>IsSystemGenerated</summary>
    bool IsSystemGenerated,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsTagDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TagCode</summary>
    string? TagCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>NormalizedName</summary>
    string? NormalizedName,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BackgroundColor</summary>
    string? BackgroundColor,
    /// <summary>TextColor</summary>
    string? TextColor,
    /// <summary>IconFileId</summary>
    long? IconFileId,
    /// <summary>UsageCount</summary>
    int UsageCount,
    /// <summary>IsSystemGenerated</summary>
    bool IsSystemGenerated,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsTagCreateDto(
    /// <summary>TagCode</summary>
    string? TagCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>NormalizedName</summary>
    string? NormalizedName,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BackgroundColor</summary>
    string? BackgroundColor,
    /// <summary>TextColor</summary>
    string? TextColor,
    /// <summary>IconFileId</summary>
    long? IconFileId,
    /// <summary>UsageCount</summary>
    int UsageCount,
    /// <summary>IsSystemGenerated</summary>
    bool IsSystemGenerated)
;

public sealed record FgsTagUpdateDto(
    /// <summary>TagCode</summary>
    string? TagCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>NormalizedName</summary>
    string? NormalizedName,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BackgroundColor</summary>
    string? BackgroundColor,
    /// <summary>TextColor</summary>
    string? TextColor,
    /// <summary>IconFileId</summary>
    long? IconFileId,
    /// <summary>UsageCount</summary>
    int UsageCount,
    /// <summary>IsSystemGenerated</summary>
    bool IsSystemGenerated)
;

public sealed record FgsTagPatchDto(
    /// <summary>TagCode</summary>
    string? TagCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>NormalizedName</summary>
    string? NormalizedName,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BackgroundColor</summary>
    string? BackgroundColor,
    /// <summary>TextColor</summary>
    string? TextColor,
    /// <summary>IconFileId</summary>
    long? IconFileId,
    /// <summary>UsageCount</summary>
    int? UsageCount,
    /// <summary>IsSystemGenerated</summary>
    bool? IsSystemGenerated)
;

