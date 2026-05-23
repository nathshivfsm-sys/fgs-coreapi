namespace Fgs.User.Application.IntegrationEvents;

/// <summary>
/// RabbitMQ exchange names for integration events published from UserService.
/// </summary>
public static class IntegrationEventExchanges
{
    public const string UserEvents = "fgs.user";

    public const string TenantEvents = "tenant.events";
}
