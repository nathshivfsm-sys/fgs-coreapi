using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsInvoiceDetail : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long InvoiceId { get; set; }

    public long? ParentLineId { get; set; }

    public int LineNumber { get; set; }

    public int BillingCategoryId { get; set; }

    public string? ItemCode { get; set; }

    public string ItemDescription { get; set; } = null!;

    public bool IsInventory { get; set; }

    public string? MasterPartNum { get; set; }

    public long? InventoryItemId { get; set; }

    public long? PriceBookItemId { get; set; }

    public int? LaborRateTypeId { get; set; }

    public long? TechnicianId { get; set; }

    public decimal Quantity { get; set; } = 1;

    public decimal UnitCost { get; set; }

    public decimal ExtendedCost { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal ExtendedPrice { get; set; }

    public bool IsTaxable { get; set; }

    public int? GLBreak1Id { get; set; }

    public int? GLBreak2Id { get; set; }

    public string? LineAddedFrom { get; set; }

    public long? LineAddedFromId { get; set; }

    public string? AddedSource { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
