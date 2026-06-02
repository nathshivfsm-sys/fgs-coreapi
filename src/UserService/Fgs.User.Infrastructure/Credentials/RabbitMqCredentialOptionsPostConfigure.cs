using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Credentials;

internal sealed class RabbitMqCredentialOptionsPostConfigure(CredentialConfigurationHolder holder)
    : IPostConfigureOptions<RabbitMqOptions>
{
    private const string ProviderCode = "RABBITMQ";

    public void PostConfigure(string? name, RabbitMqOptions options)
    {
        if (TryGetCredential("HostName", out var hostName) || TryGetCredential("Host", out hostName))
        {
            options.HostName = hostName!;
        }

        if (TryGetCredential("UserName", out var userName) || TryGetCredential("Username", out userName))
        {
            options.UserName = userName!;
        }

        if (TryGetCredential("Password", out var password))
        {
            options.Password = password!;
        }

        if (TryGetCredential("Port", out var port) && int.TryParse(port, out var portNumber))
        {
            options.Port = portNumber;
        }

        if (TryGetCredential("ConnectionUri", out var connectionUri) && !string.IsNullOrWhiteSpace(connectionUri))
        {
            options.ConnectionUri = connectionUri;
        }

        if (TryGetCredential("SslEnabled", out var sslEnabled) && bool.TryParse(sslEnabled, out var ssl))
        {
            options.SslEnabled = ssl;
        }
    }

    private bool TryGetCredential(string property, out string? value)
    {
        value = holder.GetValue($"Global:{ProviderCode}:{property}");
        return !string.IsNullOrWhiteSpace(value);
    }
}
