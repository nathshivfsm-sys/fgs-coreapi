using Fgs.Contracts.Auth;
using Fgs.Security.Constants;
using Fgs.Security.UserAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fgs.Security.Authorization;

public sealed class PermissionAuthorizationFilter(string[] permissionCodes) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (permissionCodes.Length == 0)
        {
            return;
        }

        var httpContext = context.HttpContext;
        var profile = httpContext.Items[UserAuthHttpContextKeys.Profile] as UserAuthProfileDto;

        // Internal service-key requests skip ActiveUser middleware profile load.
        if (profile is null)
        {
            if (httpContext.User?.Identity?.IsAuthenticated == true)
            {
                context.Result = new ObjectResult(
                    Contracts.Api.ApiResponse<object>.Fail(
                        [UserAuthorizationMessages.InsufficientRole],
                        Contracts.Api.ApiStatusCodes.Forbidden))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }

            return;
        }

        if (profile.IsInRole(FgsRoleCodes.TenantAdmin)
            || profile.HasAnyPermission(permissionCodes))
        {
            return;
        }

        context.Result = new ObjectResult(
            Contracts.Api.ApiResponse<object>.Fail(
                [UserAuthorizationMessages.InsufficientRole],
                Contracts.Api.ApiStatusCodes.Forbidden))
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        await Task.CompletedTask;
    }
}
