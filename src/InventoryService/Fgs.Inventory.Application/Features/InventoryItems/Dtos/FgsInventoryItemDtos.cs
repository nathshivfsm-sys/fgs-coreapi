namespace Fgs.Inventory.Application.Features.InventoryItems.Dtos;

public sealed record FgsInventoryItemSummaryDto(
    long Id,
    string ItemCode,
    string Name,
    long InventoryItemTypeId,
    long? InventoryCategoryId,
    long? InventorySubCategoryId,
    bool TracksInventory,
    bool IsSerialized,
    decimal UnitCost,
    decimal SalesPrice,
    bool IsActive);

public sealed record FgsInventoryItemDetailDto(
    long Id,
    long InventoryItemTypeId,
    long? InventoryCategoryId,
    long? InventorySubCategoryId,
    string ItemCode,
    string Name,
    string? Description,
    string? PurchaseDescription,
    string? SalesDescription,
    string? ManufacturerPartNumber,
    string? ManufacturerName,
    string? Sku,
    string? UPCCode,
    string? UnitOfMeasure,
    bool TracksInventory,
    bool IsSerialized,
    decimal UnitCost,
    decimal StandardUnitCost,
    decimal SalesPrice,
    bool IsActive,
    IReadOnlyList<FgsInventoryItemAlternateDetailDto> Alternates,
    IReadOnlyList<FgsInventoryItemDependencyDetailDto> Dependencies);

public sealed record FgsInventoryItemLookupDto(
    long Id,
    string ItemCode,
    string Name);

public sealed record FgsInventoryItemAlternateDetailDto(
    long Id,
    long AlternateInventoryItemId,
    short PriorityOrder,
    string? Notes,
    bool IsActive);

public sealed record FgsInventoryItemDependencyDetailDto(
    long Id,
    long DependentInventoryItemId,
    decimal Quantity,
    bool IsRequired,
    string? Notes,
    short DisplayOrder,
    bool IsActive);

/// <summary>
/// Alternate relationship row. Omit <see cref="Id"/> (or null) to insert; supply Id to update.
/// Omitted existing rows are removed on full-replace create/update.
/// </summary>
public sealed record FgsInventoryItemAlternateDto(
    long? Id,
    long AlternateInventoryItemId,
    short PriorityOrder = 1,
    string? Notes = null,
    bool IsActive = true);

/// <summary>
/// Dependency relationship row. Omit <see cref="Id"/> (or null) to insert; supply Id to update.
/// Omitted existing rows are removed on full-replace create/update.
/// </summary>
public sealed record FgsInventoryItemDependencyDto(
    long? Id,
    long DependentInventoryItemId,
    decimal Quantity = 1m,
    bool IsRequired = true,
    string? Notes = null,
    short DisplayOrder = 1,
    bool IsActive = true);

public sealed record FgsInventoryItemAlternateReplaceDto(
    long InventoryItemId,
    IReadOnlyList<FgsInventoryItemAlternateDto> Items);

public sealed record FgsInventoryItemDependencyReplaceDto(
    long InventoryItemId,
    IReadOnlyList<FgsInventoryItemDependencyDto> Items);

public sealed record FgsInventoryItemCreateDto(
    long InventoryItemTypeId,
    string ItemCode,
    string Name,
    string? Description = null,
    string? PurchaseDescription = null,
    string? SalesDescription = null,
    string? ManufacturerPartNumber = null,
    string? ManufacturerName = null,
    string? Sku = null,
    string? UPCCode = null,
    string? UnitOfMeasure = null,
    bool TracksInventory = false,
    bool IsSerialized = false,
    decimal UnitCost = 0m,
    decimal StandardUnitCost = 0m,
    decimal SalesPrice = 0m,
    long? InventoryCategoryId = null,
    long? InventorySubCategoryId = null);

public sealed record FgsInventoryItemUpdateDto(
    long InventoryItemTypeId,
    string ItemCode,
    string Name,
    string? Description,
    string? PurchaseDescription,
    string? SalesDescription,
    string? ManufacturerPartNumber,
    string? ManufacturerName,
    string? Sku,
    string? UPCCode,
    string? UnitOfMeasure,
    bool TracksInventory,
    bool IsSerialized,
    decimal UnitCost,
    decimal StandardUnitCost,
    decimal SalesPrice,
    long? InventoryCategoryId,
    long? InventorySubCategoryId);

public sealed record FgsInventoryItemPatchDto(
    long? InventoryItemTypeId,
    string? ItemCode,
    string? Name,
    string? Description,
    string? PurchaseDescription,
    string? SalesDescription,
    string? ManufacturerPartNumber,
    string? ManufacturerName,
    string? Sku,
    string? UPCCode,
    string? UnitOfMeasure,
    bool? TracksInventory,
    bool? IsSerialized,
    decimal? UnitCost,
    decimal? StandardUnitCost,
    decimal? SalesPrice,
    bool? IsActive,
    long? InventoryCategoryId,
    long? InventorySubCategoryId);

public sealed record FgsInventoryItemListFilters(
    string? ItemCode = null,
    string? Name = null,
    long? InventoryItemTypeId = null);
