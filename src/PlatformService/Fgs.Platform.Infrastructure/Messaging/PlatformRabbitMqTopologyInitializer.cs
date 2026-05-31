using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.Platform.Infrastructure.Messaging;

public sealed class PlatformRabbitMqTopologyInitializer(
    IOptions<RabbitMqOptions> options,
    ILogger<PlatformRabbitMqTopologyInitializer> logger)
{
    private readonly RabbitMqOptions _options = options.Value;

    public async Task EnsureTopologyAsync(IChannel channel, CancellationToken cancellationToken)
    {
        var exchangeName = _options.ExchangeName;
        var deadLetterExchangeName = _options.DeadLetterExchangeName
            ?? throw new InvalidOperationException("RabbitMq:DeadLetterExchangeName must be configured.");
        var deadLetterQueueName = _options.DeadLetterQueueName
            ?? throw new InvalidOperationException("RabbitMq:DeadLetterQueueName must be configured.");
        var notificationQueueName = _options.NotificationQueueName
            ?? throw new InvalidOperationException("RabbitMq:NotificationQueueName must be configured.");

        await channel.ExchangeDeclareAsync(
            exchangeName,
            ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            deadLetterExchangeName,
            ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        var dlqArgs = new Dictionary<string, object?>();
        await channel.QueueDeclareAsync(
            deadLetterQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: dlqArgs,
            passive: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            deadLetterQueueName,
            deadLetterExchangeName,
            "platform.notifications.dlq",
            arguments: null,
            cancellationToken: cancellationToken);

        var queueArgs = new Dictionary<string, object?>
        {
            ["x-dead-letter-exchange"] = deadLetterExchangeName,
            ["x-dead-letter-routing-key"] = "platform.notifications.dlq"
        };

        await channel.QueueDeclareAsync(
            notificationQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs,
            passive: false,
            cancellationToken: cancellationToken);

        var bindings = _options.QueueBindings.Count > 0
            ? _options.QueueBindings
            :
            [
                new RabbitMqQueueBindingOptions { QueueName = notificationQueueName, RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail },
                new RabbitMqQueueBindingOptions { QueueName = notificationQueueName, RoutingKey = IntegrationEventRoutingKeys.UserInvited },
                new RabbitMqQueueBindingOptions { QueueName = notificationQueueName, RoutingKey = IntegrationEventRoutingKeys.PasswordReset },
                new RabbitMqQueueBindingOptions { QueueName = notificationQueueName, RoutingKey = IntegrationEventRoutingKeys.CompanyCreated }
            ];

        foreach (var binding in bindings)
        {
            if (string.IsNullOrWhiteSpace(binding.RoutingKey))
            {
                continue;
            }

            var queueName = string.IsNullOrWhiteSpace(binding.QueueName)
                ? notificationQueueName
                : binding.QueueName.Trim();

            if (!string.Equals(queueName, notificationQueueName, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning(
                    "Skipping bind for queue {Queue}: Platform service only declares {PlatformQueue}. " +
                    "Do not reuse User Service queue names (they have different broker arguments).",
                    queueName,
                    notificationQueueName);
                continue;
            }

            await channel.QueueBindAsync(
                queueName,
                exchangeName,
                binding.RoutingKey.Trim(),
                arguments: null,
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Bound queue {Queue} to exchange {Exchange} with routing key {RoutingKey}.",
                queueName,
                exchangeName,
                binding.RoutingKey);
        }
    }
}
