namespace Fgs.User.Application.Abstractions.Messaging;

public interface IOutboxWriter
{
    Task EnqueueAsync(
        string eventType,
        string payload,
        string idempotencyKey,
        string? correlationId,
        CancellationToken cancellationToken = default);
}
