using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Fgs.Messaging.Consumer;

/// <summary>
/// Durable consumer idempotency backed by <see cref="IDistributedCache"/> (Redis in production).
/// </summary>
public sealed class DistributedCacheConsumerIdempotencyStore(
    IDistributedCache cache,
    ILogger<DistributedCacheConsumerIdempotencyStore> logger) : IConsumerIdempotencyStore
{
    private static readonly DistributedCacheEntryOptions DefaultExpiration = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    public async Task<bool> HasBeenProcessedAsync(
        string messageId,
        string routingKey,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            return false;
        }

        var existing = await cache.GetAsync(BuildKey(messageId, routingKey), cancellationToken);
        return existing is { Length: > 0 };
    }

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
        return true;
    }

    internal static string BuildKey(string messageId, string routingKey) =>
        $"fgs:consumer:idempotency:{routingKey}:{messageId}";
}
