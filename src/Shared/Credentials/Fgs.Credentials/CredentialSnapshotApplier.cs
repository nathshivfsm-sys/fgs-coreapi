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

        // Keep Redis connection keys so snapshot pub/sub reload works even when REDIS
        // is not listed in RequiredProviders for the service.
        foreach (var (key, value) in values)
        {
            if (key.StartsWith("Global:REDIS:", StringComparison.OrdinalIgnoreCase))
            {
                filtered[key] = value;
            }
        }

        holder.ReplaceValues(filtered);
        changeNotifier.NotifyChange();
        return filtered.Count;
    }
}
