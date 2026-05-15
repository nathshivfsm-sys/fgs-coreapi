namespace Fgs.User.Infrastructure.Options;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    /// <summary>
    /// Optional full AMQP URI (e.g. <c>amqps://user:pass@broker.mq.region.on.aws:5671</c>).
    /// When set, overrides <see cref="HostName"/> and <see cref="Port"/> for the broker address.
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

    /// <summary>When true, enables TLS certificate revocation checks. Some environments fail CRL checks and appear as "unreachable".</summary>
    public bool SslCheckCertificateRevocation { get; set; }

    public string ExchangeName { get; set; } = "fgs.integration";

    public string RoutingKeyPrefix { get; set; } = "user.";
}
