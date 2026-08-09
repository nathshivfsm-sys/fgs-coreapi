using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;

namespace Fgs.Inventory.Infrastructure.InventoryStocks;

internal sealed class FgsInventoryStockSummaryRow
{
    public long Id { get; set; }
    public long InventoryItemId { get; set; }
    public decimal QuantityOnHand { get; set; }
    public decimal QuantityCommitted { get; set; }
    public decimal QuantityAvailable { get; set; }
    public decimal AverageCost { get; set; }
    public decimal LastCost { get; set; }
    public DateTimeOffset? LastPurchaseDate { get; set; }
    public DateTimeOffset? LastSoldDate { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }

    public FgsInventoryStockSummaryDto ToDto() =>
        new(
            Id,
            InventoryItemId,
            QuantityOnHand,
            QuantityCommitted,
            QuantityAvailable,
            AverageCost,
            LastCost,
            LastPurchaseDate,
            LastSoldDate,
            UpdatedOn);
}

internal sealed class FgsInventoryStockDetailRow
{
    public long Id { get; set; }
    public long InventoryItemId { get; set; }
    public decimal QuantityOnHand { get; set; }
    public decimal QuantityCommitted { get; set; }
    public decimal QuantityAvailable { get; set; }
    public decimal AverageCost { get; set; }
    public decimal LastCost { get; set; }
    public DateTimeOffset? LastPurchaseDate { get; set; }
    public DateTimeOffset? LastSoldDate { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }

    public FgsInventoryStockDetailDto ToDto() =>
        new(
            Id,
            InventoryItemId,
            QuantityOnHand,
            QuantityCommitted,
            QuantityAvailable,
            AverageCost,
            LastCost,
            LastPurchaseDate,
            LastSoldDate,
            UpdatedOn);
}
