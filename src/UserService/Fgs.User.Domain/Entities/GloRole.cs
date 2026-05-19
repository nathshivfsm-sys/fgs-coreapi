namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global system role shared across all tenants.
/// </summary>
public class GloRole
{
    public short Id { get; set; }

    public string RoleCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    /// <summary>SYSTEM, TENANT, COMPANY, or FIELD.</summary>
    public string RoleLevel { get; set; } = null!;

    public bool IsAssignable { get; set; } = true;

    public bool IsSystemRole { get; set; }

    public short SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
