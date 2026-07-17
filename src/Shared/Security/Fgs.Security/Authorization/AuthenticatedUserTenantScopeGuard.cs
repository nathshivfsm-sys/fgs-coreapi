using Fgs.Contracts.Api;
using Fgs.Security.Abstractions;
using Fgs.Security.Constants;

namespace Fgs.Security.Authorization;

public static class AuthenticatedUserTenantScopeGuard
{
    public static ApiResponse<T>? DenyCrossTenantAccess<T>(
        IFgsUserContext userContext,
        long requestedTenantId)
    {
        if (!userContext.IsAuthenticated)
        {
            return null;
        }

        if (userContext.TenantId is null || userContext.TenantId.Value != requestedTenantId)
        {
            return ApiResponse<T>.Fail(
                [UserAuthorizationMessages.TenantMismatch],
                ApiStatusCodes.Forbidden);
        }

        return null;
    }

    public static ApiResponse<T>? DenyCrossTenantCompanyAccess<T>(
        IFgsUserContext userContext,
        long requestedTenantId,
        long requestedCompanyId)
    {
        var tenantDenied = DenyCrossTenantAccess<T>(userContext, requestedTenantId);
        if (tenantDenied is not null)
        {
            return tenantDenied;
        }

        if (!userContext.IsAuthenticated)
        {
            return null;
        }

        if (userContext.IsInRole(FgsRoleCodes.TenantAdmin))
        {
            return null;
        }

        if (userContext.CompanyId is null || userContext.CompanyId.Value != requestedCompanyId)
        {
            return ApiResponse<T>.Fail(
                [UserAuthorizationMessages.CompanyMismatch],
                ApiStatusCodes.Forbidden);
        }

        return null;
    }
}
