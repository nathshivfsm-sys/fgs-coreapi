using Fgs.Kernel.Entities;

namespace Fgs.ServiceAgreement.Domain.Entities;

public class FgsServiceAgreementBillingSchedule : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long ServiceAgreementId { get; set; }
    public int BillingSequence { get; set; }
    public DateOnly BillingDate { get; set; }
    public decimal BillingAmount { get; set; }
    public short BillingScheduleStatusId { get; set; }
    public long? InvoiceId { get; set; }
    public string? ExternalInvoiceNumber { get; set; }

    public FgsServiceAgreement ServiceAgreement { get; set; } = null!;
}
