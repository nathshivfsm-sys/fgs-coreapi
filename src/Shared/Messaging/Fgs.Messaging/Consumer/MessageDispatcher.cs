using System.Diagnostics;
using Fgs.Contracts.Observability;
using Microsoft.Extensions.Logging;

namespace Fgs.Messaging.Consumer;

public sealed class MessageDispatcher(
    IConsumerMessageRouter router,
    IConsumerIdempotencyStore idempotency,
    ILogger<MessageDispatcher> logger,
    IFgsMetrics? metrics = null)
{
    private readonly IFgsMetrics _metrics = metrics ?? NoOpFgsMetrics.Instance;

    public async Task DispatchAsync(
        string routingKey,
        ReadOnlyMemory<byte> body,
        ConsumerMessageContext context,
        CancellationToken cancellationToken)
    {
        if (!router.CanRoute(routingKey))
        {
            logger.LogWarning(
                "No consumer route registered for routing key {RoutingKey} (MessageId={MessageId}, CorrelationId={CorrelationId})",
                routingKey,
                context.MessageId,
                context.CorrelationId);
            return;
        }

        if (await idempotency.HasBeenProcessedAsync(context.MessageId, routingKey, cancellationToken))
        {
            _metrics.Increment("rabbitmq.consumer_duplicate");
            logger.LogInformation(
                "Skipping duplicate message {MessageId} for routing key {RoutingKey}",
                context.MessageId,
                routingKey);
            return;
        }

        logger.LogInformation(
            "Dispatching message {MessageId} with routing key {RoutingKey} (CorrelationId={CorrelationId}, RetryCount={RetryCount})",
            context.MessageId,
            routingKey,
            context.CorrelationId,
            context.RetryCount);

        var sw = Stopwatch.StartNew();
        try
        {
            await router.RouteAsync(routingKey, body, context, cancellationToken);
            await idempotency.TryMarkProcessedAsync(context.MessageId, routingKey, cancellationToken);
            _metrics.Increment("rabbitmq.consume");
            _metrics.Histogram("rabbitmq.consume_latency_ms", sw.Elapsed.TotalMilliseconds);
        }
        catch
        {
            _metrics.Increment("rabbitmq.consume_failure");
            throw;
        }
    }
}
