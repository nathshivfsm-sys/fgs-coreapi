using Fgs.Credentials.Mapping;
using Microsoft.Extensions.Configuration;

namespace Fgs.Credentials.Configuration;

public static class CredentialApplicationConfigurationExtensions
{
    public static IConfigurationBuilder AddFgsCredentialApplicationConfiguration(
        this IConfigurationBuilder builder,
        CredentialConfigurationHolder holder) =>
        builder.Add(new CredentialApplicationConfigurationSource(holder));
}

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

    public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var credentialKey in holder.Values.Keys)
        {
            if (!CredentialSectionMapper.TryMap(credentialKey, out var mappedKey, out _))
            {
                continue;
            }

            var fullPath = mappedKey.Replace(":", ConfigurationPath.KeyDelimiter, StringComparison.Ordinal);

            if (string.IsNullOrEmpty(parentPath))
            {
                var segment = fullPath.Split(ConfigurationPath.KeyDelimiter)[0];
                if (!string.IsNullOrEmpty(segment))
                {
                    keys.Add(segment);
                }

                continue;
            }

            if (!fullPath.StartsWith(parentPath + ConfigurationPath.KeyDelimiter, StringComparison.OrdinalIgnoreCase)
                && !fullPath.Equals(parentPath, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var suffix = fullPath.Length > parentPath.Length
                ? fullPath[(parentPath.Length + 1)..]
                : string.Empty;

            if (string.IsNullOrEmpty(suffix))
            {
                continue;
            }

            var nextSegment = suffix.Split(ConfigurationPath.KeyDelimiter)[0];
            if (!string.IsNullOrEmpty(nextSegment))
            {
                keys.Add(nextSegment);
            }
        }

        return keys.Concat(earlierKeys).OrderBy(static k => k, ConfigurationKeyComparer.Instance);
    }
}
