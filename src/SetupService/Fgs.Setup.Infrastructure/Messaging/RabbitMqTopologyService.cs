using Fgs.Setup.Infrastructure.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.Setup.Infrastructure.Messaging;

public sealed class RabbitMqTopologyService(
    IOptions<RabbitMqConsumerOptions> tenantProvisionConsumerOptions,
    IOptions<TenantProvisioningOptions> tenantProvisioningOptions,
    ILogger<RabbitMqTopologyService> logger)
{
    public async Task EnsureTenantProvisioningTopologyAsync(
        IChannel channel,
        CancellationToken cancellationToken = default)
    {
        var consumer = tenantProvisionConsumerOptions.Value;
        var tenantExchange = tenantProvisioningOptions.Value.TenantEventsExchangeName;

        await channel.ExchangeDeclareAsync(
            exchange: tenantExchange,
            type: ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: consumer.DeadLetterExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        var mainQueueArgs = new Dictionary<string, object?>
        {
            ["x-dead-letter-exchange"] = consumer.DeadLetterExchangeName,
            ["x-dead-letter-routing-key"] = consumer.DeadLetterRoutingKey
        };

        await channel.QueueDeclareAsync(
            queue: consumer.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: mainQueueArgs,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: consumer.QueueName,
            exchange: tenantExchange,
            routingKey: consumer.RoutingKey,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: consumer.DeadLetterQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: consumer.DeadLetterQueueName,
            exchange: consumer.DeadLetterExchangeName,
            routingKey: consumer.DeadLetterRoutingKey,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "RabbitMQ topology ready: exchange {Exchange}, queue {Queue}, DLQ {Dlq}",
            tenantExchange,
            consumer.QueueName,
            consumer.DeadLetterQueueName);
    }
}
