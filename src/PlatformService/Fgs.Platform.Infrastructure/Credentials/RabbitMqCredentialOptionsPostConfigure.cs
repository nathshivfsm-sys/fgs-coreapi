using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Platform.Infrastructure.Credentials;

internal sealed class RabbitMqCredentialOptionsPostConfigure(CredentialConfigurationHolder holder)
    : IPostConfigureOptions<RabbitMqOptions>
{
    public void PostConfigure(string? name, RabbitMqOptions options) =>
        RabbitMqCredentialSettings.ApplyConnectionSettings(holder, options);
}
