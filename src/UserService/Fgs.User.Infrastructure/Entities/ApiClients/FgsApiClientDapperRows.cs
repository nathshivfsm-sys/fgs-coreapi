using Fgs.User.Application.Features.ApiClients.Dtos;

namespace Fgs.User.Infrastructure.Entities.ApiClients;

internal sealed class FgsApiClientSummaryRow
{
    public long Id { get; set; }

    public Guid ClientId { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }

    public int RateLimitPerMinute { get; set; }

    public bool IsActive { get; set; }

    public FgsApiClientSummaryDto ToDto() =>
        new(Id, ClientId, ApplicationName, Description, ContactName, ContactEmail, RateLimitPerMinute, IsActive);
}

internal sealed class FgsApiClientDetailRow
{
    public long Id { get; set; }

    public Guid ClientId { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }

    public int RateLimitPerMinute { get; set; }

    public bool IsActive { get; set; }

    public FgsApiClientDetailDto ToDto() =>
        new(Id, ClientId, ApplicationName, Description, ContactName, ContactEmail, RateLimitPerMinute, IsActive);
}

internal sealed class FgsApiClientLookupRow
{
    public long Id { get; set; }

    public Guid ClientId { get; set; }

    public string ApplicationName { get; set; } = null!;

    public FgsApiClientLookupDto ToDto() => new(Id, ClientId, ApplicationName);
}
