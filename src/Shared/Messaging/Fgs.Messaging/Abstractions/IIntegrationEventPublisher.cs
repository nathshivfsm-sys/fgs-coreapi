using Fgs.Messaging.Models;

namespace Fgs.Messaging.Abstractions;

/// <summary>
/// Broker-agnostic publisher used by the outbox processor.
/// Default implementation: RabbitMQ. Swap via DI for SQS without changing claim/retry logic.
/// </summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync(
        IntegrationEventDestination destination,
        string payload,
        string? correlationId,
        CancellationToken cancellationToken = default);
}
