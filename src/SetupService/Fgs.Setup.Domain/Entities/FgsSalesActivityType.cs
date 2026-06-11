using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped sales activity type used by Leads and Opportunities.
/// </summary>
public class FgsSalesActivityType : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string ActivityTypeCode { get; set; } = null!;

    public string ActivityTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; } = true;

    public bool AppliesToOpportunity { get; set; } = true;

    public bool AllowManualSelection { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; }

    public bool IsActive { get; set; } = true;
}
