namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of permissions supported by the FGS platform.
/// Permissions define the actions that can be assigned to security roles.
/// </summary>
public class GloPermission
{
    public long Id { get; set; }

    public string PermissionCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
