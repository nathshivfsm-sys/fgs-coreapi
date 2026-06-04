using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure.Credentials;

/// <summary>
/// Maps decrypted credential keys (e.g. Global:SENDGRID:ApiKey) into standard appsettings sections
/// (e.g. SendGrid:ApiKey) so <see cref="IOptions{T}"/> binding works across services.
/// </summary>
internal sealed class CredentialApplicationConfigurationSource : IConfigurationSource
{
    private readonly CredentialConfigurationHolder _holder;

    public CredentialApplicationConfigurationSource(CredentialConfigurationHolder holder) =>
        _holder = holder;

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new CredentialApplicationConfigurationProvider(_holder);
}

internal sealed class CredentialApplicationConfigurationProvider : ConfigurationProvider
{
    private static readonly Dictionary<string, string> RabbitMqPropertyAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Host"] = "HostName",
        ["Username"] = "UserName"
    };

    private readonly CredentialConfigurationHolder _holder;

    public CredentialApplicationConfigurationProvider(CredentialConfigurationHolder holder) => _holder = holder;

    public override bool TryGet(string key, out string? value)
    {
        value = null;

        if (key.StartsWith("SendGrid:", StringComparison.OrdinalIgnoreCase))
        {
            var property = key["SendGrid:".Length..];
            return TryGetGlobalProviderValue("SENDGRID", property, out value);
        }

        if (key.StartsWith("RabbitMq:", StringComparison.OrdinalIgnoreCase))
        {
            var property = key["RabbitMq:".Length..];
            if (TryGetGlobalProviderValue("RABBITMQ", property, out value))
            {
                return true;
            }

            foreach (var alias in RabbitMqPropertyAliases)
            {
                if (!string.Equals(property, alias.Value, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (TryGetGlobalProviderValue("RABBITMQ", alias.Key, out value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override void Set(string key, string? value) =>
        throw new NotSupportedException("Credential-backed application configuration is read-only.");

    private bool TryGetGlobalProviderValue(string providerCode, string property, out string? value)
    {
        var credentialKey = $"Global:{providerCode}:{property}";
        return _holder.Values.TryGetValue(credentialKey, out value);
    }
}
