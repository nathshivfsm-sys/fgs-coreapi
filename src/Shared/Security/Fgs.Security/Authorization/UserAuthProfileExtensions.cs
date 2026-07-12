using Fgs.Contracts.Auth;

namespace Fgs.Security.Authorization;

public static class UserAuthProfileExtensions
{
    public static bool IsInRole(this UserAuthProfileDto profile, string roleCode) =>
        profile.Roles.Contains(roleCode, StringComparer.OrdinalIgnoreCase);
}
