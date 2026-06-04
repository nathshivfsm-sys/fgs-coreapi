namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped lead source catalog.
/// </summary>
public class FgsLeadSource : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string SourceCode { get; set; } = null!;

    public string SourceName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
