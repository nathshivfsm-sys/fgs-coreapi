namespace Fgs.User.Domain.Entities;

/// <summary>
/// Stores the menu items assigned to each role within a tenant company and defines which navigation items the role can access.
/// </summary>
public class FgsRoleMenu : FgsTenantCompanySetupEntityBase<long>
{
    public long RoleId { get; set; }

    public int MenuId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public FgsRole? Role { get; set; }
}
