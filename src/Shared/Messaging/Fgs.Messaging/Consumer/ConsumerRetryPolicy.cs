using System.Text;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Fgs.Messaging.Consumer;

public sealed class ConsumerRetryPolicy(
    IOptions<ConsumerOptions> consumerOptions,
    ILogger<ConsumerRetryPolicy> logger)
{
    private readonly ConsumerOptions _options = consumerOptions.Value;

    public int MaxRetryAttempts => _options.MaxRetryAttempts;

    public async Task HandleFailureAsync(
        IChannel channel,
        string exchangeName,
        string routingKey,
        ReadOnlyMemory<byte> body,
        IReadOnlyBasicProperties originalProperties,
        int retryCount,
        string messageId,
        string? correlationId,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (retryCount >= _options.MaxRetryAttempts)
        {
            logger.LogError(
                exception,
                "Message {MessageId} exceeded max retries ({MaxRetries}); dead-lettering (CorrelationId={CorrelationId}, RoutingKey={RoutingKey})",
                messageId,
                _options.MaxRetryAttempts,
                correlationId,
                routingKey);
            throw new ConsumerRetryExhaustedException(
                $"Message {messageId} exceeded max retry attempts.",
                exception);
        }

        var delaySeconds = _options.InitialRetryDelaySeconds * Math.Pow(2, retryCount);
        logger.LogWarning(
            exception,
            "Message processing failed; scheduling retry {Attempt}/{MaxAttempts} after {DelaySeconds}s (MessageId={MessageId}, CorrelationId={CorrelationId}, RoutingKey={RoutingKey})",
            retryCount + 1,
            _options.MaxRetryAttempts,
            delaySeconds,
            messageId,
            correlationId,
            routingKey);

        await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);

        if (!channel.IsOpen)
        {
            throw new ConsumerChannelClosedException(
                $"Channel closed during retry delay for message {messageId}.");
        }

        var properties = new BasicProperties
        {
            ContentType = originalProperties.ContentType ?? "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = originalProperties.MessageId ?? messageId,
            CorrelationId = originalProperties.CorrelationId ?? correlationId,
            Headers = new Dictionary<string, object?> { ["x-retry-count"] = retryCount + 1 }
        };

        try
        {
            await channel.BasicPublishAsync(
                exchangeName,
                routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);
        }
        catch (AlreadyClosedException ex)
        {
            throw new ConsumerChannelClosedException(
                $"Channel closed while republishing message {messageId} for retry.",
                ex);
        }
    }

    public static int GetRetryCount(IDictionary<string, object?>? headers)
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
            byte[] bytes when int.TryParse(Encoding.UTF8.GetString(bytes), out var parsed) => parsed,
            _ => 0
        };
    }
}

public sealed class ConsumerRetryExhaustedException(string message, Exception innerException)
    : Exception(message, innerException);

public sealed class ConsumerChannelClosedException(string message, Exception? innerException = null)
    : Exception(message, innerException);
