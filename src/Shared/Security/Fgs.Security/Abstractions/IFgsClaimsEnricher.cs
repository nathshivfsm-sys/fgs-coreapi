using System.Security.Claims;

namespace Fgs.Security.Abstractions;

public interface IFgsClaimsEnricher
{
    Task EnrichAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
