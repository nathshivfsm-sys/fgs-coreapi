using System.Security.Claims;
using Fgs.Security.Abstractions;
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

        FgsClaimsEnrichment.Apply(
            principal,
            new FgsAuthenticatedUserProfile(
                profile.UserId,
                profile.Email,
                profile.EntraObjectId,
                profile.TenantId,
                profile.CompanyId,
                profile.Roles));
    }
}
