using System.Security.Claims;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Security.Abstractions;
using Fgs.Security.Models;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Services;

public sealed class RemoteFgsClaimsEnricher(
    IFgsClaimsClient claimsClient,
    IHttpContextAccessor httpContextAccessor) : IFgsClaimsEnricher
{
    public async Task EnrichAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        if (FgsClaimsEnrichment.IsEnriched(principal))
        {
            return;
        }

        var httpContext = httpContextAccessor.HttpContext;
        var accessToken = FgsRequestAuthContext.ExtractBearerToken(httpContext);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return;
        }

        var (tenantId, companyId) = FgsRequestAuthContext.ExtractTenantScope(httpContext);

        ApiResponse<FgsAuthMeDto> response;
        try
        {
            response = await claimsClient.GetMeAsync(
                $"Bearer {accessToken}",
                tenantId,
                companyId,
                cancellationToken);
        }
        catch
        {
            return;
        }

        if (!response.Success || response.Data is null)
        {
            return;
        }

        var dto = response.Data;
        FgsClaimsEnrichment.Apply(
            principal,
            new FgsAuthenticatedUserProfile(
                dto.UserId,
                dto.Email,
                dto.EntraObjectId,
                dto.Roles));
    }
}
