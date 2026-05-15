using System.Text;
using Fgs.User.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.User.Infrastructure.Messaging;

public sealed class RabbitMqPublisher(IOptions<RabbitMqOptions> options, ILogger<RabbitMqPublisher> logger) : IRabbitMqPublisher, IAsyncDisposable
{
    private readonly RabbitMqOptions _options = options.Value;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public async Task PublishAsync(
        string routingKey,
        string payload,
        string? correlationId,
        CancellationToken cancellationToken = default)
    {
        await EnsureChannelAsync(cancellationToken);

        var body = Encoding.UTF8.GetBytes(payload);
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = Guid.NewGuid().ToString(),
            CorrelationId = correlationId,
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        await _channel!.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);

        logger.LogInformation("Published message to {RoutingKey}", routingKey);
    }

    private async Task EnsureChannelAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
        {
            return;
        }

        await _initLock.WaitAsync(cancellationToken);
        try
        {
            if (_channel is not null)
            {
                return;
            }

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await _channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                cancellationToken: cancellationToken);
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        _initLock.Dispose();
    }
}
