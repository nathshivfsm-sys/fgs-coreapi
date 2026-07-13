namespace Fgs.User.Domain.Entities;

/// <summary>
/// Webhook endpoint registered by tenant administrators for receiving API event notifications.
/// </summary>
public class FgsApiWebhook : FgsTenantCompanySetupEntityBase<long>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string EndpointUrl { get; set; } = null!;

    public string AuthenticationType { get; set; } = null!;

    public string? AuthenticationValue { get; set; }

    public string? Secret { get; set; }

    public short TimeoutSeconds { get; set; } = 30;

    public short MaximumRetryCount { get; set; } = 5;

    public DateTimeOffset? LastSuccessfulDeliveryOn { get; set; }

    public ICollection<FgsApiWebhookSubscription> Subscriptions { get; set; } = [];
}
