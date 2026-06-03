namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// RabbitMQ exchange names for cross-service integration events.
/// </summary>
public static class IntegrationEventExchanges
{
    public const string UserEvents = "fgs.user";

    public const string TenantEvents = "tenant.events";

    public const string PlatformEvents = "fgs.platform";
}
