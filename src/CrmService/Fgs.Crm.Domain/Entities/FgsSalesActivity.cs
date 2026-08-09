using Fgs.Crm.Domain.Enums;
using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsSalesActivity : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long? LeadId { get; set; }

    public long? OpportunityId { get; set; }

    public long ActivityTypeId { get; set; }

    public long? AssignedToUserId { get; set; }

    public DateTimeOffset? ScheduledOn { get; set; }

    public decimal? EstimatedHours { get; set; }

    public DateTimeOffset? StartedOn { get; set; }

    public DateTimeOffset? CompletedOn { get; set; }

    public decimal? ActualHours { get; set; }

    public long? PerformedByUserId { get; set; }

    public long? SalesActivityOutcomeId { get; set; }

    public string? OutcomeDetails { get; set; }

    public string? Comments { get; set; }

    public bool RequiresFollowUp { get; set; }

    public DateTimeOffset? FollowUpOn { get; set; }

    public long? FollowUpActivityId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool IsSystemGenerated { get; set; }

    public SalesPriority Priority { get; set; } = SalesPriority.NORMAL;
}
