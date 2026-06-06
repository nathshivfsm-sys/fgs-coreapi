using System.Text.Json;
using Fgs.Messaging.Consumer;
using Fgs.Messaging.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Consumer.Infrastructure.Messaging;

public sealed class MediatRConsumerMessageRouter(
    IMediator mediator,
    ConsumerRoutingRegistry registry,
    ILogger<MediatRConsumerMessageRouter> logger) : IConsumerMessageRouter
{
    private static readonly JsonSerializerOptions JsonOptions = IntegrationEventJsonSerializerOptions.Create();

    public bool CanRoute(string routingKey) => registry.TryGet(routingKey, out _);

    public async Task RouteAsync(
        string routingKey,
        ReadOnlyMemory<byte> body,
        ConsumerMessageContext context,
        CancellationToken cancellationToken)
    {
        if (!registry.TryGet(routingKey, out var entry))
        {
            throw new InvalidOperationException($"No route registered for routing key '{routingKey}'.");
        }

        var message = JsonSerializer.Deserialize(body.Span, entry.MessageType, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize body to {entry.MessageType.Name}.");

        var command = entry.CreateCommand(message, context);
        logger.LogDebug(
            "Sending MediatR command {CommandType} for routing key {RoutingKey}",
            command.GetType().Name,
            routingKey);

        await mediator.Send(command, cancellationToken);
    }
}
