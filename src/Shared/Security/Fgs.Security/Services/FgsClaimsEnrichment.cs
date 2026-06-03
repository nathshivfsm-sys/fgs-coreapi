using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fgs.Security.Constants;
using Fgs.Security.Models;

namespace Fgs.Security.Services;

public static class FgsClaimsEnrichment
{
    public const string EnrichmentClaimType = "fgs_enriched";

    public static void Apply(ClaimsPrincipal principal, FgsAuthenticatedUserProfile profile)
    {
        if (principal.Identity is not ClaimsIdentity identity)
        {
            return;
        }

        SetClaim(identity, ClaimTypes.NameIdentifier, profile.UserId.ToString());
        SetClaim(identity, JwtRegisteredClaimNames.Sub, profile.UserId.ToString());
        SetClaim(identity, JwtRegisteredClaimNames.Email, profile.Email);
        SetClaim(identity, JwtClaimTypes.TenantId, profile.TenantId.ToString());
        SetClaim(identity, JwtClaimTypes.CompanyId, profile.CompanyId.ToString());
        SetClaim(identity, JwtClaimTypes.EntraObjectId, profile.EntraObjectId);

        foreach (var existing in identity.FindAll(ClaimTypes.Role).ToList())
        {
            identity.RemoveClaim(existing);
        }

        foreach (var role in profile.Roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        SetClaim(identity, EnrichmentClaimType, bool.TrueString);
    }

    public static bool IsEnriched(ClaimsPrincipal principal) =>
        principal.HasClaim(c => c.Type == EnrichmentClaimType && c.Value == bool.TrueString);

    private static void SetClaim(ClaimsIdentity identity, string claimType, string value)
    {
        foreach (var existing in identity.FindAll(claimType).ToList())
        {
            identity.RemoveClaim(existing);
        }

        identity.AddClaim(new Claim(claimType, value));
    }
}
