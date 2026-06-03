using System.Net;
using System.Security.Authentication;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.Messaging.RabbitMq;

public sealed class RabbitMqConnectionFactory : IAsyncDisposable
{
    private readonly IRabbitMqEffectiveOptionsProvider _effectiveOptions;
    private readonly IOptionsMonitor<RabbitMqOptions> _optionsMonitor;
    private readonly ILogger<RabbitMqConnectionFactory> _logger;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private readonly IDisposable? _optionsChangeSubscription;
    private IConnection? _connection;

    public RabbitMqConnectionFactory(
        IRabbitMqEffectiveOptionsProvider effectiveOptions,
        IOptionsMonitor<RabbitMqOptions> optionsMonitor,
        ILogger<RabbitMqConnectionFactory> logger)
    {
        _effectiveOptions = effectiveOptions;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
        _optionsChangeSubscription = optionsMonitor.OnChange((_, _) => _ = ResetConnectionAsync());
    }

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

            var options = _effectiveOptions.GetEffectiveOptions();
            var factory = CreateConnectionFactory(options);
            try
            {
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                _logger.LogInformation(
                    "RabbitMQ connection established ({ClientName}) to {HostName}:{Port} as {UserName}.",
                    options.ClientProvidedName,
                    options.HostName,
                    options.Port,
                    options.UserName);
                return _connection;
            }
            catch (Exception ex)
            {
                LogConnectionFailure(ex, factory, options);
                throw;
            }
        }
        finally
        {
            _initLock.Release();
        }
    }

    private ConnectionFactory CreateConnectionFactory(RabbitMqOptions options)
    {
        var factory = new ConnectionFactory
        {
            ClientProvidedName = options.ClientProvidedName,
            RequestedConnectionTimeout = TimeSpan.FromSeconds(
                Math.Clamp(options.ConnectionTimeoutSeconds, 5, 120)),
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled
        };

        ApplyBrokerUri(factory, options);

        if (!string.IsNullOrWhiteSpace(options.UserName))
        {
            factory.UserName = options.UserName;
        }

        if (options.Password is { Length: > 0 })
        {
            factory.Password = options.Password;
        }

        return factory;
    }

    private static void ApplyBrokerUri(ConnectionFactory factory, RabbitMqOptions options)
    {
        var uri = RabbitMqBrokerUriResolver.Resolve(options);
        factory.Uri = uri;

        if (string.Equals(uri.Scheme, "amqps", StringComparison.OrdinalIgnoreCase))
        {
            factory.Ssl.Version = SslProtocols.Tls12 | SslProtocols.Tls13;
            factory.Ssl.CheckCertificateRevocation = options.SslCheckCertificateRevocation;
        }
    }

    private async Task ResetConnectionAsync()
    {
        await _initLock.WaitAsync();
        try
        {
            if (_connection is null)
            {
                return;
            }

            _logger.LogInformation("Resetting RabbitMQ connection after credential/options change.");
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while resetting RabbitMQ connection.");
        }
        finally
        {
            _initLock.Release();
        }
    }

    private void LogConnectionFailure(Exception ex, ConnectionFactory factory, RabbitMqOptions options)
    {
        var safeEndpoint = factory.Uri is null
            ? "(no URI)"
            : factory.Uri.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped);

        var innerMessages = ex is AggregateException agg
            ? string.Join(" | ", agg.InnerExceptions.Select(e => e.Message))
            : ex.InnerException?.Message ?? ex.Message;

        var hostForDns = factory.Uri?.Host ?? options.HostName;
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

        _logger.LogError(
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
        _optionsChangeSubscription?.Dispose();
        await ResetConnectionAsync();
        _initLock.Dispose();
    }
}
