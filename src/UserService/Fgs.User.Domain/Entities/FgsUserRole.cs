namespace Fgs.User.Domain.Entities;

/// <summary>
/// Assigns a user to either a global (<see cref="GloRole"/>) or tenant custom (<see cref="FgsRole"/>) role.
/// </summary>
public class FgsUserRole
{
    public long Id { get; set; }

    public Guid UserId { get; set; }

    public Guid TenantId { get; set; }

    public long CompanyId { get; set; }

    public short? GloRoleId { get; set; }

    public long? FgsRoleId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public FgsUser? User { get; set; }

    public GloRole? GloRole { get; set; }

    public FgsRole? FgsRole { get; set; }
}
