using Fgs.Messaging.Options;

namespace Fgs.Messaging.RabbitMq;

internal static class RabbitMqBrokerUriResolver
{
    public static Uri Resolve(RabbitMqOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ConnectionUri))
        {
            var uri = new Uri(options.ConnectionUri.Trim(), UriKind.Absolute);
            return RewriteLoopbackHostWhenComposeHostSet(uri, options);
        }

        if (options.HostName.Contains("://", StringComparison.Ordinal))
        {
            return new Uri(options.HostName.Trim(), UriKind.Absolute);
        }

        var useTls = options.SslEnabled || UseTlsForAmazonMqStyleBroker(options);
        var ub = new UriBuilder
        {
            Scheme = useTls ? "amqps" : "amqp",
            Host = options.HostName,
            Port = options.Port,
            Path = "/"
        };

        if (!string.IsNullOrWhiteSpace(options.UserName))
        {
            ub.UserName = options.UserName;
        }

        if (options.Password is { Length: > 0 })
        {
            ub.Password = options.Password;
        }

        return ub.Uri;
    }

    public static bool UseTlsForAmazonMqStyleBroker(RabbitMqOptions options) =>
        options.Port == 5671 && options.HostName.Contains(".mq.", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Compose sets <c>RabbitMq__HostName=rabbitmq</c> on Docker hosts. Credential
    /// <c>ConnectionUri</c> often still points at localhost from local/dev seeding —
    /// rewrite the URI host so in-container brokers are reachable.
    /// Non-loopback URIs (e.g. Amazon MQ) are left unchanged.
    /// </summary>
    private static Uri RewriteLoopbackHostWhenComposeHostSet(Uri connectionUri, RabbitMqOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.HostName)
            || IsLoopbackHost(options.HostName)
            || !IsLoopbackHost(connectionUri.Host))
        {
            return connectionUri;
        }

        var builder = new UriBuilder(connectionUri)
        {
            Host = options.HostName.Trim()
        };

        if (options.Port > 0)
        {
            builder.Port = options.Port;
        }

        return builder.Uri;
    }

    private static bool IsLoopbackHost(string host) =>
        string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
        || string.Equals(host, "127.0.0.1", StringComparison.OrdinalIgnoreCase)
        || string.Equals(host, "::1", StringComparison.OrdinalIgnoreCase)
        || string.Equals(host, "[::1]", StringComparison.OrdinalIgnoreCase);
}
