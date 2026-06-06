using RabbitMQ.Client;

namespace Fgs.Messaging.RabbitMq;

public static class RabbitMqQueueTopology
{
    public static Dictionary<string, object?>? BuildQueueArguments(
        string? deadLetterExchangeName,
        string? deadLetterRoutingKey)
    {
        if (string.IsNullOrWhiteSpace(deadLetterExchangeName)
            || string.IsNullOrWhiteSpace(deadLetterRoutingKey))
        {
            return null;
        }

        return new Dictionary<string, object?>
        {
            ["x-dead-letter-exchange"] = deadLetterExchangeName,
            ["x-dead-letter-routing-key"] = deadLetterRoutingKey
        };
    }

    public static async Task EnsureQueueBindingAsync(
        IChannel channel,
        string exchangeName,
        string queueName,
        string routingKey,
        string? deadLetterExchangeName = null,
        string? deadLetterQueueName = null,
        string? deadLetterRoutingKey = null,
        CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(
            exchangeName,
            ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(deadLetterExchangeName))
        {
            await channel.ExchangeDeclareAsync(
                deadLetterExchangeName,
                ExchangeType.Topic,
                durable: true,
                cancellationToken: cancellationToken);
        }

        await channel.QueueDeclareAsync(
            queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: BuildQueueArguments(deadLetterExchangeName, deadLetterRoutingKey),
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queueName,
            exchangeName,
            routingKey,
            cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(deadLetterQueueName)
            && !string.IsNullOrWhiteSpace(deadLetterExchangeName)
            && !string.IsNullOrWhiteSpace(deadLetterRoutingKey))
        {
            await channel.QueueDeclareAsync(
                deadLetterQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(
                deadLetterQueueName,
                deadLetterExchangeName,
                deadLetterRoutingKey,
                cancellationToken: cancellationToken);
        }
    }
}
