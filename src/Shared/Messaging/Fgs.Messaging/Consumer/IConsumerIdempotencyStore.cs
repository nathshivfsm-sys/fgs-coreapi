namespace Fgs.Messaging.Consumer;

public interface IConsumerIdempotencyStore
{
    /// <summary>
    /// Returns <c>true</c> when this message was already successfully processed (or is held by a lease).
    /// </summary>
    Task<bool> HasBeenProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Atomically acquires processing ownership (e.g. Redis SET NX). Returns <c>false</c> when
    /// another consumer already holds or completed the message — do not dispatch.
    /// </summary>
    Task<bool> TryMarkProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases a previously acquired mark so a failed dispatch can be retried.
    /// </summary>
    Task TryReleaseAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default);
}
