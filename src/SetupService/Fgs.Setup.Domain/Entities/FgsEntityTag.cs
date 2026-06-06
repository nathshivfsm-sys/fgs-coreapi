namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Assigns a tenant tag to a specific master entity instance.
/// </summary>
public class FgsEntityTag : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long TagId { get; set; }

    public int MasterEntityTypeId { get; set; }

    public long EntityId { get; set; }

    public string? Notes { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
