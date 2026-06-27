using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsWorkOrder : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string WorkOrderNumber { get; set; } = null!;
    public long? ProjectId { get; set; }
    public long CustomerId { get; set; }
    public long LocationId { get; set; }
    public long? ServiceAgreementId { get; set; }
    public long? ServiceAgreementVisitId { get; set; }
    public long? Break1Id { get; set; }
    public long? Break2Id { get; set; }
    public long JobTypeId { get; set; }
    public long PriorityId { get; set; }
    public long WorkOrderStatusId { get; set; }
    public long? WorkOrderResolutionId { get; set; }
    public long? TimeSlotId { get; set; }
    public string? CustomerPO { get; set; }
    public string? PersonCalling { get; set; }
    public string? PersonCallingPhoneNumber { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPersonPhoneNumber { get; set; }
    public string? ProblemDescription { get; set; }
    public string? Note { get; set; }
    public long? MaterialPricingMatrixId { get; set; }
    public long? LaborPricingMatrixId { get; set; }
    public long? OtherPricingMatrixId { get; set; }
    public long? PaymentMethodId { get; set; }
    public decimal? EstimatedHours { get; set; }
    public DateTimeOffset RequestedOn { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string? Source { get; set; }
}
