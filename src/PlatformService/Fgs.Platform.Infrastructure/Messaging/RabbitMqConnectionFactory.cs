using System.Security.Authentication;
using Fgs.Platform.Application.IntegrationEvents;
using Fgs.Platform.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.Platform.Infrastructure.Messaging;

public sealed class RabbitMqConnectionFactory(
    IOptions<RabbitMqOptions> options,
    ILogger<RabbitMqConnectionFactory> logger)
{
    private readonly RabbitMqOptions _options = options.Value;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private IConnection? _connection;

    public async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (_connection is { IsOpen: true })
        {
            return _connection;
        }

        await _initLock.WaitAsync(cancellationToken);
        try
        {
            if (_connection is { IsOpen: true })
            {
                return _connection;
            }

            var factory = new ConnectionFactory
            {
                ClientProvidedName = "Fgs.Platform",
                RequestedConnectionTimeout = TimeSpan.FromSeconds(
                    Math.Clamp(_options.ConnectionTimeoutSeconds, 5, 120)),
                AutomaticRecoveryEnabled = true
            };

            ApplyBrokerUri(factory);

            if (!string.IsNullOrWhiteSpace(_options.UserName))
            {
                factory.UserName = _options.UserName;
            }

            if (_options.Password is { Length: > 0 })
            {
                factory.Password = _options.Password;
            }

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            logger.LogInformation("RabbitMQ connection established for Platform service.");
            return _connection;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "RabbitMQ connection failed for Platform service.");
            throw;
        }
        finally
        {
            _initLock.Release();
        }
    }

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

    private void ApplyBrokerUri(ConnectionFactory factory)
    {
        var uri = ResolveBrokerUri();
        factory.Uri = uri;

        if (string.Equals(uri.Scheme, "amqps", StringComparison.OrdinalIgnoreCase))
        {
            factory.Ssl.Version = SslProtocols.Tls12 | SslProtocols.Tls13;
            factory.Ssl.CheckCertificateRevocation = _options.SslCheckCertificateRevocation;
        }
    }

    private Uri ResolveBrokerUri()
    {
        if (!string.IsNullOrWhiteSpace(_options.ConnectionUri))
        {
            return new Uri(_options.ConnectionUri.Trim(), UriKind.Absolute);
        }

        var useTls = _options.SslEnabled
            || (_options.Port == 5671 && _options.HostName.Contains(".mq.", StringComparison.OrdinalIgnoreCase));

        var ub = new UriBuilder
        {
            Scheme = useTls ? "amqps" : "amqp",
            Host = _options.HostName,
            Port = _options.Port,
            Path = "/",
            UserName = _options.UserName,
            Password = _options.Password
        };

        return ub.Uri;
    }
}
