namespace Fgs.User.Domain.Entities;

/// <summary>
/// Common <c>IsActive</c> flag for global catalog rows.
/// </summary>
public abstract class GloActiveOnlyEntityBase
{
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Global geo-style rows: active flag and creation timestamp (no <c>UpdatedOn</c> in schema).
/// </summary>
public abstract class GloGeoEntityBase : GloActiveOnlyEntityBase
{
    public DateTimeOffset CreatedOn { get; set; }
}

/// <summary>
/// Typical int-key global catalog: lifecycle timestamps and active flag.
/// </summary>
public abstract class GloIntCatalogEntityBase : GloActiveOnlyEntityBase
{
    public int Id { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }
}

/// <summary>
/// Optional audit fields for global tables that allow unknown creation time (e.g. <see cref="GloMasterEntityType"/>).
/// </summary>
public abstract class GloOptionalAuditEntityBase
{
    public DateTimeOffset? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}

/// <summary>
/// Minimal global lookup: int key, code, and display name (e.g. <see cref="GloTimeCardOption"/>).
/// </summary>
public abstract class GloCodeNameLookupBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
