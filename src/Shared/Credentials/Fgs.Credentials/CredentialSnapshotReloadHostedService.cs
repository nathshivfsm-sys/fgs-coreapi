using Fgs.Credentials.Options;
using Fgs.Credentials.Redis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials;

/// <summary>
/// After HTTP bootstrap, listens for Redis credential-change signals and refreshes the in-memory holder.
/// </summary>
public sealed class CredentialSnapshotReloadHostedService(
    ICredentialSnapshotRedisCache snapshotCache,
    CredentialConfigurationHolder holder,
    CredentialOptionsChangeNotifier changeNotifier,
    IOptions<CredentialConsumerOptions> consumerOptions,
    ILogger<CredentialSnapshotReloadHostedService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        snapshotCache.SubscribeAsync(ReloadFromRedisAsync, stoppingToken);

    private async Task ReloadFromRedisAsync(CancellationToken cancellationToken)
    {
        var snapshot = await snapshotCache.GetAsync(cancellationToken);
        if (snapshot is null)
        {
            logger.LogWarning("Credential change signal received but Redis snapshot was empty.");
            return;
        }

        var count = CredentialSnapshotApplier.Apply(
            holder,
            changeNotifier,
            snapshot,
            consumerOptions.Value.RequiredProviders);

        logger.LogInformation("Reloaded {Count} credential configuration entries from Redis.", count);
    }
}
