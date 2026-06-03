using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using System.Text;
using System.Text.Json;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Infrastructure.Credentials;
using Fgs.Notification.Infrastructure.Messaging;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fgs.Notification.Infrastructure.Notifications.Workers;

public sealed class NotificationQueueWorker(
    RabbitMqConnectionFactory connectionFactory,
    NotificationRabbitMqTopologyInitializer topologyInitializer,
    IServiceScopeFactory scopeFactory,
    IRabbitMqEffectiveOptionsProvider rabbitMqOptions,
    IOptions<NotificationWorkerOptions> workerOptions,
    RabbitMqConsumerStartupGate rabbitMqConsumerStartupGate,
    ILogger<NotificationQueueWorker> logger) : BackgroundService
{
    private readonly NotificationWorkerOptions _worker = workerOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await rabbitMqConsumerStartupGate.WaitAsync(stoppingToken);
        logger.LogInformation("Notification queue worker starting.");

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
                logger.LogError(ex, "Notification consumer faulted; retrying in {Delay}s.", _worker.RetryDelaySeconds);
                await Task.Delay(TimeSpan.FromSeconds(_worker.RetryDelaySeconds), stoppingToken);
            }
        }
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        var rabbit = rabbitMqOptions.GetEffectiveOptions();
        logger.LogInformation(
            "Notification consumer connecting to RabbitMQ {Host}:{Port} as {User}.",
            rabbit.HostName,
            rabbit.Port,
            rabbit.UserName);
        var connection = await connectionFactory.GetConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        if (rabbit.EnsureQueuesOnStartup)
        {
            await topologyInitializer.EnsureTopologyAsync(channel, stoppingToken);
        }

        await channel.BasicQosAsync(0, _worker.PrefetchCount, false, stoppingToken);

        var notificationQueueName = rabbit.NotificationQueueName
            ?? throw new InvalidOperationException("RabbitMq:NotificationQueueName must be configured.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            var routingKey = args.RoutingKey;
            var messageId = args.BasicProperties.MessageId ?? Guid.NewGuid().ToString("N");
            var correlationId = args.BasicProperties.CorrelationId;
            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            using var scope = scopeFactory.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IIntegrationEventMapper>();
            var idempotency = scope.ServiceProvider.GetRequiredService<IIdempotencyStore>();
            var dispatcher = scope.ServiceProvider.GetRequiredService<INotificationDispatcher>();

            try
            {
                if (!mapper.CanMap(routingKey))
                {
                    logger.LogWarning(
                        "No mapper for routing key {RoutingKey} (MessageId={MessageId}, CorrelationId={CorrelationId}).",
                        routingKey,
                        messageId,
                        correlationId);
                    await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
                    return;
                }

                if (!await idempotency.TryMarkProcessedAsync(messageId, routingKey, stoppingToken))
                {
                    logger.LogInformation(
                        "Skipping duplicate message {MessageId} (CorrelationId={CorrelationId}).",
                        messageId,
                        correlationId);
                    await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
                    return;
                }

                var request = mapper.Map(routingKey, body, correlationId, messageId);
                if (request is null)
                {
                    await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
                    return;
                }

                logger.LogInformation(
                    "Dispatching notification (TenantId={TenantId}, Channel={Channel}, Template={Template}, CorrelationId={CorrelationId}).",
                    request.TenantId,
                    request.Channel,
                    request.TemplateCode,
                    request.CorrelationId);

                var result = await dispatcher.DispatchAsync(request, stoppingToken);
                if (!result.Success)
                {
                    throw new InvalidOperationException(result.Error ?? "Notification dispatch failed.");
                }

                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                var retryCount = GetRetryCount(args.BasicProperties.Headers);
                if (retryCount >= _worker.MaxRetryAttempts)
                {
                    logger.LogError(
                        ex,
                        "Message {MessageId} exceeded max retries; dead-lettering (CorrelationId={CorrelationId}).",
                        messageId,
                        correlationId);
                    await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false, stoppingToken);
                    return;
                }

                logger.LogWarning(
                    ex,
                    "Notification processing failed; requeueing (Attempt={Attempt}, MessageId={MessageId}, CorrelationId={CorrelationId}).",
                    retryCount + 1,
                    messageId,
                    correlationId);

                await PublishRetryAsync(channel, routingKey, body, args.BasicProperties, retryCount + 1, stoppingToken);
                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(
            notificationQueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private static int GetRetryCount(IDictionary<string, object?>? headers)
    {
        if (headers is null || !headers.TryGetValue("x-retry-count", out var value))
        {
            return 0;
        }

        return value switch
        {
            int i => i,
            byte b => b,
            long l => (int)l,
            _ => 0
        };
    }

    private async Task PublishRetryAsync(
        IChannel channel,
        string routingKey,
        string body,
        IReadOnlyBasicProperties originalProperties,
        int retryCount,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(_worker.RetryDelaySeconds), cancellationToken);

        var properties = new BasicProperties
        {
            ContentType = originalProperties.ContentType ?? "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = originalProperties.MessageId ?? Guid.NewGuid().ToString("N"),
            CorrelationId = originalProperties.CorrelationId,
            Headers = new Dictionary<string, object?> { ["x-retry-count"] = retryCount }
        };

        await channel.BasicPublishAsync(
            rabbitMqOptions.GetEffectiveOptions().ExchangeName,
            routingKey,
            mandatory: false,
            basicProperties: properties,
            body: Encoding.UTF8.GetBytes(body),
            cancellationToken: cancellationToken);
    }
}
