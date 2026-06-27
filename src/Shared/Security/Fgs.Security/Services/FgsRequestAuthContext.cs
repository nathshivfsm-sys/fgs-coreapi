using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Services;

public static class FgsRequestAuthContext
{
    public static string? ExtractBearerToken(HttpContext? httpContext)
    {
        var authorization = httpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization)
            || !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authorization["Bearer ".Length..].Trim();
    }

    public static (long? TenantId, long? CompanyId) ExtractTenantScope(HttpContext? httpContext)
    {
        if (httpContext is null)
        {
            return (null, null);
        }

        long? tenantId = long.TryParse(
            httpContext.Request.Headers["X-Tenant-Id"].FirstOrDefault(),
            out var parsedTenantId)
            ? parsedTenantId
            : null;

        long? companyId = long.TryParse(
            httpContext.Request.Headers["X-Company-Id"].FirstOrDefault(),
            out var parsedCompanyId)
            ? parsedCompanyId
            : null;

        return (tenantId, companyId);
    }
}
