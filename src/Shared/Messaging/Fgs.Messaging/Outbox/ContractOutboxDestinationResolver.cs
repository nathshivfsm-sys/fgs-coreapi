using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Messaging.Outbox;

public sealed class ContractOutboxDestinationResolver(IOptions<RabbitMqOptions> rabbitOptions)
    : IOutboxDestinationResolver
{
    private readonly RabbitMqOptions _rabbitOptions = rabbitOptions.Value;

    public IntegrationEventDestination Resolve(PendingOutboxMessage message)
    {
        var routingKey = !string.IsNullOrWhiteSpace(message.RoutingKey)
            ? message.RoutingKey
            : IntegrationEventRoutingKeys.ForEventType(
                message.EventType,
                _rabbitOptions.RoutingKeyPrefix);

        var destinationName = IntegrationEventExchanges.ForEventType(message.EventType);
        return new IntegrationEventDestination(destinationName, routingKey);
    }
}
