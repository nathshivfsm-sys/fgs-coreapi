using Fgs.Setup.Application.Common;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Refit;

namespace Fgs.Notification.Infrastructure.Credentials;

public interface ISetupCredentialConfigurationClient
{
    [Get("/api/v1/credentials/resolved")]
    Task<Fgs.Foundation.Result.ApiResponse<ResolvedCredentialConfigurationDto>> GetResolvedAsync(
        [Header(CredentialDistributionHeaders.InternalServiceKey)] string internalServiceKey,
        CancellationToken cancellationToken = default);
}
