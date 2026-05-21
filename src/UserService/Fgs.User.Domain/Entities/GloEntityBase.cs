namespace Fgs.User.Domain.Entities;

/// <summary>
/// Common properties for global catalog rows.
/// </summary>
public abstract class GloEntityBase
{
    public bool IsActive { get; set; } = true;

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
