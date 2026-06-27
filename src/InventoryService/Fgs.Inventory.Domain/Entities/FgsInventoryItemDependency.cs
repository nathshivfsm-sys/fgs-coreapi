using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Dependent inventory item required or bundled with a parent item (e.g. kit components).
/// </summary>
public class FgsInventoryItemDependency : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long InventoryItemId { get; set; }

    public long DependentInventoryItemId { get; set; }

    public decimal Quantity { get; set; } = 1;

    public bool IsRequired { get; set; } = true;

    public string? Notes { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;
}
