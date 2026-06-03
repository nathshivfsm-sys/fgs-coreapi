namespace Fgs.Kernel.Entities;

/// <summary>
/// Marks entities scoped to a tenant and company (CompanyId maps to FgsTenantCompany.CompanyNumber).
/// </summary>
public interface ITenantCompanyScoped
{
    long TenantId { get; }

    long CompanyId { get; }
}
