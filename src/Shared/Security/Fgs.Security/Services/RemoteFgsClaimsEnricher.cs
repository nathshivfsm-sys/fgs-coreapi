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

        var accessToken = ExtractBearerToken(httpContextAccessor.HttpContext);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return;
        }

        ApiResponse<FgsAuthMeDto> response;
        try
        {
            response = await claimsClient.GetMeAsync($"Bearer {accessToken}", cancellationToken);
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
                dto.TenantId,
                dto.CompanyId,
                dto.Roles));
    }

    private static string? ExtractBearerToken(HttpContext? httpContext)
    {
        var authorization = httpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization)
            || !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authorization["Bearer ".Length..].Trim();
    }
}
