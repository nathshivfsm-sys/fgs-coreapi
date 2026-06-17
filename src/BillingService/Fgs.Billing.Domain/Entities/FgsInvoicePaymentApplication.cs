using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsInvoicePaymentApplication : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long PaymentId { get; set; }

    public long InvoiceId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public decimal AppliedAmount { get; set; }

    public DateTime AppliedOn { get; set; }

    public string? ApplicationNote { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
