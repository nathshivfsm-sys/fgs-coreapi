using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using Refit;

namespace Fgs.Platform.Infrastructure.Credentials;

public interface IUserCredentialConfigurationClient
{
    [Get("/api/v1/credentials/resolved")]
    Task<Fgs.Foundation.Result.ApiResponse<ResolvedCredentialConfigurationDto>> GetResolvedAsync(
        [Header(CredentialDistributionHeaders.InternalServiceKey)] string internalServiceKey,
        CancellationToken cancellationToken = default);
}
