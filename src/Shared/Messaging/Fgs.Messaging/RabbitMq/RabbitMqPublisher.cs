using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using System.Text;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Observability;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.Messaging.RabbitMq;

public sealed class RabbitMqPublisher(
    IOptions<RabbitMqOptions> options,
    ILogger<RabbitMqPublisher> logger,
    IFgsMetrics? metrics = null) : IRabbitMqPublisher, IAsyncDisposable
{
    private readonly RabbitMqOptions _options = options.Value;
    private readonly IFgsMetrics _metrics = metrics ?? NoOpFgsMetrics.Instance;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private readonly HashSet<string> _declaredExchanges = new(StringComparer.Ordinal);

    public Task PublishAsync(
        string routingKey,
        string payload,
        string? correlationId,
        CancellationToken cancellationToken = default) =>
        PublishAsync(_options.ExchangeName, routingKey, payload, correlationId, cancellationToken);

    public Task PublishAsync(
        string exchangeName,
        string routingKey,
        string payload,
        string? correlationId,
        CancellationToken cancellationToken = default)
    {
        var body = Encoding.UTF8.GetBytes(payload);
        var headers = new Dictionary<string, object?>();
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            headers["correlation_id"] = correlationId;
        }

        return PublishAsync(exchangeName, routingKey, body, headers, cancellationToken);
    }

    public async Task PublishAsync(
        string exchangeName,
        string routingKey,
        ReadOnlyMemory<byte> body,
        IDictionary<string, object?>? headers = null,
        CancellationToken cancellationToken = default)
    {
        await EnsureChannelAsync(cancellationToken);
        await EnsureExchangeDeclaredAsync(exchangeName, cancellationToken);

        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = Guid.NewGuid().ToString(),
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        if (headers is not null)
        {
            if (headers.TryGetValue("correlation_id", out var correlationId)
                && correlationId is string correlationIdText)
            {
                properties.CorrelationId = correlationIdText;
            }

            properties.Headers = new Dictionary<string, object?>(headers);
        }

        var sw = Stopwatch.StartNew();
        try
        {
            await _channel!.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);

            _metrics.Increment("rabbitmq.publish");
            _metrics.Histogram("rabbitmq.publish_latency_ms", sw.Elapsed.TotalMilliseconds);

            logger.LogInformation(
                "Published message to exchange {Exchange} routing key {RoutingKey}",
                exchangeName,
                routingKey);
        }
        catch
        {
            _metrics.Increment("rabbitmq.publish_failure");
            throw;
        }
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

            try
            {
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
                await EnsureExchangeAndQueuesAsync(cancellationToken);
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

    private async Task EnsureExchangeAndQueuesAsync(CancellationToken cancellationToken)
    {
        var channel = _channel!;
        foreach (var exchangeName in ResolveExchangeNames())
        {
            await EnsureExchangeDeclaredAsync(exchangeName, cancellationToken);
        }

        if (!_options.EnsureQueuesOnStartup || _options.QueueBindings.Count == 0)
        {
            return;
        }

        foreach (var binding in _options.QueueBindings)
        {
            if (string.IsNullOrWhiteSpace(binding.QueueName) || string.IsNullOrWhiteSpace(binding.RoutingKey))
            {
                logger.LogWarning(
                    "Skipping RabbitMQ queue binding with empty QueueName or RoutingKey.");
                continue;
            }

            var exchangeName = ResolveBindingExchangeName(binding);
            var queueName = binding.QueueName.Trim();
            var routingKey = binding.RoutingKey.Trim();

            await EnsureExchangeDeclaredAsync(exchangeName, cancellationToken);
            if (!string.IsNullOrWhiteSpace(binding.DeadLetterExchangeName))
            {
                await EnsureExchangeDeclaredAsync(binding.DeadLetterExchangeName.Trim(), cancellationToken);
            }

            await RabbitMqQueueTopology.EnsureQueueBindingAsync(
                channel,
                exchangeName,
                queueName,
                routingKey,
                binding.DeadLetterExchangeName,
                binding.DeadLetterQueueName,
                binding.DeadLetterRoutingKey,
                cancellationToken);

            logger.LogInformation(
                "Queue {Queue} ready on exchange {Exchange} (routing key {RoutingKey}).",
                queueName,
                exchangeName,
                routingKey);
        }
    }

    private IEnumerable<string> ResolveExchangeNames()
    {
        if (_options.ExchangeNames.Count > 0)
        {
            foreach (var exchangeName in _options.ExchangeNames)
            {
                if (!string.IsNullOrWhiteSpace(exchangeName))
                {
                    yield return exchangeName.Trim();
                }
            }

            yield break;
        }

        foreach (var exchangeName in IntegrationEventExchanges.All)
        {
            yield return exchangeName;
        }
    }

    private string ResolveBindingExchangeName(RabbitMqQueueBindingOptions binding) =>
        string.IsNullOrWhiteSpace(binding.ExchangeName)
            ? _options.ExchangeName
            : binding.ExchangeName.Trim();

    private async Task EnsureExchangeDeclaredAsync(string exchangeName, CancellationToken cancellationToken)
    {
        if (_channel is null || _declaredExchanges.Contains(exchangeName))
        {
            return;
        }

        await _channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        _declaredExchanges.Add(exchangeName);
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
            "RabbitMQ connection failed. Broker {Broker}, TLS {Tls}. Inner: {Inner}. {DnsHint}.",
            safeEndpoint,
            factory.Ssl.Enabled,
            innerMessages,
            dnsHint ?? "DNS not checked.");
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
