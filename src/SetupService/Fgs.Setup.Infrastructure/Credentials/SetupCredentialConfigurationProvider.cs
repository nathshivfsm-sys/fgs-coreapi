using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Setup.Infrastructure.Credentials;

public sealed class SetupCredentialConfigurationProvider(
    CredentialConfigurationHolder holder,
    IServiceScopeFactory scopeFactory,
    CredentialOptionsChangeNotifier changeNotifier) : ICredentialConfigurationProvider
{
    public IReadOnlyDictionary<string, string> Values => holder.Values;

    public string? GetValue(string key) => holder.GetValue(key);

    public string? GetConnectionString(string name)
    {
        if (holder.Values.TryGetValue($"Global:DATABASE:{name}", out var direct))
        {
            return direct;
        }

        if (holder.Values.TryGetValue("Global:DATABASE:ConnectionStringName", out var named)
            && string.Equals(named, name, StringComparison.OrdinalIgnoreCase)
            && holder.Values.TryGetValue("Global:DATABASE:ConnectionString", out var namedConnection))
        {
            return namedConnection;
        }

        return null;
    }

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        using var scope = scopeFactory.CreateScope();
        var loader = scope.ServiceProvider.GetRequiredService<CredentialConfigurationLoader>();
        await loader.ReloadAsync(cancellationToken);
        changeNotifier.NotifyChange();
    }
}
