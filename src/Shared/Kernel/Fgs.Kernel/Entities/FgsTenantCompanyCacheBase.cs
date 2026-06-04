namespace Fgs.Kernel.Entities;

/// <summary>
/// Local cache of tenant company identity (CompanyId maps to tenant.FgsTenantCompany.CompanyNumber).
/// </summary>
public class FgsTenantCompanyCacheBase
{
    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public Guid CompanyGuid { get; set; }

    public string CompanyCode { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset? UpdatedOn { get; set; }
}
