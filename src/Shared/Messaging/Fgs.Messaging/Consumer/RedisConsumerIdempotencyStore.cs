using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Fgs.Messaging.Consumer;

/// <summary>
/// Atomic consumer idempotency using Redis <c>SET NX</c>.
/// Returns <c>false</c> when the message was already marked processed.
/// </summary>
public sealed class RedisConsumerIdempotencyStore(
    IConnectionMultiplexer connectionMultiplexer,
    ILogger<RedisConsumerIdempotencyStore> logger) : IConsumerIdempotencyStore
{
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromDays(7);

    public async Task<bool> TryMarkProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            logger.LogWarning(
                "Consumer idempotency skipped because MessageId is empty (RoutingKey={RoutingKey})",
                routingKey);
            return true;
        }

        var key = DistributedCacheConsumerIdempotencyStore.BuildKey(messageId, routingKey);
        var database = connectionMultiplexer.GetDatabase();
        var created = await database.StringSetAsync(
            key,
            DateTimeOffset.UtcNow.ToString("O"),
            DefaultTtl,
            When.NotExists);

        return created;
    }
}
