using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Defines the inventory, non-inventory, and free-form items that make up a price book service.
/// </summary>
public class FgsPriceBookItem : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long PriceBookId { get; set; }

    public long? InventoryItemId { get; set; }

    public string? ItemCode { get; set; }

    public string ItemDescription { get; set; } = null!;

    public decimal Quantity { get; set; } = 1;

    public short DisplayOrder { get; set; } = 1;

    public string? Notes { get; set; }

    public FgsPriceBook? PriceBook { get; set; }
}
