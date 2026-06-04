using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

namespace Fgs.Notification.Infrastructure.Credentials;

public sealed class RemoteCredentialConfigurationLoader
{
    private readonly ISetupCredentialConfigurationClient _client;
    private readonly UserServiceCredentialClientOptions _options;
    private readonly ILogger<RemoteCredentialConfigurationLoader> _logger;

    public RemoteCredentialConfigurationLoader(
        ISetupCredentialConfigurationClient client,
        IOptions<UserServiceCredentialClientOptions> options,
        ILogger<RemoteCredentialConfigurationLoader> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyDictionary<string, string>> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
        {
            throw new InvalidOperationException("SetupService:BaseUrl is required to load credential configuration.");
        }

        if (string.IsNullOrWhiteSpace(_options.InternalServiceKey))
        {
            throw new InvalidOperationException(
                "UserService:InternalServiceKey is required to load credential configuration.");
        }

        Fgs.Contracts.Api.ApiResponse<ResolvedCredentialConfigurationDto> envelope;
        try
        {
            envelope = await _client.GetResolvedAsync(_options.InternalServiceKey, cancellationToken);
        }
        catch (ApiException ex)
        {
            throw new InvalidOperationException(
                $"Setup Service returned HTTP {(int)ex.StatusCode} while loading credential configuration.",
                ex);
        }

        var data = Fgs.Contracts.Api.ApiResponseExtensions.EnsureSuccess(envelope);

        _logger.LogInformation(
            "Loaded {Count} credential configuration entries from Setup Service.",
            data.Values.Count);

        return data.Values;
    }
}
