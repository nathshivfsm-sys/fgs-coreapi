namespace Fgs.User.Application.IntegrationEvents;

/// <summary>
/// Routing keys published to RabbitMQ topic exchanges.
/// </summary>
public static class IntegrationEventRoutingKeys
{
    public const string Prefix = "user.";

    public const string TenantProvisionRequested = "tenant.provision.requested";

    public const string CompanySignupInviteEmail = "user.CompanySignupInviteEmail";

    public static string ForEventType(string eventType, string? routingKeyPrefix = null) =>
        eventType switch
        {
            IntegrationEventTypes.TenantProvisionRequested => TenantProvisionRequested,
            IntegrationEventTypes.CompanySignupInviteEmail => CompanySignupInviteEmail,
            _ => $"{routingKeyPrefix ?? Prefix}{eventType}"
        };
}
