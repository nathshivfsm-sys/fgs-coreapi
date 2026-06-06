namespace Fgs.Messaging.Consumer;

public interface IConsumerIdempotencyStore
{
    Task<bool> TryMarkProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default);
}
