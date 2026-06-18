using Fgs.Contracts.Clients;
using Fgs.Security.Abstractions;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Services;

public sealed class RemoteFgsUserStatusValidator(
    IFgsClaimsClient claimsClient,
    IHttpContextAccessor httpContextAccessor) : IFgsUserStatusValidator
{
    public async Task<bool> IsActiveAsync(CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var accessToken = FgsRequestAuthContext.ExtractBearerToken(httpContext);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return false;
        }

        var (tenantId, companyId) = FgsRequestAuthContext.ExtractTenantScope(httpContext);

        try
        {
            var response = await claimsClient.ValidateUserAsync(
                $"Bearer {accessToken}",
                tenantId,
                companyId,
                cancellationToken);

            return response.Success;
        }
        catch
        {
            return false;
        }
    }
}
