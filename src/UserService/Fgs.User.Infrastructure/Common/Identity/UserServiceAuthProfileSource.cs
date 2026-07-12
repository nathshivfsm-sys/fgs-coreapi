using Fgs.Contracts.Auth;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Common.Identity;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class UserServiceAuthProfileSource(IFgsUserProfileResolver profileResolver) : IUserAuthProfileSource
{
    public async Task<UserAuthProfileDto?> LoadByEntraObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default)
    {
        var profile = await profileResolver.ResolveByEntraObjectIdAsync(entraObjectId, cancellationToken);
        return profile is null ? null : UserAuthProfileMapper.ToDto(profile);
    }
}
