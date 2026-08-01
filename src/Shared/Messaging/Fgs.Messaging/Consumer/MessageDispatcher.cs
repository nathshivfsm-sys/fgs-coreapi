using Microsoft.Extensions.Logging;

namespace Fgs.Messaging.Consumer;

public sealed class MessageDispatcher(
    IConsumerMessageRouter router,
    IConsumerIdempotencyStore idempotency,
    ILogger<MessageDispatcher> logger)
{
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

        if (!await idempotency.TryMarkProcessedAsync(context.MessageId, routingKey, cancellationToken))
        {
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

        await router.RouteAsync(routingKey, body, context, cancellationToken);
    }
}
