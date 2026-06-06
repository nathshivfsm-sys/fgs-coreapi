using Refit;

namespace Fgs.Contracts.Clients;

public interface ISetupCredentialConfigurationClient
{
    [Get("/api/v1/credentials/resolved")]
    Task<Fgs.Contracts.Api.ApiResponse<ResolvedCredentialConfigurationDto>> GetResolvedAsync(
        [Header(InternalServiceHeaders.ServiceKey)] string internalServiceKey,
        CancellationToken cancellationToken = default);
}

public sealed record ResolvedCredentialConfigurationDto(
    IReadOnlyDictionary<string, string> Values);
