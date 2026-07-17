using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;

namespace Fgs.User.Infrastructure.Entities.ApiWebhookSubscriptions;

internal sealed class FgsApiWebhookSubscriptionRow
{
    public long Id { get; set; }

    public long FgsApiWebhookId { get; set; }

    public long FgsApiEventId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsApiWebhookSubscriptionSummaryDto ToSummaryDto() =>
        new(Id, FgsApiWebhookId, FgsApiEventId, CreatedOn, CreatedBy);

    public FgsApiWebhookSubscriptionDetailDto ToDetailDto() =>
        new(Id, FgsApiWebhookId, FgsApiEventId, CreatedOn, CreatedBy);
}
