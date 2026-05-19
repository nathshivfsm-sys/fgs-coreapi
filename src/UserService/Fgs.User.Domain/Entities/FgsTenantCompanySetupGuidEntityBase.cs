namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant/company setup row with <see cref="Guid"/> primary key.
/// </summary>
public abstract class FgsTenantCompanySetupGuidEntityBase : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public long CompanyId { get; set; }

    public bool IsActive { get; set; } = true;
}
