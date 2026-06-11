namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsInventorySubCategory</summary>
public sealed record FgsInventorySubCategorySummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>InventoryCategoryId</summary>
    long InventoryCategoryId,
    /// <summary>SubCategoryCode</summary>
    string? SubCategoryCode,
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

public sealed record FgsInventorySubCategoryDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>InventoryCategoryId</summary>
    long InventoryCategoryId,
    /// <summary>SubCategoryCode</summary>
    string? SubCategoryCode,
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

public sealed record FgsInventorySubCategoryCreateDto(
    /// <summary>InventoryCategoryId</summary>
    long InventoryCategoryId,
    /// <summary>SubCategoryCode</summary>
    string? SubCategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsInventorySubCategoryUpdateDto(
    /// <summary>InventoryCategoryId</summary>
    long InventoryCategoryId,
    /// <summary>SubCategoryCode</summary>
    string? SubCategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsInventorySubCategoryPatchDto(
    /// <summary>InventoryCategoryId</summary>
    long? InventoryCategoryId,
    /// <summary>SubCategoryCode</summary>
    string? SubCategoryCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder,
    /// <summary>IsSystem</summary>
    bool? IsSystem)
;

