using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped sales disposition reason used when a Lead is Disqualified or an Opportunity is Lost.
/// </summary>
public class FgsSalesDispositionReason : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string DispositionReasonCode { get; set; } = null!;

    public string DispositionReasonName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; }

    public bool AppliesToOpportunity { get; set; }

    public bool RequireComment { get; set; }

    public bool IsTerminal { get; set; } = true;

    public bool AllowManualSelection { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; }

    public bool IsActive { get; set; } = true;
}
