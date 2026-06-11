namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsInventoryCategory</summary>
public sealed record FgsInventoryCategorySummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsInventoryCategoryDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem,
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

public sealed record FgsInventoryCategoryCreateDto(
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsInventoryCategoryUpdateDto(
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsInventoryCategoryPatchDto(
    /// <summary>CategoryCode</summary>
    string? CategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder,
    /// <summary>IsSystem</summary>
    bool? IsSystem)
;

