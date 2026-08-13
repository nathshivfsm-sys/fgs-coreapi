using Fgs.Setup.Application.Features.PriceBookItems.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.PriceBookItems;

internal sealed class FgsPriceBookItemSummaryRow
{
    public long Id { get; set; }
    public long PriceBookId { get; set; }
    public long? InventoryItemId { get; set; }
    public string? ItemCode { get; set; }
    public string ItemDescription { get; set; } = null!;
    public decimal Quantity { get; set; }
    public short DisplayOrder { get; set; }

    public FgsPriceBookItemSummaryDto ToDto() =>
        new(Id, PriceBookId, InventoryItemId, ItemCode, ItemDescription, Quantity, DisplayOrder);
}

internal sealed class FgsPriceBookItemDetailRow
{
    public long Id { get; set; }
    public long PriceBookId { get; set; }
    public long? InventoryItemId { get; set; }
    public string? ItemCode { get; set; }
    public string ItemDescription { get; set; } = null!;
    public decimal Quantity { get; set; }
    public short DisplayOrder { get; set; }
    public string? Notes { get; set; }

    public FgsPriceBookItemDetailDto ToDto() =>
        new(Id, PriceBookId, InventoryItemId, ItemCode, ItemDescription, Quantity, DisplayOrder, Notes);
}

internal sealed class FgsPriceBookItemLookupRow
{
    public long Id { get; set; }
    public long PriceBookId { get; set; }
    public string? ItemCode { get; set; }
    public string ItemDescription { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsPriceBookItemLookupDto ToDto() =>
        new(Id, PriceBookId, ItemCode, ItemDescription, DisplayOrder);
}
