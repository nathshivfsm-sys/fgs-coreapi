using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmLead : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long LeadStatusId { get; set; }

    public long LeadSourceId { get; set; }

    public long? CampaignId { get; set; }

    public string Name { get; set; } = null!;

    public string? LeadDescription { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public long? PrimaryContactMethodId { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public long? AssignedToUserId { get; set; }

    public long? CustomerId { get; set; }

    public long? ServiceLocationId { get; set; }

    public DateTimeOffset LeadReceivedOn { get; set; }

    public long? DisqualificationReasonId { get; set; }

    public DateTimeOffset? DisqualifiedOn { get; set; }

    public long? ConvertedOpportunityId { get; set; }

    public DateTimeOffset? ConvertedOn { get; set; }
}
