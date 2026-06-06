using System.Text;
using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fgs.Messaging.Consumer;

public sealed class ConsumerHost(
    IServiceScopeFactory scopeFactory,
    RabbitMqConnectionFactory connectionFactory,
    SubscriptionManager subscriptionManager,
    ConsumerRetryPolicy retryPolicy,
    IOptions<ConsumerOptions> consumerOptions,
    IOptions<RabbitMqOptions> rabbitMqOptions,
    ILogger<ConsumerHost> logger) : BackgroundService
{
    private readonly ConsumerOptions _consumerOptions = consumerOptions.Value;
    private readonly RabbitMqOptions _rabbitMqOptions = rabbitMqOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_consumerOptions.Enabled)
        {
            logger.LogInformation("RabbitMQ consumer host is disabled.");
            return;
        }

        if (_consumerOptions.Subscriptions.Count == 0)
        {
            logger.LogWarning("RabbitMQ consumer host enabled but no subscriptions configured.");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunConsumersAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "RabbitMQ consumer host faulted; retrying in {Delay}s",
                    _consumerOptions.InitialRetryDelaySeconds);
                await Task.Delay(
                    TimeSpan.FromSeconds(_consumerOptions.InitialRetryDelaySeconds),
                    stoppingToken);
            }
        }
    }

    private async Task RunConsumersAsync(CancellationToken stoppingToken)
    {
        var connection = await connectionFactory.GetConnectionAsync(stoppingToken);
        var channels = new List<IChannel>();

        try
        {
            foreach (var subscription in _consumerOptions.Subscriptions)
            {
                var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
                channels.Add(channel);

                await subscriptionManager.EnsureSubscriptionTopologyAsync(channel, subscription, stoppingToken);
                await channel.BasicQosAsync(0, _consumerOptions.PrefetchCount, false, stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += (_, args) =>
                    HandleMessageAsync(channel, subscription, args, stoppingToken);

                await channel.BasicConsumeAsync(
                    subscription.QueueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken);

                logger.LogInformation(
                    "Consumer listening on queue {Queue} for routing key {RoutingKey}",
                    subscription.QueueName,
                    subscription.RoutingKey);
            }

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // shutdown
        }
        finally
        {
            foreach (var channel in channels)
            {
                await channel.CloseAsync(stoppingToken);
                await channel.DisposeAsync();
            }
        }
    }

    private async Task HandleMessageAsync(
        IChannel channel,
        ConsumerSubscriptionOptions subscription,
        BasicDeliverEventArgs args,
        CancellationToken stoppingToken)
    {
        var routingKey = string.IsNullOrWhiteSpace(args.RoutingKey)
            ? subscription.RoutingKey
            : args.RoutingKey;
        var messageId = args.BasicProperties.MessageId ?? Guid.NewGuid().ToString("N");
        var correlationId = args.BasicProperties.CorrelationId ?? Guid.NewGuid().ToString();
        var retryCount = ConsumerRetryPolicy.GetRetryCount(args.BasicProperties.Headers);
        var body = args.Body;
        var rawBody = Encoding.UTF8.GetString(body.ToArray());

        var context = new ConsumerMessageContext
        {
            RoutingKey = routingKey,
            MessageId = messageId,
            CorrelationId = correlationId,
            RetryCount = retryCount,
            RawBody = rawBody,
            Headers = args.BasicProperties.Headers is { } headers
                ? new Dictionary<string, object?>(headers)
                : new Dictionary<string, object?>()
        };

        try
        {
            using var scope = scopeFactory.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<MessageDispatcher>();
            await dispatcher.DispatchAsync(routingKey, body, context, stoppingToken);
            await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
        }
        catch (ConsumerRetryExhaustedException ex)
        {
            logger.LogError(
                ex,
                "Dead-lettering message {MessageId} (CorrelationId={CorrelationId}, RoutingKey={RoutingKey})",
                messageId,
                correlationId,
                routingKey);
            await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false, stoppingToken);
        }
        catch (Exception ex)
        {
            try
            {
                await retryPolicy.HandleFailureAsync(
                    channel,
                    subscription.ExchangeName,
                    routingKey,
                    body,
                    args.BasicProperties,
                    retryCount,
                    messageId,
                    correlationId,
                    ex,
                    stoppingToken);
                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (ConsumerRetryExhaustedException)
            {
                await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false, stoppingToken);
            }
        }
    }
}
