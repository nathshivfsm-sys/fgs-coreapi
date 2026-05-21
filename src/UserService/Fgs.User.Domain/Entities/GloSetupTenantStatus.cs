namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of tenant lifecycle statuses (e.g. prospect, active, suspended).
/// </summary>
public class GloSetupTenantStatus
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public long? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
