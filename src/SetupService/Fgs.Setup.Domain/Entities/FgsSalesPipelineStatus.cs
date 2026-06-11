using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped sales pipeline status used by Leads and Opportunities.
/// </summary>
public class FgsSalesPipelineStatus : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; }

    public bool AppliesToOpportunity { get; set; }

    public bool IsTerminal { get; set; }

    public bool AllowManualSelection { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; }

    public bool IsActive { get; set; } = true;
}
