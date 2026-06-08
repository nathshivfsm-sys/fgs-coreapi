namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// RabbitMQ exchange names for cross-service integration events.
/// </summary>
public static class IntegrationEventExchanges
{
    public const string UserEvents = "fgs.user";

    public const string TenantEvents = "tenant.events";

    public const string SetupEvents = "setup.events";

    public static IReadOnlyList<string> All { get; } =
    [
        UserEvents,
        TenantEvents,
        SetupEvents
    ];

    /// <summary>
    /// Resolves the topic exchange for an integration event type.
    /// </summary>
    public static string ForEventType(string eventType) =>
        eventType switch
        {
            IntegrationEventTypes.TenantProvisionRequested => TenantEvents,
            IntegrationEventTypes.TenantProvisionCompleted => TenantEvents,
            IntegrationEventTypes.CompanySignupInviteEmail => UserEvents,
            IntegrationEventTypes.CredentialConfigurationChanged => SetupEvents,

            _ => UserEvents
        };
}
