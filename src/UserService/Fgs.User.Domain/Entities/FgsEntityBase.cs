namespace Fgs.User.Domain.Entities;

/// <summary>
/// Auditing fields shared by FGS platform tables (maps to PostgreSQL <c>timestamptz</c> / <c>uuid</c> columns).
/// </summary>
public abstract class FgsEntityBase
{
    public DateTimeOffset CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}
