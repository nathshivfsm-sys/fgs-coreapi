namespace Fgs.User.Domain.Entities;

/// <summary>
/// Assigns API events to webhook endpoints for event delivery.
/// </summary>
public class FgsApiWebhookSubscription : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long FgsApiWebhookId { get; set; }

    public long FgsApiEventId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsApiWebhook? FgsApiWebhook { get; set; }

    public FgsApiEvent? FgsApiEvent { get; set; }
}
