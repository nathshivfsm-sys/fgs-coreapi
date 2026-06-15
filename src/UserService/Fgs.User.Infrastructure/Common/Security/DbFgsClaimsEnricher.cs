using System.Security.Claims;
using Fgs.Security.Abstractions;
using Fgs.Security.Constants;
using Fgs.Security.Models;
using Fgs.Security.Services;
using Fgs.User.Application.Abstractions.Identity;

namespace Fgs.User.Infrastructure.Common.Security;

public sealed class DbFgsClaimsEnricher(IFgsUserProfileResolver profileResolver) : IFgsClaimsEnricher
{
    public async Task EnrichAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        if (FgsClaimsEnrichment.IsEnriched(principal))
        {
            return;
        }

        var entraObjectId = principal.FindFirst("oid")?.Value
            ?? principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
            ?? principal.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(entraObjectId))
        {
            return;
        }

        var profile = await profileResolver.ResolveByEntraObjectIdAsync(entraObjectId, cancellationToken);
        if (profile is null)
        {
            return;
        }

        if (!TryValidateTokenScopeClaims(principal, profile.TenantId, profile.CompanyId))
        {
            return;
        }

        FgsClaimsEnrichment.Apply(
            principal,
            new FgsAuthenticatedUserProfile(
                profile.UserId,
                profile.Email,
                profile.EntraObjectId ?? entraObjectId,
                profile.TenantId,
                profile.CompanyId,
                profile.Roles));
    }

    private static bool TryValidateTokenScopeClaims(ClaimsPrincipal principal, long dbTenantId, long dbCompanyId)
    {
        var tokenTenant = principal.FindFirst(JwtClaimTypes.TenantId)?.Value;
        var tokenCompany = principal.FindFirst(JwtClaimTypes.CompanyId)?.Value;

        if (string.IsNullOrWhiteSpace(tokenTenant) || string.IsNullOrWhiteSpace(tokenCompany))
        {
            return true;
        }

        return long.TryParse(tokenTenant, out var claimTenantId)
               && long.TryParse(tokenCompany, out var claimCompanyId)
               && claimTenantId == dbTenantId
               && claimCompanyId == dbCompanyId;
    }
}
