using Fgs.Contracts.Clients;
using Fgs.Credentials.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials;

public sealed class RemoteCredentialConfigurationLoader
{
    private readonly ISetupClient _setupClient;
    private readonly CredentialConfigurationHolder _holder;
    private readonly CredentialOptionsChangeNotifier _changeNotifier;
    private readonly IOptions<CredentialDistributionOptions> _distributionOptions;
    private readonly IOptions<CredentialConsumerOptions> _consumerOptions;
    private readonly ILogger<RemoteCredentialConfigurationLoader> _logger;

    public RemoteCredentialConfigurationLoader(
        ISetupClient setupClient,
        CredentialConfigurationHolder holder,
        CredentialOptionsChangeNotifier changeNotifier,
        IOptions<CredentialDistributionOptions> distributionOptions,
        IOptions<CredentialConsumerOptions> consumerOptions,
        ILogger<RemoteCredentialConfigurationLoader> logger)
    {
        _setupClient = setupClient;
        _holder = holder;
        _changeNotifier = changeNotifier;
        _distributionOptions = distributionOptions;
        _consumerOptions = consumerOptions;
        _logger = logger;
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
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

                var filtered = CredentialConfigurationFilter.Filter(
                    response.Data.Values,
                    _consumerOptions.Value.RequiredProviders);

                _holder.ReplaceValues(filtered);
                _changeNotifier.NotifyChange();
                _logger.LogInformation(
                    "Loaded {Count} credential configuration entries from Setup Service.",
                    filtered.Count);
                return;
            }
            catch (Exception ex) when (attempt < maxAttempts)
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
            "Unable to load credential configuration from Setup Service after multiple attempts.");
    }
}
