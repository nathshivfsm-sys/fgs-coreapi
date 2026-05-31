namespace Fgs.Kernel.Entities;

/// <summary>
/// Marks entities with optional tenant/company scope (null = global default).
/// </summary>
public interface INullableTenantCompanyScoped
{
    long? TenantId { get; }

    long? CompanyId { get; }
}
