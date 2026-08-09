using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Inventory item and desired stocking quantities for a truck stock template.
/// </summary>
public class FgsTruckStockTemplateItem : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long TruckStockTemplateId { get; set; }

    public long InventoryItemId { get; set; }

    public decimal TargetQuantity { get; set; }

    public decimal MinimumQuantity { get; set; }

    public int DisplayOrder { get; set; } = 1;

    public FgsTruckStockTemplate? TruckStockTemplate { get; set; }

    public FgsInventoryItem? InventoryItem { get; set; }
}
