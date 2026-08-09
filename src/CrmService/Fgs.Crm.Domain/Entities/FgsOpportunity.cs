using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsOpportunity : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long? LeadId { get; set; }

    public long OpportunityStatusId { get; set; }

    public long? LeadSourceId { get; set; }

    public long? CampaignId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long CustomerId { get; set; }

    public long? ServiceLocationId { get; set; }

    public long? AssignedToUserId { get; set; }

    public decimal? EstimatedAmount { get; set; }

    public decimal? SoldAmount { get; set; }

    public DateTimeOffset? ExpectedCloseOn { get; set; }

    public DateTimeOffset? WonOn { get; set; }

    public DateTimeOffset? LostOn { get; set; }

    public long? DispositionReasonId { get; set; }

    public long? EstimateId { get; set; }

    public long? WorkOrderId { get; set; }
}
