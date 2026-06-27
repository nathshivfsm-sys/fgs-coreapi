using Microsoft.AspNetCore.Http;

namespace Fgs.MultiTenancy;

public interface ITenantResolver
{
    bool TryResolve(HttpContext httpContext, out TenantContext tenantContext);
}

public sealed class HeaderTenantResolver : ITenantResolver
{
    public bool TryResolve(HttpContext httpContext, out TenantContext tenantContext)
    {
        tenantContext = default!;

        if (TryGetLong(httpContext.Request.Headers["X-Tenant-Id"].FirstOrDefault(), out var headerTenantId)
            && TryGetLong(httpContext.Request.Headers["X-Company-Id"].FirstOrDefault(), out var headerCompanyId))
        {
            tenantContext = new TenantContext
            {
                TenantId = headerTenantId,
                CompanyId = headerCompanyId
            };
            return true;
        }

        return false;
    }

    private static bool TryGetLong(string? value, out long parsed) =>
        long.TryParse(value, out parsed);
}
