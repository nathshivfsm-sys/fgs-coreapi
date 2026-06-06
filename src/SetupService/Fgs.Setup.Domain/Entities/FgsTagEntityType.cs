namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Defines which tags apply to which master entity types for a tenant/company.
/// </summary>
public class FgsTagEntityType : ITenantCompanyScoped
{
    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long TagId { get; set; }

    public int MasterEntityTypeId { get; set; }

    public bool IsDefault { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
