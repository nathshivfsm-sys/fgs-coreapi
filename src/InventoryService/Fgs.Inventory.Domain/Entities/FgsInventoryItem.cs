using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Inventory item master record for purchasing, sales, and stock tracking.
/// </summary>
public class FgsInventoryItem : FgsTenantCompanySetupEntityBase<long>
{
    public long InventoryItemTypeId { get; set; }

    public long? InventoryCategoryId { get; set; }

    public long? InventorySubCategoryId { get; set; }

    public string ItemCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? PurchaseDescription { get; set; }

    public string? SalesDescription { get; set; }

    public string? ManufacturerPartNumber { get; set; }

    public string? ManufacturerName { get; set; }

    public string? Sku { get; set; }

    public string? UPCCode { get; set; }

    public string? UnitOfMeasure { get; set; }

    public bool TracksInventory { get; set; }

    /// <summary>Indicates whether individual serial numbers are tracked for this inventory item.</summary>
    public bool IsSerialized { get; set; }

    public decimal UnitCost { get; set; }

    public decimal StandardUnitCost { get; set; }

    public decimal SalesPrice { get; set; }

    public ICollection<FgsInventoryItemAlternate> Alternates { get; set; } = new List<FgsInventoryItemAlternate>();

    public ICollection<FgsInventoryItemDependency> Dependencies { get; set; } = new List<FgsInventoryItemDependency>();
}
