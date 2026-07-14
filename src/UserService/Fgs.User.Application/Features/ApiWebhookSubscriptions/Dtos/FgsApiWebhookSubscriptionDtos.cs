namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;

public sealed record FgsApiWebhookSubscriptionSummaryDto(
    long Id,
    long FgsApiWebhookId,
    long FgsApiEventId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsApiWebhookSubscriptionDetailDto(
    long Id,
    long FgsApiWebhookId,
    long FgsApiEventId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsApiWebhookSubscriptionCreateDto(long FgsApiWebhookId, long FgsApiEventId);

public sealed record FgsApiWebhookSubscriptionListFilters(
    long? FgsApiWebhookId = null,
    long? FgsApiEventId = null);
