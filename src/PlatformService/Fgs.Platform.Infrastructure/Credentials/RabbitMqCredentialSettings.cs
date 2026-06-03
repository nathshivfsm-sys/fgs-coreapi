using Fgs.Messaging.Options;

namespace Fgs.Platform.Infrastructure.Credentials;

internal static class RabbitMqCredentialSettings
{
    private const string ProviderCode = "RABBITMQ";

    public static bool HasConnectionSettings(CredentialConfigurationHolder holder) =>
        TryGet(holder, "Password", out _)
        || TryGet(holder, "UserName", out _)
        || TryGet(holder, "Username", out _)
        || TryGet(holder, "HostName", out _)
        || TryGet(holder, "Host", out _)
        || TryGet(holder, "ConnectionUri", out _);

    public static void ApplyConnectionSettings(CredentialConfigurationHolder holder, RabbitMqOptions options)
    {
        if (TryGet(holder, "HostName", out var hostName) || TryGet(holder, "Host", out hostName))
        {
            options.HostName = hostName!;
        }

        if (TryGet(holder, "UserName", out var userName) || TryGet(holder, "Username", out userName))
        {
            options.UserName = userName!;
        }

        if (TryGet(holder, "Password", out var password))
        {
            options.Password = password!;
        }

        if (TryGet(holder, "Port", out var port) && int.TryParse(port, out var portNumber))
        {
            options.Port = portNumber;
        }

        if (TryGet(holder, "ConnectionUri", out var connectionUri) && !string.IsNullOrWhiteSpace(connectionUri))
        {
            options.ConnectionUri = connectionUri;
        }

        if (TryGet(holder, "SslEnabled", out var sslEnabled) && bool.TryParse(sslEnabled, out var ssl))
        {
            options.SslEnabled = ssl;
        }

        if (TryGet(holder, "SslServerName", out var sslServerName) && !string.IsNullOrWhiteSpace(sslServerName))
        {
            options.SslServerName = sslServerName;
        }
    }

    public static RabbitMqOptions ResolveConnectionOptions(
        CredentialConfigurationHolder holder,
        RabbitMqOptions baseOptions)
    {
        var resolved = Clone(baseOptions);
        ApplyConnectionSettings(holder, resolved);
        return resolved;
    }

    private static RabbitMqOptions Clone(RabbitMqOptions source) =>
        new()
        {
            ConnectionUri = source.ConnectionUri,
            HostName = source.HostName,
            Port = source.Port,
            UserName = source.UserName,
            Password = source.Password,
            SslEnabled = source.SslEnabled,
            SslServerName = source.SslServerName,
            SslCheckCertificateRevocation = source.SslCheckCertificateRevocation,
            ConnectionTimeoutSeconds = source.ConnectionTimeoutSeconds,
            ClientProvidedName = source.ClientProvidedName,
            AutomaticRecoveryEnabled = source.AutomaticRecoveryEnabled,
            ExchangeName = source.ExchangeName,
            RoutingKeyPrefix = source.RoutingKeyPrefix,
            EnsureQueuesOnStartup = source.EnsureQueuesOnStartup,
            NotificationQueueName = source.NotificationQueueName,
            DeadLetterQueueName = source.DeadLetterQueueName,
            DeadLetterExchangeName = source.DeadLetterExchangeName,
            QueueBindings = source.QueueBindings
        };

    private static bool TryGet(CredentialConfigurationHolder holder, string property, out string? value)
    {
        value = holder.GetValue($"Global:{ProviderCode}:{property}");
        return !string.IsNullOrWhiteSpace(value);
    }
}
