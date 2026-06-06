using System.Text;
using System.Text.Json;
using Fgs.Messaging.Serialization;
using Microsoft.Extensions.Logging;

namespace Fgs.Messaging.Consumer;

public sealed class MessageDispatcher(
    IConsumerMessageRouter router,
    IConsumerIdempotencyStore idempotency,
    ILogger<MessageDispatcher> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = IntegrationEventJsonSerializerOptions.Create();

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

    internal static string GetBodyText(ReadOnlyMemory<byte> body) =>
        Encoding.UTF8.GetString(body.Span);

    internal static object DeserializeBody(Type messageType, ReadOnlyMemory<byte> body)
    {
        var json = GetBodyText(body);
        return JsonSerializer.Deserialize(json, messageType, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize message body to {messageType.Name}.");
    }
}
