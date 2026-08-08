namespace Fgs.Kernel.Entities;

/// <summary>
/// Common shape for tenant- and company-scoped setup catalog rows.
/// </summary>
public abstract class FgsTenantCompanySetupEntityBase<TKey> : FgsEntityBase, ITenantCompanyScoped, ISoftDeletable
{
    public TKey Id { get; set; } = default!;

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public bool IsActive { get; set; } = true;
}
