namespace Fgs.User.Domain.Entities;

/// <summary>
/// Developer application created by tenant administrators for third-party integrations.
/// Represents an application, not a credential.
/// </summary>
public class FgsApiClient : FgsTenantCompanySetupEntityBase<long>
{
    public Guid ClientId { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }

    public int RateLimitPerMinute { get; set; } = 60;

    public ICollection<FgsApiSecret> Secrets { get; set; } = [];

    public ICollection<FgsApiRequestLog> RequestLogs { get; set; } = [];
}
