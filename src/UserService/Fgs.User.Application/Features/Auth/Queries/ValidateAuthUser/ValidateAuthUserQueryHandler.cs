using Fgs.Contracts.Api;
using Fgs.Security.Constants;
using Fgs.Security.Services;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Fgs.User.Application.Features.Auth.Queries.ValidateAuthUser;

public sealed class ValidateAuthUserQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    IFgsUserProfileResolver profileResolver)
    : IRequestHandler<ValidateAuthUserQuery, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(ValidateAuthUserQuery request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User.Identity?.IsAuthenticated != true)
        {
            return ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized);
        }

        var entraObjectId = httpContext.User.FindFirst(JwtClaimTypes.EntraObjectId)?.Value
            ?? httpContext.User.FindFirst("oid")?.Value
            ?? httpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

        if (string.IsNullOrWhiteSpace(entraObjectId))
        {
            return ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized);
        }

        var profile = await profileResolver.ResolveByEntraObjectIdAsync(entraObjectId, cancellationToken);
        if (profile is null)
        {
            return ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized);
        }

        var (headerTenantId, headerCompanyId) = FgsRequestAuthContext.ExtractTenantScope(httpContext);
        if (!AuthScopeValidation.TryValidateHeadersAgainstProfile(
                headerTenantId,
                headerCompanyId,
                profile,
                out var errors))
        {
            return ApiResponse<object>.Fail(errors, ApiStatusCodes.Unauthorized);
        }

        return ApiResponse<object>.Ok(new { });
    }
}
