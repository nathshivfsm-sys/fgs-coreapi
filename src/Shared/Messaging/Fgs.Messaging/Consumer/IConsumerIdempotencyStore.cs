namespace Fgs.Messaging.Consumer;

public interface IConsumerIdempotencyStore
{
    /// <summary>
    /// Returns <c>true</c> when this message was already successfully processed.
    /// </summary>
    Task<bool> HasBeenProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the message as successfully processed. Returns <c>false</c> when it was already marked.
    /// </summary>
    Task<bool> TryMarkProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default);
}
