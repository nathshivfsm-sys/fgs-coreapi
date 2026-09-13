using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;

namespace Fgs.Messaging.RabbitMq;

public sealed class RabbitMqIntegrationEventPublisher(IRabbitMqPublisher rabbitMqPublisher)
    : IIntegrationEventPublisher
{
    public Task PublishAsync(
        IntegrationEventDestination destination,
        string payload,
        string? correlationId,
        string? messageId = null,
        CancellationToken cancellationToken = default) =>
        rabbitMqPublisher.PublishAsync(
            destination.DestinationName,
            destination.RoutingKey,
            payload,
            correlationId,
            messageId,
            cancellationToken);
}
