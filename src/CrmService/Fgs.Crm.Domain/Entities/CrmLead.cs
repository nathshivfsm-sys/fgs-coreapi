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

    public string LeadSummary { get; set; } = null!;

    public string? LeadDescription { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public long? CustomerTypeId { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public long? PrimaryContactMethodId { get; set; }

    public string? ServiceZipCode { get; set; }

    public long? AssignedToUserId { get; set; }

    public DateTimeOffset LeadReceivedOn { get; set; }

    public DateTimeOffset? QualifiedOn { get; set; }

    public DateTimeOffset? DisqualifiedOn { get; set; }

    public long? DisqualificationReasonId { get; set; }

    public long? CustomerId { get; set; }

    public DateTimeOffset? ConvertedOn { get; set; }
}
