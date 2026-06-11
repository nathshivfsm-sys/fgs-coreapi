using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped sales activity outcome used by Leads and Opportunities.
/// </summary>
public class FgsSalesActivityOutcome : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string OutcomeCode { get; set; } = null!;

    public string OutcomeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; } = true;

    public bool AppliesToOpportunity { get; set; } = true;

    public long? NextSalesPipelineStatusId { get; set; }

    public bool IsTerminal { get; set; }

    public bool RequireComment { get; set; }

    public bool AllowManualSelection { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; }

    public bool IsActive { get; set; } = true;
}
