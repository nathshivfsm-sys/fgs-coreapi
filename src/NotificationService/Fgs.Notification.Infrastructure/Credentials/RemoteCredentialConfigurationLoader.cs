using Fgs.Notification.Infrastructure.Options;
using Fgs.User.Application.Features.Credentials.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

namespace Fgs.Notification.Infrastructure.Credentials;

public sealed class RemoteCredentialConfigurationLoader
{
    private readonly IUserCredentialConfigurationClient _client;
    private readonly UserServiceCredentialClientOptions _options;
    private readonly ILogger<RemoteCredentialConfigurationLoader> _logger;

    public RemoteCredentialConfigurationLoader(
        IUserCredentialConfigurationClient client,
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
            throw new InvalidOperationException("UserService:BaseUrl is required to load credential configuration.");
        }

        if (string.IsNullOrWhiteSpace(_options.InternalServiceKey))
        {
            throw new InvalidOperationException(
                "UserService:InternalServiceKey is required to load credential configuration.");
        }

        Fgs.Foundation.Result.ApiResponse<ResolvedCredentialConfigurationDto> envelope;
        try
        {
            envelope = await _client.GetResolvedAsync(_options.InternalServiceKey, cancellationToken);
        }
        catch (ApiException ex)
        {
            throw new InvalidOperationException(
                $"User Service returned HTTP {(int)ex.StatusCode} while loading credential configuration.",
                ex);
        }

        if (envelope is not { Success: true, Data: not null })
        {
            var errors = envelope?.Errors is { Count: > 0 } errorsList
                ? string.Join("; ", errorsList)
                : "Unknown error";
            throw new InvalidOperationException(
                $"User Service returned an error while loading credential configuration: {errors}");
        }

        _logger.LogInformation(
            "Loaded {Count} credential configuration entries from User Service.",
            envelope.Data.Values.Count);

        return envelope.Data.Values;
    }
}
