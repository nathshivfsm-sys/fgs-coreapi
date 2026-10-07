namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped source catalog.
/// </summary>
public class FgsSource : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string SourceCode { get; set; } = null!;

    public string SourceName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
