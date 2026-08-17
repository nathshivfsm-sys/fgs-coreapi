namespace Fgs.Setup.Application.Features.PriceBookItems.Dtos;

public sealed record FgsPriceBookItemSummaryDto(
    long Id,
    long PriceBookId,
    long? InventoryItemId,
    string? ItemCode,
    string ItemDescription,
    decimal Quantity,
    short DisplayOrder);

public sealed record FgsPriceBookItemDetailDto(
    long Id,
    long PriceBookId,
    long? InventoryItemId,
    string? ItemCode,
    string ItemDescription,
    decimal Quantity,
    short DisplayOrder,
    string? Notes);

public sealed record FgsPriceBookItemLookupDto(
    long Id,
    long PriceBookId,
    string? ItemCode,
    string ItemDescription,
    short DisplayOrder);

public sealed record FgsPriceBookItemCreateDto(
    long PriceBookId,
    long? InventoryItemId,
    string? ItemCode,
    string ItemDescription,
    decimal Quantity,
    short DisplayOrder,
    string? Notes);

public sealed record FgsPriceBookItemUpdateDto(
    long PriceBookId,
    long? InventoryItemId,
    string? ItemCode,
    string ItemDescription,
    decimal Quantity,
    short DisplayOrder,
    string? Notes);

public sealed record FgsPriceBookItemPatchDto(
    long? PriceBookId = null,
    long? InventoryItemId = null,
    string? ItemCode = null,
    string? ItemDescription = null,
    decimal? Quantity = null,
    short? DisplayOrder = null,
    string? Notes = null);

public sealed record FgsPriceBookItemListFilters(
    long? PriceBookId = null,
    string? ItemCode = null,
    string? ItemDescription = null);
