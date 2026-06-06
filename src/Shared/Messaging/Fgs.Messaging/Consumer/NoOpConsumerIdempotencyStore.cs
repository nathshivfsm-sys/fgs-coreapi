namespace Fgs.Messaging.Consumer;

public sealed class NoOpConsumerIdempotencyStore : IConsumerIdempotencyStore
{
    public Task<bool> TryMarkProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(true);
}
