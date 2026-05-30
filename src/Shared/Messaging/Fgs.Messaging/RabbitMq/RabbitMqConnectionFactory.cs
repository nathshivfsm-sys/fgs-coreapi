using System.Net;
using System.Security.Authentication;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.Messaging.RabbitMq;

public sealed class RabbitMqConnectionFactory(
    IOptions<RabbitMqOptions> options,
    ILogger<RabbitMqConnectionFactory> logger) : IAsyncDisposable
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

            var factory = CreateConnectionFactory();
            try
            {
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                logger.LogInformation(
                    "RabbitMQ connection established ({ClientName}).",
                    _options.ClientProvidedName);
                return _connection;
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

    private ConnectionFactory CreateConnectionFactory()
    {
        var factory = new ConnectionFactory
        {
            ClientProvidedName = _options.ClientProvidedName,
            RequestedConnectionTimeout = TimeSpan.FromSeconds(
                Math.Clamp(_options.ConnectionTimeoutSeconds, 5, 120)),
            AutomaticRecoveryEnabled = _options.AutomaticRecoveryEnabled
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

        return factory;
    }

    private void ApplyBrokerUri(ConnectionFactory factory)
    {
        var uri = RabbitMqBrokerUriResolver.Resolve(_options);
        factory.Uri = uri;

        if (string.Equals(uri.Scheme, "amqps", StringComparison.OrdinalIgnoreCase))
        {
            factory.Ssl.Version = SslProtocols.Tls12 | SslProtocols.Tls13;
            factory.Ssl.CheckCertificateRevocation = _options.SslCheckCertificateRevocation;
        }
    }

    private void LogConnectionFailure(Exception ex, ConnectionFactory factory)
    {
        var safeEndpoint = factory.Uri is null
            ? "(no URI)"
            : factory.Uri.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped);

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
                dnsHint =
                    $"DNS resolved {hostForDns} to {addrs.Length} address(es): {string.Join(", ", addrs.Take(4).Select(a => a.ToString()))}";
            }
            catch (Exception dnsEx)
            {
                dnsHint = $"DNS lookup failed for {hostForDns}: {dnsEx.Message}";
            }
        }

        logger.LogError(
            ex,
            "RabbitMQ connection failed. Broker {Broker}, TLS {Tls}, SNI {Sni}. Inner: {Inner}. {DnsHint}.",
            safeEndpoint,
            factory.Ssl.Enabled,
            factory.Ssl.Enabled ? factory.Ssl.ServerName : "(n/a)",
            innerMessages,
            dnsHint ?? "DNS not checked.");
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        _initLock.Dispose();
    }
}
