using Fgs.Messaging.Options;

namespace Fgs.Messaging.RabbitMq;

internal static class RabbitMqBrokerUriResolver
{
    public static Uri Resolve(RabbitMqOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ConnectionUri))
        {
            return new Uri(options.ConnectionUri.Trim(), UriKind.Absolute);
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
}
