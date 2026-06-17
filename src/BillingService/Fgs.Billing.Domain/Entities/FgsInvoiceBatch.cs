using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsInvoiceBatch : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string BatchNumber { get; set; } = null!;

    public DateOnly BatchDate { get; set; }

    public int InvoiceCount { get; set; }

    public decimal InvoiceSubtotal { get; set; }

    public decimal TotalTax { get; set; }

    public decimal InvoiceTotal { get; set; }

    public bool IsClosed { get; set; }

    public DateTime? ClosedOn { get; set; }

    public long? ClosedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
