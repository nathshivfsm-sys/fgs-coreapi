namespace Fgs.Credentials;

/// <summary>
/// Single place to push a credential snapshot into the in-memory holder.
/// </summary>
internal static class CredentialSnapshotApplier
{
    public static int Apply(
        CredentialConfigurationHolder holder,
        CredentialOptionsChangeNotifier changeNotifier,
        IReadOnlyDictionary<string, string> values,
        IReadOnlyList<string> requiredProviders)
    {
        var filtered = new Dictionary<string, string>(
            CredentialConfigurationFilter.Filter(values, requiredProviders),
            StringComparer.OrdinalIgnoreCase);

        // Always keep Redis (snapshot pub/sub) and Entra (JWT bearer) even when not listed
        // in RequiredProviders — most API hosts register AddFgsApiSecurity.
        foreach (var (key, value) in values)
        {
            if (IsPlatformAuthOrDistributionKey(key))
            {
                filtered[key] = value;
            }
        }

        holder.ReplaceValues(filtered);
        changeNotifier.NotifyChange();
        return filtered.Count;
    }

    private static bool IsPlatformAuthOrDistributionKey(string key) =>
        key.StartsWith("Global:REDIS:", StringComparison.OrdinalIgnoreCase)
        || key.StartsWith("Global:ENTRA_EXTERNAL_ID:", StringComparison.OrdinalIgnoreCase);
}
