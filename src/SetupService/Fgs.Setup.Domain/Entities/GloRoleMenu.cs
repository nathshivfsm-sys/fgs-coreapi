namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global default mapping of standard roles to menu items used to seed tenant role menu assignments during onboarding.
/// </summary>
public class GloRoleMenu
{
    public short RoleId { get; set; }

    public int MenuId { get; set; }

    public short SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public GloRole? Role { get; set; }

    public GloMenu? Menu { get; set; }
}
