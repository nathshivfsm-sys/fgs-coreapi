namespace Fgs.User.Domain.Entities;

/// <summary>
/// Master catalog of all permissions supported by the platform.
/// Permissions are seeded by the application and assigned to security roles.
/// </summary>
public class FgsPermission
{
    public long Id { get; set; }

    public string PermissionCode { get; set; } = null!;

    public string Module { get; set; } = null!;

    public string Resource { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
