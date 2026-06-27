using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Quantity-on-hand and cost snapshot for an inventory item.
/// </summary>
public class FgsInventoryStock : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long InventoryItemId { get; set; }

    public decimal QuantityOnHand { get; set; }

    public decimal QuantityCommitted { get; set; }

    public decimal QuantityAvailable { get; set; }

    public decimal AverageCost { get; set; }

    public decimal LastCost { get; set; }

    public DateTimeOffset? LastPurchaseDate { get; set; }

    public DateTimeOffset? LastSoldDate { get; set; }

    public DateTimeOffset UpdatedOn { get; set; }
}
