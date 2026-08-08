using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Fgs.Messaging.Consumer;

/// <summary>
/// Durable consumer idempotency backed by <see cref="IDistributedCache"/> (Redis in production).
/// Returns <c>false</c> when the message was already marked processed.
/// </summary>
public sealed class DistributedCacheConsumerIdempotencyStore(
    IDistributedCache cache,
    ILogger<DistributedCacheConsumerIdempotencyStore> logger) : IConsumerIdempotencyStore
{
    private static readonly DistributedCacheEntryOptions DefaultExpiration = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

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

        var key = BuildKey(messageId, routingKey);
        var existing = await cache.GetAsync(key, cancellationToken);
        if (existing is { Length: > 0 })
        {
            return false;
        }

        var payload = System.Text.Encoding.UTF8.GetBytes(
            DateTimeOffset.UtcNow.ToString("O"));
        await cache.SetAsync(key, payload, DefaultExpiration, cancellationToken);

        // Re-read to reduce (not eliminate) race windows across instances.
        var confirm = await cache.GetAsync(key, cancellationToken);
        return confirm is { Length: > 0 };
    }

    internal static string BuildKey(string messageId, string routingKey) =>
        $"fgs:consumer:idempotency:{routingKey}:{messageId}";
}
