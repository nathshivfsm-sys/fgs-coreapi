namespace Fgs.ServiceAgreement.Domain.Enums;

public enum ServiceAgreementVisitStatus : short
{
    Pending = 1,
    WorkOrderCreated = 2,
    Completed = 3,
    Skipped = 4,
    Cancelled = 5
}
