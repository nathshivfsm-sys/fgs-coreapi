namespace Fgs.User.Domain.Entities;

/// <summary>
/// Built-in platform role or company-defined custom role.
/// Built-in roles are immutable and may only be cloned.
/// </summary>
public class FgsRole : FgsTenantCompanySetupEntityBase<long>
{
    public long? ParentRoleId { get; set; }

    public string RoleCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public FgsRole? ParentRole { get; set; }

    public ICollection<FgsRole> ChildRoles { get; set; } = [];
}
