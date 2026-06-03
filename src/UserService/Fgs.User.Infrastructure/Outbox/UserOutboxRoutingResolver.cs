using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;
using Fgs.Messaging.Options;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Outbox;

public sealed class UserOutboxRoutingResolver(
    IOptions<RabbitMqOptions> rabbitOptions,
    IOptions<TenantProvisioningOptions> tenantProvisioningOptions) : IOutboxRoutingResolver
{
    private readonly RabbitMqOptions _rabbitOptions = rabbitOptions.Value;
    private readonly TenantProvisioningOptions _tenantProvisioningOptions = tenantProvisioningOptions.Value;

    public string ResolveRoutingKey(PendingOutboxMessage message) =>
        !string.IsNullOrWhiteSpace(message.RoutingKey)
            ? message.RoutingKey
            : IntegrationEventRoutingKeys.ForEventType(
                message.EventType,
                _rabbitOptions.RoutingKeyPrefix);

    public string ResolveExchangeName(PendingOutboxMessage message)
    {
        if (!string.IsNullOrWhiteSpace(message.ExchangeName))
        {
            return message.ExchangeName;
        }

        return message.EventType switch
        {
            IntegrationEventTypes.TenantProvisionRequested => _tenantProvisioningOptions.TenantEventsExchangeName,
            _ => _rabbitOptions.ExchangeName
        };
    }
}
