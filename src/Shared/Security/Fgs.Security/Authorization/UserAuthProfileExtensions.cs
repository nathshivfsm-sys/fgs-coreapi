using Fgs.Contracts.Auth;

namespace Fgs.Security.Authorization;

public static class UserAuthProfileExtensions
{
    public static bool IsInRole(this UserAuthProfileDto profile, string roleCode) =>
        profile.Roles.Contains(roleCode, StringComparer.OrdinalIgnoreCase);

    public static bool HasPermission(this UserAuthProfileDto profile, string permissionCode) =>
        profile.Permissions.Contains(permissionCode, StringComparer.OrdinalIgnoreCase);

    public static bool HasAnyPermission(this UserAuthProfileDto profile, params string[] permissionCodes) =>
        permissionCodes.Length > 0
        && permissionCodes.Any(code => profile.HasPermission(code));
}
