using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Immutable audit log of inventory movements between locations.
/// </summary>
public class FgsInventoryTransaction : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string TransactionNumber { get; set; } = null!;

    public long InventoryItemId { get; set; }

    /// <summary>Serial number of the inventory item involved in the transaction. Null for non-serialized inventory items.</summary>
    public string? SerialNumber { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Quantity { get; set; }

    public long? FromInventoryLocationId { get; set; }

    public long? ToInventoryLocationId { get; set; }

    public decimal UnitCost { get; set; }

    public DateTimeOffset TransactionDate { get; set; }

    public string? ReferenceType { get; set; }

    public long? ReferenceId { get; set; }

    public string? Notes { get; set; }
}
