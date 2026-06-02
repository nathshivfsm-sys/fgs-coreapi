using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Fgs.User.Application.Abstractions.Credentials;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fgs.Platform.Infrastructure.Credentials;

/// <summary>
/// Reloads decrypted credentials when the User Service publishes <see cref="CredentialConfigurationChangedEvent"/>.
/// </summary>
public sealed class CredentialConfigurationReloadConsumerService(
    RabbitMqConnectionFactory connectionFactory,
    IServiceScopeFactory scopeFactory,
    IRabbitMqEffectiveOptionsProvider rabbitMqOptions,
    RabbitMqConsumerStartupGate rabbitMqConsumerStartupGate,
    ILogger<CredentialConfigurationReloadConsumerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await rabbitMqConsumerStartupGate.WaitAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Credential configuration reload consumer faulted; retrying in 10s.");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        var rabbit = rabbitMqOptions.GetEffectiveOptions();
        var connection = await connectionFactory.GetConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            rabbit.ExchangeName,
            ExchangeType.Topic,
            durable: true,
            cancellationToken: stoppingToken);

        var queueName = $"{rabbit.NotificationQueueName ?? "fgs.platform.notifications"}.credential-reload";
        await channel.QueueDeclareAsync(
            queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queueName,
            rabbit.ExchangeName,
            IntegrationEventRoutingKeys.CredentialConfigurationChanged,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var configurationProvider = scope.ServiceProvider.GetRequiredService<ICredentialConfigurationProvider>();
                await configurationProvider.ReloadAsync(stoppingToken);

                logger.LogInformation(
                    "Reloaded credential configuration after {RoutingKey} (delivery tag {Tag}).",
                    args.RoutingKey,
                    args.DeliveryTag);

                await channel.BasicAckAsync(args.DeliveryTag, multiple: false, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to reload credential configuration for routing key {RoutingKey}.",
                    args.RoutingKey);
                await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer, stoppingToken);
        logger.LogInformation(
            "Credential configuration reload consumer listening on {Queue} for {RoutingKey}.",
            queueName,
            IntegrationEventRoutingKeys.CredentialConfigurationChanged);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
