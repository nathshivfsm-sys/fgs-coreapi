using Fgs.Contracts.Auth;

namespace Fgs.Security.UserAuth;

public interface IUserAuthProfileSource
{
    Task<UserAuthProfileDto?> LoadByEntraObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default);
}
