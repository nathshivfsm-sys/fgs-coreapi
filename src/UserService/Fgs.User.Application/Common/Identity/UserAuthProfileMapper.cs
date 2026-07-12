using Fgs.Contracts.Auth;
using Fgs.User.Application.Abstractions.Identity;

namespace Fgs.User.Application.Common.Identity;

public static class UserAuthProfileMapper
{
    public static UserAuthProfileDto ToDto(FgsUserProfile profile) =>
        new(
            profile.UserId,
            profile.Email,
            profile.EntraObjectId,
            profile.TenantId,
            profile.CompanyId,
            profile.IsActive,
            profile.IsDeleted,
            profile.Roles);
}
