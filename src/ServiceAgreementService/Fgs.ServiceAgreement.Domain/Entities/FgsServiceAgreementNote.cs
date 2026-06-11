using Fgs.Kernel.Entities;

namespace Fgs.ServiceAgreement.Domain.Entities;

public class FgsServiceAgreementNote : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long? ServiceAgreementId { get; set; }
    public long? ServiceAgreementVisitId { get; set; }
    public long? ServiceAgreementBillingScheduleId { get; set; }
    public long? NoteTypeId { get; set; }
    public string Note { get; set; } = null!;

    public FgsServiceAgreement? ServiceAgreement { get; set; }
    public FgsServiceAgreementVisit? ServiceAgreementVisit { get; set; }
    public FgsServiceAgreementBillingSchedule? ServiceAgreementBillingSchedule { get; set; }
}
