namespace Fgs.Messaging.Models;

public sealed record PendingOutboxMessage(
    string SourceKey,
    long Id,
    string EventType,
    string Payload,
    Guid CorrelationId,
    string? ExchangeName,
    string? RoutingKey,
    int RetryCount,
    int MaxRetryCount);
