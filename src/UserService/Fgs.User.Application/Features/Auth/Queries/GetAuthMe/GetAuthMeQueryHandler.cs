using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Security.Constants;
using Fgs.Security.Services;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Fgs.User.Application.Features.Auth.Queries.GetAuthMe;

public sealed class GetAuthMeQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    IFgsUserProfileResolver profileResolver)
    : IRequestHandler<GetAuthMeQuery, ApiResponse<FgsAuthMeDto>>
{
    public async Task<ApiResponse<FgsAuthMeDto>> Handle(GetAuthMeQuery request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User.Identity?.IsAuthenticated != true)
        {
            return ApiResponse<FgsAuthMeDto>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized);
        }

        var entraObjectId = httpContext.User.FindFirst(JwtClaimTypes.EntraObjectId)?.Value
            ?? httpContext.User.FindFirst("oid")?.Value
            ?? httpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

        if (string.IsNullOrWhiteSpace(entraObjectId))
        {
            return ApiResponse<FgsAuthMeDto>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized);
        }

        var profile = await profileResolver.ResolveByEntraObjectIdAsync(entraObjectId, cancellationToken);
        if (profile is null || string.IsNullOrWhiteSpace(profile.Email) || string.IsNullOrWhiteSpace(profile.EntraObjectId))
        {
            return ApiResponse<FgsAuthMeDto>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized);
        }

        var (headerTenantId, headerCompanyId) = FgsRequestAuthContext.ExtractTenantScope(httpContext);
        if (!AuthScopeValidation.TryValidateHeadersAgainstProfile(
                headerTenantId,
                headerCompanyId,
                profile,
                out var errors))
        {
            return ApiResponse<FgsAuthMeDto>.Fail(errors, ApiStatusCodes.Unauthorized);
        }

        return ApiResponse<FgsAuthMeDto>.Ok(new FgsAuthMeDto(
            profile.UserId,
            profile.Email,
            profile.EntraObjectId,
            profile.Roles));
    }
}
