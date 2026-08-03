using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Purchase order line item.
/// </summary>
public class FgsPurchaseOrderDetail : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long PurchaseOrderId { get; set; }

    public short LineNumber { get; set; }

    public long ItemId { get; set; }

    public string? VendorPartNumber { get; set; }

    public string ItemDescription { get; set; } = null!;

    public string UnitOfMeasureCode { get; set; } = null!;

    public decimal OrderedQuantity { get; set; }

    public decimal ReceivedQuantity { get; set; }

    public decimal UnitCost { get; set; }

    public decimal DiscountAmount { get; set; }

    public bool IsTaxable { get; set; } = true;

    public decimal ExtendedAmount { get; set; }

    public DateTimeOffset? ExpectedDeliveryDate { get; set; }

    public string? Notes { get; set; }

    public FgsPurchaseOrder? PurchaseOrder { get; set; }
}
