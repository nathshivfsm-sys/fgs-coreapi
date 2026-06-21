using System.Security.Claims;
using Fgs.Contracts.Api;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Fgs.User.Application.Features.Auth.Queries.GetAuthMe;

public sealed class GetAuthMeQueryHandler(
    IFgsUserContext userContext,
    IFgsUserProfileResolver profileResolver,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetAuthMeQuery, ApiResponse<AuthMeDto>>
{
    public async Task<ApiResponse<AuthMeDto>> Handle(
        GetAuthMeQuery request,
        CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated)
        {
            return ApiResponse<AuthMeDto>.Fail(
                [AuthErrorMessages.Unauthenticated],
                ApiStatusCodes.Unauthorized);
        }

        FgsUserProfile? profile = null;
        if (!string.IsNullOrWhiteSpace(userContext.EntraObjectId))
        {
            profile = await profileResolver.ResolveByEntraObjectIdAsync(
                userContext.EntraObjectId,
                cancellationToken);
        }

        if (profile is null && !string.IsNullOrWhiteSpace(userContext.Email))
        {
            profile = await profileResolver.ResolveBySignupEmailAsync(
                userContext.Email.Trim(),
                cancellationToken);
        }

        if (profile is null)
        {
            return ApiResponse<AuthMeDto>.Fail(
                [AuthErrorMessages.UserNotFound],
                ApiStatusCodes.NotFound);
        }

        var claims = httpContextAccessor.HttpContext?.User;
        var tenantId = ResolveScopeId(
            userContext.TenantId,
            claims,
            "tenant_id");
        var companyId = ResolveScopeId(
            userContext.CompanyId,
            claims,
            "company_id");

        if (tenantId is null || companyId is null)
        {
            tenantId ??= profile.TenantId;
            companyId ??= profile.CompanyId;
        }

        return ApiResponse<AuthMeDto>.Ok(new AuthMeDto(
            profile.UserId,
            profile.Email,
            profile.EntraObjectId ?? userContext.EntraObjectId,
            tenantId ?? profile.TenantId,
            companyId ?? profile.CompanyId,
            profile.Roles));
    }

    private static long? ResolveScopeId(long? headerValue, ClaimsPrincipal? claims, string claimType)
    {
        if (headerValue is > 0)
        {
            return headerValue;
        }

        var claimValue = claims?.FindFirst(claimType)?.Value;
        return long.TryParse(claimValue, out var parsed) ? parsed : null;
    }
}
