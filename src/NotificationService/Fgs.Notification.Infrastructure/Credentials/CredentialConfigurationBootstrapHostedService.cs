using Fgs.Notification.Infrastructure.Credentials;
using Fgs.Setup.Application.Abstractions.Credentials;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

namespace Fgs.Notification.Infrastructure.Credentials;

/// <summary>
/// Loads credential configuration from User Service at startup (with retries) and keeps retrying in the
/// background when User Service was not yet available.
/// </summary>
public sealed class CredentialConfigurationBootstrapHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<UserServiceCredentialClientOptions> options,
    ILogger<CredentialConfigurationBootstrapHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var settings = options.Value;
            var loaded = await TryLoadWithRetriesAsync(
                settings.StartupRetryCount,
                settings.StartupRetryDelaySeconds,
                stoppingToken);
            if (loaded)
            {
                return;
            }

            if (settings.RequiredOnStartup)
            {
                throw new InvalidOperationException(
                    $"Failed to load credential configuration from User Service at {settings.BaseUrl}. " +
                    "Ensure User Service is running (http profile: http://localhost:5001) or set UserService:RequiredOnStartup to false for local development.");
            }

            logger.LogWarning(
                "User Service is not available at {BaseUrl}. Notification is using appsettings credentials until User Service starts.",
                settings.BaseUrl);

            var interval = TimeSpan.FromSeconds(Math.Max(5, settings.BackgroundRetryIntervalSeconds));
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(interval, stoppingToken);
                if (await TryLoadOnceAsync(stoppingToken))
                {
                    logger.LogInformation(
                        "Credential configuration loaded from User Service after it became available.");
                    return;
                }
            }
        }
        finally
        {
            logger.LogInformation("Credential configuration bootstrap completed.");
        }
    }

    private async Task<bool> TryLoadWithRetriesAsync(int retryCount, int delaySeconds, CancellationToken cancellationToken)
    {
        var attempts = Math.Max(1, retryCount);
        var delay = TimeSpan.FromSeconds(Math.Max(1, delaySeconds));

        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            if (await TryLoadOnceAsync(cancellationToken))
            {
                return true;
            }

            if (attempt < attempts)
            {
                logger.LogWarning(
                    "User Service credential load attempt {Attempt}/{MaxAttempts} failed; retrying in {DelaySeconds}s.",
                    attempt,
                    attempts,
                    delay.TotalSeconds);
                await Task.Delay(delay, cancellationToken);
            }
        }

        return false;
    }

    private async Task<bool> TryLoadOnceAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var provider = scope.ServiceProvider.GetRequiredService<ICredentialConfigurationProvider>();
            await provider.ReloadAsync(cancellationToken);
            return provider.Values.Count > 0 && HasSendGridVaultSettings(provider);
        }
        catch (Exception ex) when (IsUserServiceUnreachable(ex))
        {
            logger.LogDebug(ex, "User Service credential endpoint is not reachable yet.");
            return false;
        }
    }

    private static bool HasSendGridVaultSettings(ICredentialConfigurationProvider provider) =>
        !string.IsNullOrWhiteSpace(provider.GetValue("Global:SENDGRID:ApiKey"));

    private static bool IsUserServiceUnreachable(Exception ex) =>
        ex is HttpRequestException or ApiException
        || (ex is InvalidOperationException && ex.InnerException is not null && IsUserServiceUnreachable(ex.InnerException));
}
