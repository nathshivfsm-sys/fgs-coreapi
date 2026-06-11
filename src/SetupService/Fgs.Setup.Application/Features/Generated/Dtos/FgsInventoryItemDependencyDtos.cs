namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsInventoryItemDependency</summary>
public sealed record FgsInventoryItemDependencySummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>DependentInventoryItemId</summary>
    long DependentInventoryItemId,
    /// <summary>Quantity</summary>
    decimal Quantity,
    /// <summary>DependencyType</summary>
    string? DependencyType,
    /// <summary>IsRequired</summary>
    bool IsRequired,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsInventoryItemDependencyDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>DependentInventoryItemId</summary>
    long DependentInventoryItemId,
    /// <summary>Quantity</summary>
    decimal Quantity,
    /// <summary>DependencyType</summary>
    string? DependencyType,
    /// <summary>IsRequired</summary>
    bool IsRequired,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
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

public sealed record FgsInventoryItemDependencyCreateDto(
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>DependentInventoryItemId</summary>
    long DependentInventoryItemId,
    /// <summary>Quantity</summary>
    decimal Quantity,
    /// <summary>DependencyType</summary>
    string? DependencyType,
    /// <summary>IsRequired</summary>
    bool IsRequired,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsInventoryItemDependencyUpdateDto(
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>DependentInventoryItemId</summary>
    long DependentInventoryItemId,
    /// <summary>Quantity</summary>
    decimal Quantity,
    /// <summary>DependencyType</summary>
    string? DependencyType,
    /// <summary>IsRequired</summary>
    bool IsRequired,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsInventoryItemDependencyPatchDto(
    /// <summary>InventoryItemId</summary>
    long? InventoryItemId,
    /// <summary>DependentInventoryItemId</summary>
    long? DependentInventoryItemId,
    /// <summary>Quantity</summary>
    decimal? Quantity,
    /// <summary>DependencyType</summary>
    string? DependencyType,
    /// <summary>IsRequired</summary>
    bool? IsRequired,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder)
;

