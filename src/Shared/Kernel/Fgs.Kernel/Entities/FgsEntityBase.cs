namespace Fgs.Kernel.Entities;

/// <summary>
/// Auditing fields shared by FGS platform tables (maps to PostgreSQL <c>timestamptz</c> / <c>varchar</c> columns).
/// </summary>
public abstract class FgsEntityBase : IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }
}
