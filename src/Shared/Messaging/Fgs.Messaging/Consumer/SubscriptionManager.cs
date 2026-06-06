using Fgs.Messaging.Options;
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
        await channel.ExchangeDeclareAsync(
            subscription.ExchangeName,
            ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(subscription.DeadLetterExchangeName))
        {
            await channel.ExchangeDeclareAsync(
                subscription.DeadLetterExchangeName,
                ExchangeType.Topic,
                durable: true,
                cancellationToken: cancellationToken);
        }

        IDictionary<string, object?>? queueArgs = null;
        if (!string.IsNullOrWhiteSpace(subscription.DeadLetterExchangeName)
            && !string.IsNullOrWhiteSpace(subscription.DeadLetterRoutingKey))
        {
            queueArgs = new Dictionary<string, object?>
            {
                ["x-dead-letter-exchange"] = subscription.DeadLetterExchangeName,
                ["x-dead-letter-routing-key"] = subscription.DeadLetterRoutingKey
            };
        }

        await channel.QueueDeclareAsync(
            subscription.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            subscription.QueueName,
            subscription.ExchangeName,
            subscription.RoutingKey,
            cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(subscription.DeadLetterQueueName)
            && !string.IsNullOrWhiteSpace(subscription.DeadLetterExchangeName)
            && !string.IsNullOrWhiteSpace(subscription.DeadLetterRoutingKey))
        {
            await channel.QueueDeclareAsync(
                subscription.DeadLetterQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(
                subscription.DeadLetterQueueName,
                subscription.DeadLetterExchangeName,
                subscription.DeadLetterRoutingKey,
                cancellationToken: cancellationToken);
        }

        logger.LogInformation(
            "RabbitMQ consumer topology ready: exchange {Exchange}, queue {Queue}, routing key {RoutingKey}",
            subscription.ExchangeName,
            subscription.QueueName,
            subscription.RoutingKey);
    }
}
