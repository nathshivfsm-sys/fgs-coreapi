using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Mapping;

namespace Fgs.Credentials;

public sealed class CredentialConfigurationProvider(
    CredentialConfigurationHolder holder,
    RemoteCredentialConfigurationLoader loader,
    CredentialOptionsChangeNotifier changeNotifier) : ICredentialConfigurationProvider
{
    public IReadOnlyDictionary<string, string> Values => holder.Values;

    public string? GetValue(string key) => holder.GetValue(key);

    public string? GetConnectionString(string name)
    {
        var configurationKey = $"ConnectionStrings:{name}";
        foreach (var credentialKey in holder.Values.Keys)
        {
            if (CredentialSectionMapper.TryResolveValue(credentialKey, configurationKey, holder.Values, out var value))
            {
                return value;
            }
        }

        return null;
    }

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        await loader.LoadAsync(cancellationToken);
        changeNotifier.NotifyChange();
    }
}
