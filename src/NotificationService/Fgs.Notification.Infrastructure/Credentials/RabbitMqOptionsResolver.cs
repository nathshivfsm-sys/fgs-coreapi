using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.Infrastructure.Credentials;

/// <summary>
/// Resolves effective RabbitMQ options by merging appsettings with live credential holder values.
/// </summary>
public sealed class RabbitMqOptionsResolver(
    IOptionsMonitor<RabbitMqOptions> optionsMonitor,
    CredentialConfigurationHolder holder)
{
    public RabbitMqOptions Resolve() =>
        RabbitMqCredentialSettings.ResolveConnectionOptions(holder, optionsMonitor.CurrentValue);

    public bool HasVaultConnectionSettings() =>
        RabbitMqCredentialSettings.HasConnectionSettings(holder);
}
