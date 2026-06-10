namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsWarehouse</summary>
public sealed record FgsWarehouseSummaryDto(
    /// <summary>Primary key.</summary>
    long Id,
    /// <summary>Tenant identifier.</summary>
    long TenantId,
    /// <summary>Company identifier.</summary>
    long CompanyId,
    /// <summary>Unique warehouse code within the tenant and company scope.</summary>
    string? WarehouseCode,
    /// <summary>Display name of the warehouse or inventory location.</summary>
    string? Name,
    /// <summary>Optional reference to the warehouse address record.</summary>
    string? WarehouseType,
    /// <summary>AddressId</summary>
    Guid? AddressId,
    /// <summary>Optional description or notes for the warehouse.</summary>
    string? Description,
    /// <summary>Indicates whether this warehouse is the default inventory location for the company.</summary>
    bool IsDefault,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the warehouse is active and available for inventory operations.</summary>
    bool IsActive)
;

public sealed record FgsWarehouseDetailDto(
    /// <summary>Primary key.</summary>
    long Id,
    /// <summary>Tenant identifier.</summary>
    long TenantId,
    /// <summary>Company identifier.</summary>
    long CompanyId,
    /// <summary>Unique warehouse code within the tenant and company scope.</summary>
    string? WarehouseCode,
    /// <summary>Display name of the warehouse or inventory location.</summary>
    string? Name,
    /// <summary>Optional reference to the warehouse address record.</summary>
    string? WarehouseType,
    /// <summary>AddressId</summary>
    Guid? AddressId,
    /// <summary>Optional description or notes for the warehouse.</summary>
    string? Description,
    /// <summary>Indicates whether this warehouse is the default inventory location for the company.</summary>
    bool IsDefault,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the warehouse is active and available for inventory operations.</summary>
    bool IsActive)
;

public sealed record FgsWarehouseCreateDto(
    /// <summary>Unique warehouse code within the tenant and company scope.</summary>
    string? WarehouseCode,
    /// <summary>Display name of the warehouse or inventory location.</summary>
    string? Name,
    /// <summary>Optional reference to the warehouse address record.</summary>
    string? WarehouseType,
    /// <summary>AddressId</summary>
    Guid? AddressId,
    /// <summary>Optional description or notes for the warehouse.</summary>
    string? Description,
    /// <summary>Indicates whether this warehouse is the default inventory location for the company.</summary>
    bool IsDefault)
;

public sealed record FgsWarehouseUpdateDto(
    /// <summary>Unique warehouse code within the tenant and company scope.</summary>
    string? WarehouseCode,
    /// <summary>Display name of the warehouse or inventory location.</summary>
    string? Name,
    /// <summary>Optional reference to the warehouse address record.</summary>
    string? WarehouseType,
    /// <summary>AddressId</summary>
    Guid? AddressId,
    /// <summary>Optional description or notes for the warehouse.</summary>
    string? Description,
    /// <summary>Indicates whether this warehouse is the default inventory location for the company.</summary>
    bool IsDefault)
;

public sealed record FgsWarehousePatchDto(
    /// <summary>Unique warehouse code within the tenant and company scope.</summary>
    string? WarehouseCode,
    /// <summary>Display name of the warehouse or inventory location.</summary>
    string? Name,
    /// <summary>Optional reference to the warehouse address record.</summary>
    string? WarehouseType,
    /// <summary>AddressId</summary>
    Guid? AddressId,
    /// <summary>Optional description or notes for the warehouse.</summary>
    string? Description,
    /// <summary>Indicates whether this warehouse is the default inventory location for the company.</summary>
    bool? IsDefault)
;

