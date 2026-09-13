using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Infrastructure.Credentials;

public sealed class SetupCredentialConfigurationProvider(
    CredentialConfigurationHolder holder,
    IServiceScopeFactory scopeFactory,
    CredentialOptionsChangeNotifier changeNotifier,
    ICredentialSnapshotRedisCache snapshotCache,
    ILogger<SetupCredentialConfigurationProvider> logger) : ICredentialConfigurationProvider
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

        // Distribute Global credentials only — tenant secrets stay in Setup memory / DB.
        var published = await snapshotCache.PublishAsync(
            CredentialConfigurationFilter.GlobalOnly(holder.Values),
            cancellationToken);
        if (!published)
        {
            logger.LogError(
                "Setup reloaded credentials in-memory but failed to publish Redis snapshot. "
                + "API peers will not receive the update until Redis is available or they restart.");
        }
    }
}
