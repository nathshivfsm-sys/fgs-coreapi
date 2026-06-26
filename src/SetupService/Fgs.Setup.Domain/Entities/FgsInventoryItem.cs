namespace Fgs.Setup.Domain.Entities;

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

    public string? UPCCode { get; set; }

    public string? UnitOfMeasure { get; set; }

    public bool TrackQuantity { get; set; }

    public decimal Cost { get; set; }

    public decimal SalesPrice { get; set; }

    public bool DefaultTaxable { get; set; } = true;
}
