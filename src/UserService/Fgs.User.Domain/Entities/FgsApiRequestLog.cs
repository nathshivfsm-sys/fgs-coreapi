namespace Fgs.User.Domain.Entities;

/// <summary>
/// API request metadata for monitoring, troubleshooting, rate limiting and analytics.
/// </summary>
public class FgsApiRequestLog : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long FgsApiClientId { get; set; }

    public Guid RequestId { get; set; }

    public string? Resource { get; set; }

    public string HttpMethod { get; set; } = null!;

    public string Endpoint { get; set; } = null!;

    public short HttpStatusCode { get; set; }

    public int DurationMilliseconds { get; set; }

    public string? ClientIpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTimeOffset RequestedOn { get; set; }

    public FgsApiClient? FgsApiClient { get; set; }
}
