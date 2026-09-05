using System.Diagnostics;
using Fgs.Foundation.Correlation;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Observability.Tracing;

/// <summary>
/// Tags the current <see cref="Activity"/> with correlation, tenant, and user context.
/// </summary>
public sealed class ActivitySpanTagMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var activity = Activity.Current;
        if (activity is not null)
        {
            var correlationContext = context.RequestServices.GetService<ICorrelationContext>();
            if (correlationContext is not null)
            {
                activity.SetTag("correlation_id", correlationContext.GetCorrelationId().ToString("N"));
            }

            var tenantContextAccessor = context.RequestServices.GetService<ITenantContextAccessor>();
            if (tenantContextAccessor?.Current is { } tenant)
            {
                activity.SetTag("tenant_id", tenant.TenantId.ToString());
                activity.SetTag("company_id", tenant.CompanyId.ToString());
            }

            var userContext = context.RequestServices.GetService<IFgsUserContext>();
            if (userContext?.UserId is { } userId)
            {
                activity.SetTag("user_id", userId.ToString());
            }
        }

        await next(context);
    }
}
