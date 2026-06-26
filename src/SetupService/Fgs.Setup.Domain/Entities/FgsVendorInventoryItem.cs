namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Vendor-specific inventory item mapping, pricing, and purchasing defaults.
/// </summary>
public class FgsVendorInventoryItem : FgsTenantCompanySetupEntityBase<long>
{
    public long VendorId { get; set; }

    public long InventoryItemId { get; set; }

    public string? VendorPartNumber { get; set; }

    public string? VendorPartName { get; set; }

    public decimal LastCost { get; set; }

    public DateTimeOffset? LastReceivedDate { get; set; }

    public string? PurchaseOrderComments { get; set; }

    public bool IsPreferredVendor { get; set; }
}
