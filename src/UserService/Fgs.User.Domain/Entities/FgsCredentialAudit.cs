namespace Fgs.User.Domain.Entities;

/// <summary>
/// Immutable audit trail for credential secret changes (no updated columns).
/// </summary>
public class FgsCredentialAudit : ITenantCompanyScoped
{
    public Guid Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public Guid CredentialSecretId { get; set; }

    public string ActionType { get; set; } = null!;

    public string? Remarks { get; set; }

    public int? OldVersionNo { get; set; }

    public int? NewVersionNo { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
