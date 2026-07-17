namespace Fgs.User.Domain.Entities;

/// <summary>
/// Hashed API secret associated with an API client.
/// Supports secret rotation, expiration, revocation and auditing.
/// </summary>
public class FgsApiSecret : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long FgsApiClientId { get; set; }

    public string Name { get; set; } = null!;

    public string SecretHash { get; set; } = null!;

    public DateTimeOffset? ExpiresOn { get; set; }

    public DateTimeOffset? LastUsedOn { get; set; }

    public DateTimeOffset? RevokedOn { get; set; }

    public string? RevokedBy { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsApiClient? FgsApiClient { get; set; }
}
