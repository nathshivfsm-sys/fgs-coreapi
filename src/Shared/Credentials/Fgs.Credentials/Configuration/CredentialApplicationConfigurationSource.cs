using Fgs.Credentials.Mapping;
using Microsoft.Extensions.Configuration;

namespace Fgs.Credentials.Configuration;

internal sealed class CredentialApplicationConfigurationSource(CredentialConfigurationHolder holder)
    : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new CredentialApplicationConfigurationProvider(holder);
}

internal sealed class CredentialApplicationConfigurationProvider(CredentialConfigurationHolder holder)
    : ConfigurationProvider
{
    public override bool TryGet(string key, out string? value)
    {
        value = null;

        foreach (var credentialKey in holder.Values.Keys)
        {
            if (!CredentialSectionMapper.TryMap(credentialKey, out var mappedKey, out _))
            {
                continue;
            }

            if (!string.Equals(mappedKey, key, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (CredentialSectionMapper.TryResolveValue(credentialKey, key, holder.Values, out value))
            {
                return value is not null;
            }
        }

        return false;
    }

    public override void Set(string key, string? value) =>
        throw new NotSupportedException("Credential-backed application configuration is read-only.");
}
