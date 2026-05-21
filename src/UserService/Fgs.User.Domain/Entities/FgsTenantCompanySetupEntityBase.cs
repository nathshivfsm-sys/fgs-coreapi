namespace Fgs.User.Domain.Entities;

/// <summary>
/// Common shape for tenant- and company-scoped <c>FgsSetup*</c> catalog rows (see platform naming rules).
/// </summary>
public abstract class FgsTenantCompanySetupEntityBase : FgsEntityBase
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public bool IsActive { get; set; } = true;
}
