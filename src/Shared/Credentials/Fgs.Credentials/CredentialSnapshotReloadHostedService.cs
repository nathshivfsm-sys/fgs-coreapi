using Fgs.Credentials.Options;
using Fgs.Credentials.Redis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials;

/// <summary>
/// After bootstrap, listens for Redis credential-change signals and periodically
/// re-fetches the snapshot so missed pub/sub messages do not leave stale secrets.
/// </summary>
public sealed class CredentialSnapshotReloadHostedService(
    ICredentialSnapshotRedisCache snapshotCache,
    CredentialConfigurationHolder holder,
    CredentialOptionsChangeNotifier changeNotifier,
    IOptions<CredentialConsumerOptions> consumerOptions,
    ILogger<CredentialSnapshotReloadHostedService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscribe = snapshotCache.SubscribeAsync(ReloadFromRedisAsync, stoppingToken);
        var refresh = RunPeriodicRefreshAsync(stoppingToken);
        return Task.WhenAll(subscribe, refresh);
    }

    private async Task RunPeriodicRefreshAsync(CancellationToken stoppingToken)
    {
        var intervalSeconds = consumerOptions.Value.SnapshotRefreshIntervalSeconds;
        if (intervalSeconds <= 0)
        {
            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
            }

            return;
        }

        var delay = TimeSpan.FromSeconds(intervalSeconds);
        using var timer = new PeriodicTimer(delay);
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await ReloadFromRedisAsync(stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "Periodic credential snapshot refresh failed.");
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Host is stopping.
        }
    }

    private async Task ReloadFromRedisAsync(CancellationToken cancellationToken)
    {
        var snapshot = await snapshotCache.GetAsync(cancellationToken);
        if (snapshot is null)
        {
            logger.LogWarning("Credential snapshot reload skipped: Redis snapshot was empty or unavailable.");
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
