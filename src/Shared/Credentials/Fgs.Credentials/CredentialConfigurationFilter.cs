namespace Fgs.Credentials;

public static class CredentialConfigurationFilter
{
    public static IReadOnlyDictionary<string, string> Filter(
        IReadOnlyDictionary<string, string> values,
        IReadOnlyList<string> requiredProviders)
    {
        if (requiredProviders.Count == 0)
        {
            // Fail closed: empty allow-list must not return the full secret map.
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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

    /// <summary>
    /// Distribution snapshot for S2S: Global keys only, filtered by service allow-list,
    /// plus platform Redis / Entra / Datadog keys.
    /// </summary>
    public static IReadOnlyDictionary<string, string> FilterForServiceDistribution(
        IReadOnlyDictionary<string, string> values,
        IReadOnlyList<string> requiredProviders)
    {
        var globalOnly = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in values)
        {
            if (key.StartsWith("Global:", StringComparison.OrdinalIgnoreCase))
            {
                globalOnly[key] = value;
            }
        }

        var filtered = new Dictionary<string, string>(
            Filter(globalOnly, requiredProviders),
            StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in globalOnly)
        {
            if (IsPlatformAuthOrDistributionKey(key))
            {
                filtered[key] = value;
            }
        }

        return filtered;
    }

    public static IReadOnlyDictionary<string, string> GlobalOnly(
        IReadOnlyDictionary<string, string> values)
    {
        var globalOnly = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in values)
        {
            if (key.StartsWith("Global:", StringComparison.OrdinalIgnoreCase))
            {
                globalOnly[key] = value;
            }
        }

        return globalOnly;
    }

    private static bool IsPlatformAuthOrDistributionKey(string key) =>
        key.StartsWith("Global:REDIS:", StringComparison.OrdinalIgnoreCase)
        || key.StartsWith("Global:ENTRA_EXTERNAL_ID:", StringComparison.OrdinalIgnoreCase)
        || key.StartsWith("Global:DATADOG:", StringComparison.OrdinalIgnoreCase);

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
