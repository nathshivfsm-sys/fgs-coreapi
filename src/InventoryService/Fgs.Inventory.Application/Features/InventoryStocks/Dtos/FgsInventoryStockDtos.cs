namespace Fgs.Inventory.Application.Features.InventoryStocks.Dtos;

public sealed record FgsInventoryStockSummaryDto(
    long Id,
    long InventoryItemId,
    decimal QuantityOnHand,
    decimal QuantityCommitted,
    decimal QuantityAvailable,
    decimal AverageCost,
    decimal LastCost,
    DateTimeOffset? LastPurchaseDate,
    DateTimeOffset? LastSoldDate,
    DateTimeOffset UpdatedOn);

public sealed record FgsInventoryStockDetailDto(
    long Id,
    long InventoryItemId,
    decimal QuantityOnHand,
    decimal QuantityCommitted,
    decimal QuantityAvailable,
    decimal AverageCost,
    decimal LastCost,
    DateTimeOffset? LastPurchaseDate,
    DateTimeOffset? LastSoldDate,
    DateTimeOffset UpdatedOn);

public sealed record FgsInventoryStockCreateDto(
    long InventoryItemId,
    decimal QuantityOnHand,
    decimal QuantityCommitted,
    decimal QuantityAvailable,
    decimal AverageCost,
    decimal LastCost,
    DateTimeOffset? LastPurchaseDate,
    DateTimeOffset? LastSoldDate);

public sealed record FgsInventoryStockUpdateDto(
    long InventoryItemId,
    decimal QuantityOnHand,
    decimal QuantityCommitted,
    decimal QuantityAvailable,
    decimal AverageCost,
    decimal LastCost,
    DateTimeOffset? LastPurchaseDate,
    DateTimeOffset? LastSoldDate);

public sealed record FgsInventoryStockPatchDto(
    long? InventoryItemId,
    decimal? QuantityOnHand,
    decimal? QuantityCommitted,
    decimal? QuantityAvailable,
    decimal? AverageCost,
    decimal? LastCost,
    DateTimeOffset? LastPurchaseDate,
    DateTimeOffset? LastSoldDate);

public sealed record FgsInventoryStockListFilters(
    long? InventoryItemId = null);
