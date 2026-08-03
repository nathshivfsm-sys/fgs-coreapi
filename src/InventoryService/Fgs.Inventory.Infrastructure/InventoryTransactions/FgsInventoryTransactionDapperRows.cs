using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;

namespace Fgs.Inventory.Infrastructure.InventoryTransactions;

internal sealed class FgsInventoryTransactionSummaryRow
{
    public long Id { get; set; }
    public string TransactionNumber { get; set; } = null!;
    public long InventoryItemId { get; set; }
    public string TransactionType { get; set; } = null!;
    public decimal Quantity { get; set; }
    public long? FromInventoryLocationId { get; set; }
    public long? ToInventoryLocationId { get; set; }
    public decimal UnitCost { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public string? ReferenceType { get; set; }
    public long? ReferenceId { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsInventoryTransactionSummaryDto ToDto() =>
        new(
            Id,
            TransactionNumber,
            InventoryItemId,
            TransactionType,
            Quantity,
            FromInventoryLocationId,
            ToInventoryLocationId,
            UnitCost,
            TransactionDate,
            ReferenceType,
            ReferenceId,
            Notes,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsInventoryTransactionDetailRow
{
    public long Id { get; set; }
    public string TransactionNumber { get; set; } = null!;
    public long InventoryItemId { get; set; }
    public string TransactionType { get; set; } = null!;
    public decimal Quantity { get; set; }
    public long? FromInventoryLocationId { get; set; }
    public long? ToInventoryLocationId { get; set; }
    public decimal UnitCost { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public string? ReferenceType { get; set; }
    public long? ReferenceId { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsInventoryTransactionDetailDto ToDto() =>
        new(
            Id,
            TransactionNumber,
            InventoryItemId,
            TransactionType,
            Quantity,
            FromInventoryLocationId,
            ToInventoryLocationId,
            UnitCost,
            TransactionDate,
            ReferenceType,
            ReferenceId,
            Notes,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}
