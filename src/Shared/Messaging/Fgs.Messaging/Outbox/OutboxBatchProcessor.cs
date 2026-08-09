using Fgs.Contracts.Observability;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Messaging.Outbox;

public sealed class OutboxBatchProcessor(
    IOutboxStore store,
    IRabbitMqPublisher publisher,
    IOutboxRoutingResolver routingResolver,
    IOptions<OutboxOptions> options,
    ILogger<OutboxBatchProcessor> logger,
    IFgsMetrics? metrics = null)
{
    private readonly OutboxOptions _options = options.Value;
    private readonly IFgsMetrics _metrics = metrics ?? NoOpFgsMetrics.Instance;

    public async Task ProcessBatchAsync(CancellationToken cancellationToken)
    {
        var messages = await store.ClaimPendingBatchAsync(_options.BatchSize, cancellationToken);
        if (messages.Count == 0)
        {
            return;
        }

        _metrics.Gauge("outbox.pending", messages.Count);

        var now = DateTimeOffset.UtcNow;

        foreach (var message in messages)
        {
            try
            {
                var routingKey = routingResolver.ResolveRoutingKey(message);
                var exchangeName = routingResolver.ResolveExchangeName(message);

                logger.LogInformation(
                    "Publishing outbox {OutboxId} event {EventType} to {Exchange}/{RoutingKey} (correlation {CorrelationId})",
                    message.Id,
                    message.EventType,
                    exchangeName,
                    routingKey,
                    message.CorrelationId);

                await publisher.PublishAsync(
                    exchangeName,
                    routingKey,
                    message.Payload,
                    message.CorrelationId.ToString(),
                    cancellationToken);

                await store.MarkPublishedAsync(message.SourceKey, message.Id, now, cancellationToken);
                _metrics.Increment("outbox.published");
            }
            catch (Exception ex)
            {
                var retryCount = message.RetryCount + 1;
                var maxRetries = Math.Min(message.MaxRetryCount, _options.MaxRetryCount);
                var isFailed = retryCount >= maxRetries;

                DateTimeOffset? nextRetryOn = isFailed
                    ? null
                    : now.AddSeconds(Math.Pow(2, retryCount));

                if (isFailed)
                {
                    _metrics.Increment("outbox.failed");
                    logger.LogError(
                        ex,
                        "Outbox message {MessageId} moved to Failed after {RetryCount} attempts",
                        message.Id,
                        retryCount);
                }
                else
                {
                    _metrics.Increment("outbox.retry");
                    logger.LogWarning(
                        ex,
                        "Failed to publish outbox message {MessageId}; retry {RetryCount}/{MaxRetries}",
                        message.Id,
                        retryCount,
                        maxRetries);
                }

                await store.MarkRetryOrFailedAsync(
                    message.SourceKey,
                    message.Id,
                    retryCount,
                    ex.Message,
                    isFailed,
                    nextRetryOn,
                    cancellationToken);
            }
        }
    }
}
