using Fgs.Contracts.Clients;
using Fgs.Credentials.Options;
using Fgs.Credentials.Redis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials;

public sealed class RemoteCredentialConfigurationLoader
{
    private readonly ISetupClient _setupClient;
    private readonly ICredentialSnapshotRedisCache _snapshotCache;
    private readonly CredentialConfigurationHolder _holder;
    private readonly CredentialOptionsChangeNotifier _changeNotifier;
    private readonly IOptions<CredentialDistributionOptions> _distributionOptions;
    private readonly IOptions<CredentialConsumerOptions> _consumerOptions;
    private readonly ILogger<RemoteCredentialConfigurationLoader> _logger;

    public RemoteCredentialConfigurationLoader(
        ISetupClient setupClient,
        ICredentialSnapshotRedisCache snapshotCache,
        CredentialConfigurationHolder holder,
        CredentialOptionsChangeNotifier changeNotifier,
        IOptions<CredentialDistributionOptions> distributionOptions,
        IOptions<CredentialConsumerOptions> consumerOptions,
        ILogger<RemoteCredentialConfigurationLoader> logger)
    {
        _setupClient = setupClient;
        _snapshotCache = snapshotCache;
        _holder = holder;
        _changeNotifier = changeNotifier;
        _distributionOptions = distributionOptions;
        _consumerOptions = consumerOptions;
        _logger = logger;
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        if (await TryLoadFromRedisAsync(cancellationToken))
        {
            return;
        }

        await LoadFromSetupAsync(cancellationToken);
    }

    private async Task<bool> TryLoadFromRedisAsync(CancellationToken cancellationToken)
    {
        try
        {
            var snapshot = await _snapshotCache.GetAsync(cancellationToken);
            if (snapshot is null || snapshot.Count == 0)
            {
                _logger.LogInformation(
                    "Redis credential snapshot unavailable; falling back to Setup Service.");
                return false;
            }

            var count = CredentialSnapshotApplier.Apply(
                _holder,
                _changeNotifier,
                snapshot,
                _consumerOptions.Value.RequiredProviders);

            if (count == 0)
            {
                _logger.LogWarning(
                    "Redis credential snapshot was empty after provider filter; falling back to Setup Service.");
                return false;
            }

            _logger.LogInformation(
                "Loaded {Count} credential configuration entries from Redis.",
                count);
            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(
                ex,
                "Redis credential snapshot load failed; falling back to Setup Service.");
            return false;
        }
    }

    private async Task LoadFromSetupAsync(CancellationToken cancellationToken)
    {
        const int maxAttempts = 5;
        var delay = TimeSpan.FromSeconds(2);

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var serviceKey = _distributionOptions.Value.InternalServiceKey;
                if (string.IsNullOrWhiteSpace(serviceKey))
                {
                    throw new InvalidOperationException(
                        $"{CredentialDistributionOptions.SectionName}:InternalServiceKey is required.");
                }

                var serviceName = string.IsNullOrWhiteSpace(_consumerOptions.Value.ServiceName)
                    ? null
                    : _consumerOptions.Value.ServiceName;

                var response = await _setupClient.GetResolvedCredentialsAsync(
                    serviceKey,
                    serviceName,
                    cancellationToken);

                if (!response.Success || response.Data is null)
                {
                    throw new InvalidOperationException(
                        response.Errors.Count > 0
                            ? string.Join("; ", response.Errors)
                            : "Failed to load resolved credentials from Setup Service.");
                }

                var count = CredentialSnapshotApplier.Apply(
                    _holder,
                    _changeNotifier,
                    response.Data.Values,
                    _consumerOptions.Value.RequiredProviders);

                _logger.LogInformation(
                    "Loaded {Count} credential configuration entries from Setup Service.",
                    count);
                return;
            }
            catch (Exception ex) when (attempt < maxAttempts && ex is not OperationCanceledException)
            {
                _logger.LogWarning(
                    ex,
                    "Credential load attempt {Attempt}/{MaxAttempts} failed; retrying in {DelaySeconds}s.",
                    attempt,
                    maxAttempts,
                    delay.TotalSeconds);
                await Task.Delay(delay, cancellationToken);
                delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 30));
            }
        }

        throw new InvalidOperationException(
            "Unable to load credential configuration from Redis or Setup Service after multiple attempts.");
    }
}
