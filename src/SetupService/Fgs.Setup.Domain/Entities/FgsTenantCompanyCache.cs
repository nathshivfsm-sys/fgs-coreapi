namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Local cache of tenant company identity for CompanyGuid resolution without cross-schema FKs to tenant.
/// </summary>
public class FgsTenantCompanyCache
{
    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public Guid CompanyGuid { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset? UpdatedOn { get; set; }
}
