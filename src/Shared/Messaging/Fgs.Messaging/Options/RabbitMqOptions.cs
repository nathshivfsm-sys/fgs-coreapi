using Fgs.Contracts.IntegrationEvents;

namespace Fgs.Messaging.Options;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    /// <summary>
    /// Optional full AMQP URI (e.g. <c>amqps://user:pass@broker.mq.region.on.aws:5671</c>).
    /// When set, supplies broker credentials and address. If the URI host is loopback
    /// (<c>localhost</c> / <c>127.0.0.1</c>) and <see cref="HostName"/> is a non-loopback
    /// value (e.g. compose <c>rabbitmq</c>), the URI host is rewritten to <see cref="HostName"/>.
    /// </summary>
    public string? ConnectionUri { get; set; }

    public string HostName { get; set; } = "localhost";

    public int Port { get; set; } = 5672;

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";

    /// <summary>When not using <see cref="ConnectionUri"/>, enables TLS (e.g. Amazon MQ on port 5671).</summary>
    public bool SslEnabled { get; set; }

    /// <summary>Optional TLS SNI / certificate server name; defaults to <see cref="HostName"/> when SSL is on.</summary>
    public string? SslServerName { get; set; }

    /// <summary>AMQP TCP connect timeout (helps surface slow / blocked networks).</summary>
    public int ConnectionTimeoutSeconds { get; set; } = 30;

    /// <summary>When true, enables TLS certificate revocation checks.</summary>
    public bool SslCheckCertificateRevocation { get; set; }

    public string ClientProvidedName { get; set; } = "Fgs";

    public bool AutomaticRecoveryEnabled { get; set; }

    public string ExchangeName { get; set; } = IntegrationEventExchanges.UserEvents;

    public string RoutingKeyPrefix { get; set; } = "user.";

    /// <summary>
    /// Topic exchanges declared on first connect. When empty, <see cref="IntegrationEventExchanges.All"/> is used.
    /// </summary>
    public IList<string> ExchangeNames { get; set; } = [];

    /// <summary>
    /// When true, idempotently declares each <see cref="QueueBindings"/> queue and binds it on first connect.
    /// </summary>
    public bool EnsureQueuesOnStartup { get; set; } = true;

    /// <summary>Primary consumer queue name (Platform notifications).</summary>
    public string? NotificationQueueName { get; set; }

    public string? DeadLetterQueueName { get; set; }

    public string? DeadLetterExchangeName { get; set; }

    public IList<RabbitMqQueueBindingOptions> QueueBindings { get; set; } = [];
}
