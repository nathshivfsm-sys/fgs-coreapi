using Fgs.Contracts.Api;
using Fgs.Contracts.Auth;
using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for loading platform user auth profiles from UserService.
/// </summary>
public interface IUserAuthProfileClient
{
    [Get("/api/v1/internal/users/auth-profile")]
    Task<Fgs.Contracts.Api.ApiResponse<UserAuthProfileDto>> GetByEntraObjectIdAsync(
        [Query] string entraObjectId,
        CancellationToken cancellationToken = default);
}
