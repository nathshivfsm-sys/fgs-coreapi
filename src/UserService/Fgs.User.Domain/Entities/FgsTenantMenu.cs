namespace Fgs.User.Domain.Entities;

/// <summary>
/// Stores the menu items enabled for a company within a tenant based on the tenant subscription and available platform features.
/// </summary>
public class FgsTenantMenu : FgsTenantCompanySetupEntityBase<long>
{
    public int MenuId { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
