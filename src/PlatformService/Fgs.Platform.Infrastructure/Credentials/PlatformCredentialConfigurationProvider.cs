using Fgs.User.Application.Abstractions.Credentials;

namespace Fgs.Platform.Infrastructure.Credentials;

public sealed class PlatformCredentialConfigurationProvider : ICredentialConfigurationProvider
{
    private readonly CredentialConfigurationHolder _holder;
    private readonly RemoteCredentialConfigurationLoader _loader;
    private readonly CredentialOptionsChangeNotifier _changeNotifier;

    public PlatformCredentialConfigurationProvider(
        CredentialConfigurationHolder holder,
        RemoteCredentialConfigurationLoader loader,
        CredentialOptionsChangeNotifier changeNotifier)
    {
        _holder = holder;
        _loader = loader;
        _changeNotifier = changeNotifier;
    }

    public IReadOnlyDictionary<string, string> Values => _holder.Values;

    public string? GetValue(string key) => _holder.GetValue(key);

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        var values = await _loader.LoadAsync(cancellationToken);
        _holder.ReplaceValues(values);
        _changeNotifier.NotifyChange();
    }
}
