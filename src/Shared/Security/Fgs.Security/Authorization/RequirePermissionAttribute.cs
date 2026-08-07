using Microsoft.AspNetCore.Mvc;

namespace Fgs.Security.Authorization;

/// <summary>
/// Requires the authenticated user profile to include at least one of the specified permission codes.
/// Tenant admins (<see cref="Constants.FgsRoleCodes.TenantAdmin"/>) bypass the check.
/// Internal service-key callers (no profile) are allowed so S2S paths keep working.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class RequirePermissionAttribute : TypeFilterAttribute
{
    public RequirePermissionAttribute(params string[] permissionCodes)
        : base(typeof(PermissionAuthorizationFilter))
    {
        Arguments = [permissionCodes];
        Order = 0;
    }
}
