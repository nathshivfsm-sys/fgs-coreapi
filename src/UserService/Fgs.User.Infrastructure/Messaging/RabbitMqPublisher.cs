using System.Net;
using System.Security.Authentication;
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
                ClientProvidedName = "Fgs.User",
                RequestedConnectionTimeout = TimeSpan.FromSeconds(
                    Math.Clamp(_options.ConnectionTimeoutSeconds, 5, 120)),
                // Recovery can complicate first-connect diagnostics; broker reconnects are handled by the outbox retry loop.
                AutomaticRecoveryEnabled = false
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

            try
            {
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
                await _channel.ExchangeDeclareAsync(
                    exchange: _options.ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                LogConnectionFailure(ex, factory);
                throw;
            }
        }
        finally
        {
            _initLock.Release();
        }
    }

    private void LogConnectionFailure(Exception ex, ConnectionFactory factory)
    {
        var safeEndpoint = SafeFormatBrokerUri(factory.Uri);
        var tls = factory.Ssl.Enabled;
        var sni = factory.Ssl.Enabled ? factory.Ssl.ServerName : "(n/a)";

        var innerMessages = ex is AggregateException agg
            ? string.Join(" | ", agg.InnerExceptions.Select(e => e.Message))
            : ex.InnerException?.Message ?? ex.Message;

        var hostForDns = factory.Uri?.Host ?? _options.HostName;
        string? dnsHint = null;
        if (!string.IsNullOrWhiteSpace(hostForDns))
        {
            try
            {
                var addrs = Dns.GetHostAddresses(hostForDns);
                dnsHint = $"DNS resolved {hostForDns} to {addrs.Length} address(es): {string.Join(", ", addrs.Take(4).Select(a => a.ToString()))}";
            }
            catch (Exception dnsEx)
            {
                dnsHint = $"DNS lookup failed for {hostForDns}: {dnsEx.Message}";
            }
        }

        logger.LogError(
            ex,
            "RabbitMQ connection failed. Broker {Broker}, TLS {Tls}, SNI {Sni}. Inner: {Inner}. {DnsHint}. " +
            "For local Docker: run `docker compose up -d` in src/UserService and confirm TCP {Port} is listening.",
            safeEndpoint,
            tls,
            sni,
            innerMessages,
            dnsHint ?? "DNS not checked.",
            factory.Uri?.Port ?? _options.Port);
    }

    private static string SafeFormatBrokerUri(Uri? uri)
    {
        if (uri is null)
        {
            return "(no URI)";
        }

        return uri.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped);
    }

    /// <summary>
    /// Prefer a single <see cref="ConnectionFactory.Uri"/> (built here or from config) so the client matches other
    /// stacks and avoids subtle HostName/Port/TLS mismatches on RabbitMQ.Client 7.
    /// </summary>
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

        if (_options.HostName.Contains("://", StringComparison.Ordinal))
        {
            return new Uri(_options.HostName.Trim(), UriKind.Absolute);
        }

        var useTls = _options.SslEnabled || UseTlsForAmazonMqStyleBroker(_options);
        var ub = new UriBuilder
        {
            Scheme = useTls ? "amqps" : "amqp",
            Host = _options.HostName,
            Port = _options.Port,
            Path = "/"
        };

        if (!string.IsNullOrWhiteSpace(_options.UserName))
        {
            ub.UserName = _options.UserName;
        }

        if (_options.Password is { Length: > 0 })
        {
            ub.Password = _options.Password;
        }

        return ub.Uri;
    }

    private static bool UseTlsForAmazonMqStyleBroker(RabbitMqOptions o) =>
        o.Port == 5671 && o.HostName.Contains(".mq.", StringComparison.OrdinalIgnoreCase);

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
