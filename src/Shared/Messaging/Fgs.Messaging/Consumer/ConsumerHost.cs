using System.Text;
using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Fgs.Messaging.Consumer;

public sealed class ConsumerHost(
    IServiceScopeFactory scopeFactory,
    RabbitMqConnectionFactory connectionFactory,
    SubscriptionManager subscriptionManager,
    ConsumerRetryPolicy retryPolicy,
    IOptions<ConsumerOptions> consumerOptions,
    ILogger<ConsumerHost> logger) : BackgroundService
{
    private readonly ConsumerOptions _consumerOptions = consumerOptions.Value;

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
        using var connectionLostCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

        AsyncEventHandler<ShutdownEventArgs>? shutdownHandler = (_, args) =>
        {
            if (args.Initiator != ShutdownInitiator.Application)
            {
                logger.LogWarning(
                    "RabbitMQ connection shut down ({ReplyCode} {ReplyText}); restarting consumers.",
                    args.ReplyCode,
                    args.ReplyText);
                connectionLostCts.Cancel();
            }

            return Task.CompletedTask;
        };

        connection.ConnectionShutdownAsync += shutdownHandler;

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

            await Task.Delay(Timeout.Infinite, connectionLostCts.Token);
        }
        catch (OperationCanceledException) when (connectionLostCts.IsCancellationRequested && !stoppingToken.IsCancellationRequested)
        {
            // Connection lost; outer loop will recreate consumers.
        }
        finally
        {
            connection.ConnectionShutdownAsync -= shutdownHandler;

            foreach (var channel in channels)
            {
                try
                {
                    if (channel.IsOpen)
                    {
                        await channel.CloseAsync(stoppingToken);
                    }
                }
                catch (AlreadyClosedException)
                {
                    // Ignore shutdown races.
                }

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
            await SafeAckAsync(channel, args.DeliveryTag, messageId, stoppingToken);
        }
        catch (ConsumerRetryExhaustedException ex)
        {
            logger.LogError(
                ex,
                "Dead-lettering message {MessageId} (CorrelationId={CorrelationId}, RoutingKey={RoutingKey})",
                messageId,
                correlationId,
                routingKey);
            await SafeNackAsync(channel, args.DeliveryTag, messageId, stoppingToken);
        }
        catch (ConsumerChannelClosedException ex)
        {
            logger.LogWarning(
                ex,
                "Broker channel closed while processing message {MessageId}; RabbitMQ will redeliver it.",
                messageId);
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
                await SafeAckAsync(channel, args.DeliveryTag, messageId, stoppingToken);
            }
            catch (ConsumerRetryExhaustedException retryExhausted)
            {
                logger.LogError(
                    retryExhausted,
                    "Dead-lettering message {MessageId} (CorrelationId={CorrelationId}, RoutingKey={RoutingKey})",
                    messageId,
                    correlationId,
                    routingKey);
                await SafeNackAsync(channel, args.DeliveryTag, messageId, stoppingToken);
            }
            catch (ConsumerChannelClosedException channelClosed)
            {
                logger.LogWarning(
                    channelClosed,
                    "Broker channel closed while retrying message {MessageId}; RabbitMQ will redeliver it.",
                    messageId);
            }
        }
    }

    private async Task SafeAckAsync(
        IChannel channel,
        ulong deliveryTag,
        string messageId,
        CancellationToken cancellationToken)
    {
        if (!channel.IsOpen)
        {
            logger.LogWarning(
                "Skipping ack for message {MessageId} because the channel is already closed; RabbitMQ will redeliver it.",
                messageId);
            return;
        }

        try
        {
            await channel.BasicAckAsync(deliveryTag, false, cancellationToken);
        }
        catch (AlreadyClosedException ex)
        {
            logger.LogWarning(
                ex,
                "Channel closed before ack for message {MessageId}; RabbitMQ will redeliver it.",
                messageId);
        }
        catch (OperationInterruptedException ex)
        {
            logger.LogWarning(
                ex,
                "Ack interrupted for message {MessageId}; RabbitMQ will redeliver it.",
                messageId);
        }
    }

    private async Task SafeNackAsync(
        IChannel channel,
        ulong deliveryTag,
        string messageId,
        CancellationToken cancellationToken)
    {
        if (!channel.IsOpen)
        {
            logger.LogWarning(
                "Skipping nack for message {MessageId} because the channel is already closed; RabbitMQ will redeliver or dead-letter it.",
                messageId);
            return;
        }

        try
        {
            await channel.BasicNackAsync(deliveryTag, false, requeue: false, cancellationToken);
        }
        catch (AlreadyClosedException ex)
        {
            logger.LogWarning(
                ex,
                "Channel closed before nack for message {MessageId}; RabbitMQ will redeliver or dead-letter it.",
                messageId);
        }
        catch (OperationInterruptedException ex)
        {
            logger.LogWarning(
                ex,
                "Nack interrupted for message {MessageId}; RabbitMQ will redeliver or dead-letter it.",
                messageId);
        }
    }
}
