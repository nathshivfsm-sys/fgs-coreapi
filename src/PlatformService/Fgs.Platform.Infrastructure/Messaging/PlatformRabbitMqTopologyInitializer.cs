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
        await channel.ExchangeDeclareAsync(
            _options.ExchangeName,
            ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            _options.DeadLetterExchangeName,
            ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        var dlqArgs = new Dictionary<string, object?>();
        await channel.QueueDeclareAsync(
            _options.DeadLetterQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: dlqArgs,
            passive: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            _options.DeadLetterQueueName,
            _options.DeadLetterExchangeName,
            "platform.notifications.dlq",
            arguments: null,
            cancellationToken: cancellationToken);

        var queueArgs = new Dictionary<string, object?>
        {
            ["x-dead-letter-exchange"] = _options.DeadLetterExchangeName,
            ["x-dead-letter-routing-key"] = "platform.notifications.dlq"
        };

        await channel.QueueDeclareAsync(
            _options.NotificationQueueName,
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
                new RabbitMqQueueBindingOptions { QueueName = _options.NotificationQueueName, RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail },
                new RabbitMqQueueBindingOptions { QueueName = _options.NotificationQueueName, RoutingKey = IntegrationEventRoutingKeys.UserInvited },
                new RabbitMqQueueBindingOptions { QueueName = _options.NotificationQueueName, RoutingKey = IntegrationEventRoutingKeys.PasswordReset },
                new RabbitMqQueueBindingOptions { QueueName = _options.NotificationQueueName, RoutingKey = IntegrationEventRoutingKeys.CompanyCreated }
            ];

        foreach (var binding in bindings)
        {
            if (string.IsNullOrWhiteSpace(binding.RoutingKey))
            {
                continue;
            }

            var queueName = string.IsNullOrWhiteSpace(binding.QueueName)
                ? _options.NotificationQueueName
                : binding.QueueName.Trim();

            if (!string.Equals(queueName, _options.NotificationQueueName, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning(
                    "Skipping bind for queue {Queue}: Platform service only declares {PlatformQueue}. " +
                    "Do not reuse User Service queue names (they have different broker arguments).",
                    queueName,
                    _options.NotificationQueueName);
                continue;
            }

            await channel.QueueBindAsync(
                queueName,
                _options.ExchangeName,
                binding.RoutingKey.Trim(),
                arguments: null,
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Bound queue {Queue} to exchange {Exchange} with routing key {RoutingKey}.",
                queueName,
                _options.ExchangeName,
                binding.RoutingKey);
        }
    }
}
