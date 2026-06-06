using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Fgs.Messaging.Consumer;

public sealed class SubscriptionManager(ILogger<SubscriptionManager> logger)
{
    public async Task EnsureSubscriptionTopologyAsync(
        IChannel channel,
        ConsumerSubscriptionOptions subscription,
        CancellationToken cancellationToken = default)
    {
        await RabbitMqQueueTopology.EnsureQueueBindingAsync(
            channel,
            subscription.ExchangeName,
            subscription.QueueName,
            subscription.RoutingKey,
            subscription.DeadLetterExchangeName,
            subscription.DeadLetterQueueName,
            subscription.DeadLetterRoutingKey,
            cancellationToken);

        logger.LogInformation(
            "RabbitMQ consumer topology ready: exchange {Exchange}, queue {Queue}, routing key {RoutingKey}",
            subscription.ExchangeName,
            subscription.QueueName,
            subscription.RoutingKey);
    }
}
