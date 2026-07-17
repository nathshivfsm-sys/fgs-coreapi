using Fgs.User.Application.Features.ApiWebhooks.Dtos;

namespace Fgs.User.Infrastructure.Entities.ApiWebhooks;

internal sealed class FgsApiWebhookSummaryRow
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string EndpointUrl { get; set; } = null!;

    public string AuthenticationType { get; set; } = null!;

    public short TimeoutSeconds { get; set; }

    public short MaximumRetryCount { get; set; }

    public DateTimeOffset? LastSuccessfulDeliveryOn { get; set; }

    public bool IsActive { get; set; }

    public FgsApiWebhookSummaryDto ToDto() =>
        new(
            Id,
            Name,
            Description,
            EndpointUrl,
            AuthenticationType,
            TimeoutSeconds,
            MaximumRetryCount,
            LastSuccessfulDeliveryOn,
            IsActive);
}

internal sealed class FgsApiWebhookDetailRow
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string EndpointUrl { get; set; } = null!;

    public string AuthenticationType { get; set; } = null!;

    public string? AuthenticationValue { get; set; }

    public string? Secret { get; set; }

    public short TimeoutSeconds { get; set; }

    public short MaximumRetryCount { get; set; }

    public DateTimeOffset? LastSuccessfulDeliveryOn { get; set; }

    public bool IsActive { get; set; }

    public FgsApiWebhookDetailDto ToDto() =>
        new(
            Id,
            Name,
            Description,
            EndpointUrl,
            AuthenticationType,
            AuthenticationValue,
            Secret,
            TimeoutSeconds,
            MaximumRetryCount,
            LastSuccessfulDeliveryOn,
            IsActive);
}

internal sealed class FgsApiWebhookLookupRow
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string EndpointUrl { get; set; } = null!;

    public FgsApiWebhookLookupDto ToDto() => new(Id, Name, EndpointUrl);
}
