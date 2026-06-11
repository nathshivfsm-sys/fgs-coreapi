namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsInventoryItemType</summary>
public sealed record FgsInventoryItemTypeSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ItemTypeCode</summary>
    string? ItemTypeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>TracksQuantity</summary>
    bool TracksQuantity,
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

public sealed record FgsInventoryItemTypeDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ItemTypeCode</summary>
    string? ItemTypeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>TracksQuantity</summary>
    bool TracksQuantity,
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

public sealed record FgsInventoryItemTypeCreateDto(
    /// <summary>ItemTypeCode</summary>
    string? ItemTypeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>TracksQuantity</summary>
    bool TracksQuantity,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsInventoryItemTypeUpdateDto(
    /// <summary>ItemTypeCode</summary>
    string? ItemTypeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>TracksQuantity</summary>
    bool TracksQuantity,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>IsSystem</summary>
    bool IsSystem)
;

public sealed record FgsInventoryItemTypePatchDto(
    /// <summary>ItemTypeCode</summary>
    string? ItemTypeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>TracksQuantity</summary>
    bool? TracksQuantity,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder,
    /// <summary>IsSystem</summary>
    bool? IsSystem)
;

