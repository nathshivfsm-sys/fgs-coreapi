using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsInvoiceWorkDescription : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long InvoiceId { get; set; }

    public DateOnly ServiceDate { get; set; }

    public string? TechCode { get; set; }

    public string UserName { get; set; } = null!;

    public string WorkDescription { get; set; } = null!;

    public bool IsCustomerVisible { get; set; } = true;

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
