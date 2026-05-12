namespace Fgs.User.Domain.Entities;

/// <summary>
/// Immutable audit trail for credential secret changes (no updated columns).
/// </summary>
public class FgsCredentialAudit
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CredentialSecretId { get; set; }

    public string ActionType { get; set; } = null!;

    public string? Remarks { get; set; }

    public int? OldVersionNo { get; set; }

    public int? NewVersionNo { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }
}
