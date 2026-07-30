namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;

public sealed record FgsTruckStockTemplateItemSummaryDto(
    long Id,
    long TruckStockTemplateId,
    long InventoryItemId,
    decimal TargetQuantity,
    decimal MinimumQuantity,
    int DisplayOrder);

public sealed record FgsTruckStockTemplateItemDetailDto(
    long Id,
    long TruckStockTemplateId,
    long InventoryItemId,
    decimal TargetQuantity,
    decimal MinimumQuantity,
    int DisplayOrder);

public sealed record FgsTruckStockTemplateItemCreateDto(
    long InventoryItemId,
    decimal TargetQuantity,
    decimal MinimumQuantity,
    int DisplayOrder = 1);

public sealed record FgsTruckStockTemplateItemUpdateDto(
    long InventoryItemId,
    decimal TargetQuantity,
    decimal MinimumQuantity,
    int DisplayOrder);

public sealed record FgsTruckStockTemplateItemPatchDto(
    long? InventoryItemId,
    decimal? TargetQuantity,
    decimal? MinimumQuantity,
    int? DisplayOrder);

public sealed record FgsTruckStockTemplateItemListFilters(
    long? InventoryItemId = null);
