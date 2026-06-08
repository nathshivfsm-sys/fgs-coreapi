namespace Fgs.Credentials;

internal static class CredentialConfigurationFilter
{
    public static IReadOnlyDictionary<string, string> Filter(
        IReadOnlyDictionary<string, string> values,
        IReadOnlyList<string> requiredProviders)
    {
        if (requiredProviders.Count == 0)
        {
            return values;
        }

        var allowed = new HashSet<string>(requiredProviders, StringComparer.OrdinalIgnoreCase);
        var filtered = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in values)
        {
            var providerCode = ExtractProviderCode(key);
            if (providerCode is not null && allowed.Contains(providerCode))
            {
                filtered[key] = value;
            }
        }

        return filtered;
    }

    private static string? ExtractProviderCode(string key)
    {
        if (key.StartsWith("Global:", StringComparison.OrdinalIgnoreCase))
        {
            var remainder = key["Global:".Length..];
            var separator = remainder.IndexOf(':');
            return separator > 0 ? remainder[..separator] : remainder;
        }

        if (key.StartsWith("Tenant:", StringComparison.OrdinalIgnoreCase))
        {
            var parts = key.Split(':');
            return parts.Length >= 4 ? parts[3] : null;
        }

        return null;
    }
}
