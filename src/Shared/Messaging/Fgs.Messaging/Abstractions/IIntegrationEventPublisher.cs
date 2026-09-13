using Fgs.Messaging.Models;

namespace Fgs.Messaging.Abstractions;

/// <summary>
/// Broker-agnostic publisher used by the outbox processor.
/// Default implementation: RabbitMQ. Swap via DI for SQS without changing claim/retry logic.
/// </summary>
public interface IIntegrationEventPublisher
{
    /// <param name="messageId">
    /// Stable broker MessageId for consumer/notification idempotency.
    /// Outbox should pass <c>SourceKey:Id</c> so republishes of the same row keep the same id.
    /// When null/empty, the transport assigns a new id.
    /// </param>
    Task PublishAsync(
        IntegrationEventDestination destination,
        string payload,
        string? correlationId,
        string? messageId = null,
        CancellationToken cancellationToken = default);
}
