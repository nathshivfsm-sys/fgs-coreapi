using Fgs.Credentials;
using Fgs.Setup.Application.Common.Options;
using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure.Credentials;

internal sealed class CredentialConfigurationSource : IConfigurationSource
{
    private readonly CredentialConfigurationHolder _holder;

    public CredentialConfigurationSource(CredentialConfigurationHolder holder) => _holder = holder;

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new ResolvedCredentialConfigurationProvider(_holder);
}

internal sealed class ResolvedCredentialConfigurationProvider : ConfigurationProvider
{
    private readonly CredentialConfigurationHolder _holder;

    public ResolvedCredentialConfigurationProvider(CredentialConfigurationHolder holder) => _holder = holder;

    public override bool TryGet(string key, out string? value)
    {
        var sectionPrefix = CredentialConfigurationOptions.SectionName + ConfigurationPath.KeyDelimiter;
        if (!key.StartsWith(sectionPrefix, StringComparison.OrdinalIgnoreCase))
        {
            value = null;
            return false;
        }

        var credentialKey = key[sectionPrefix.Length..];
        if (!_holder.Values.TryGetValue(credentialKey, out var stored))
        {
            value = null;
            return false;
        }

        value = stored;
        return true;
    }

    public override void Set(string key, string? value) =>
        throw new NotSupportedException("Resolved credential configuration is read-only.");

    public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        var section = CredentialConfigurationOptions.SectionName;
        var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var holderKey in _holder.Values.Keys)
        {
            var fullPath = section + ConfigurationPath.KeyDelimiter +
                           holderKey.Replace(":", ConfigurationPath.KeyDelimiter, StringComparison.Ordinal);

            if (string.IsNullOrEmpty(parentPath))
            {
                if (fullPath.StartsWith(section + ConfigurationPath.KeyDelimiter, StringComparison.OrdinalIgnoreCase))
                {
                    var relative = fullPath[(section.Length + 1)..];
                    var segment = relative.Split(ConfigurationPath.KeyDelimiter)[0];
                    if (!string.IsNullOrEmpty(segment))
                    {
                        keys.Add(segment);
                    }
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
