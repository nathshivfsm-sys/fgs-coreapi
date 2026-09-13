namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global default mapping of standard roles to permissions used to seed tenant role permission assignments during onboarding.
/// </summary>
public class GloRolePermission
{
    public short RoleId { get; set; }

    public long PermissionId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public GloRole? Role { get; set; }

    public GloPermission? Permission { get; set; }
}
