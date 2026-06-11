using Fgs.Kernel.Entities;

namespace Fgs.ServiceAgreement.Domain.Entities;

public class FgsServiceAgreementVisit : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long ServiceAgreementId { get; set; }
    public int VisitNumber { get; set; }
    public long JobTypeId { get; set; }
    public DateOnly ExpectedServiceDate { get; set; }
    public short ServiceAgreementVisitStatusId { get; set; }
    public long? WorkOrderId { get; set; }
    public string? ExternalWorkOrderNumber { get; set; }
    public DateTimeOffset? CompletedOn { get; set; }

    public FgsServiceAgreement ServiceAgreement { get; set; } = null!;
}
