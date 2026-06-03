using Microsoft.AspNetCore.Http;

namespace Fgs.MultiTenancy.Middleware;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(
        HttpContext context,
        ITenantResolver tenantResolver,
        ITenantContextAccessor tenantContextAccessor)
    {
        if (tenantResolver.TryResolve(context, out var tenantContext))
        {
            tenantContextAccessor.Current = tenantContext;
        }

        await _next(context);
    }
}
