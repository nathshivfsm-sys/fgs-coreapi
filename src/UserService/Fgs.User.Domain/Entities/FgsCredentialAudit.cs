namespace Fgs.User.Domain.Entities;

/// <summary>
/// Immutable audit trail; DDL only defines <see cref="CreatedOn"/> / <see cref="CreatedBy"/> (no updated columns).
/// </summary>
public class FgsCredentialAudit
{
    public Guid Id { get; set; }

    public Guid CredentialSecretId { get; set; }

    public string ActionType { get; set; } = null!;

    public string? Remarks { get; set; }

    public int? OldVersionNo { get; set; }

    public int? NewVersionNo { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }
}
