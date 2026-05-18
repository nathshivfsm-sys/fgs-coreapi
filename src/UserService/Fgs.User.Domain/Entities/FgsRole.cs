namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant-defined custom role (optional override of a <see cref="GloRole"/>).
/// </summary>
public class FgsRole : FgsTenantCompanySetupEntityBase
{
    public string RoleCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short? GloRoleId { get; set; }
}
