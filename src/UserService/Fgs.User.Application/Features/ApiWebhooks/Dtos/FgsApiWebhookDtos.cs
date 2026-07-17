namespace Fgs.User.Application.Features.ApiWebhooks.Dtos;

public sealed record FgsApiWebhookSummaryDto(
    long Id,
    string Name,
    string? Description,
    string EndpointUrl,
    string AuthenticationType,
    short TimeoutSeconds,
    short MaximumRetryCount,
    DateTimeOffset? LastSuccessfulDeliveryOn,
    bool IsActive);

public sealed record FgsApiWebhookDetailDto(
    long Id,
    string Name,
    string? Description,
    string EndpointUrl,
    string AuthenticationType,
    string? AuthenticationValue,
    string? Secret,
    short TimeoutSeconds,
    short MaximumRetryCount,
    DateTimeOffset? LastSuccessfulDeliveryOn,
    bool IsActive);

public sealed record FgsApiWebhookLookupDto(
    long Id,
    string Name,
    string EndpointUrl);

public sealed record FgsApiWebhookCreateDto(
    string Name,
    string? Description,
    string EndpointUrl,
    string AuthenticationType,
    string? AuthenticationValue,
    string? Secret,
    short TimeoutSeconds = 30,
    short MaximumRetryCount = 5);

public sealed record FgsApiWebhookUpdateDto(
    string Name,
    string? Description,
    string EndpointUrl,
    string AuthenticationType,
    string? AuthenticationValue,
    string? Secret,
    short TimeoutSeconds,
    short MaximumRetryCount);

public sealed record FgsApiWebhookPatchDto(
    string? Name,
    string? Description,
    string? EndpointUrl,
    string? AuthenticationType,
    string? AuthenticationValue,
    string? Secret,
    short? TimeoutSeconds,
    short? MaximumRetryCount,
    bool? IsActive);

public sealed record FgsApiWebhookListFilters(
    string? Name = null,
    string? AuthenticationType = null);
