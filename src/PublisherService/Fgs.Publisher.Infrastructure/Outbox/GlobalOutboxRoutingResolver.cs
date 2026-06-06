using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Publisher.Infrastructure.Outbox;

public sealed class GlobalOutboxRoutingResolver(IOptions<RabbitMqOptions> rabbitOptions) : IOutboxRoutingResolver
{
    private readonly RabbitMqOptions _rabbitOptions = rabbitOptions.Value;

    public string ResolveRoutingKey(PendingOutboxMessage message) =>
        !string.IsNullOrWhiteSpace(message.RoutingKey)
            ? message.RoutingKey
            : IntegrationEventRoutingKeys.ForEventType(
                message.EventType,
                _rabbitOptions.RoutingKeyPrefix);

    public string ResolveExchangeName(PendingOutboxMessage message) =>
        IntegrationEventExchanges.ForEventType(message.EventType);
}
