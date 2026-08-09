using Fgs.Foundation.Correlation;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Observability.Tracing;

public sealed class DatadogSpanTagMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationContext = context.RequestServices.GetService<ICorrelationContext>();
        if (correlationContext is not null)
        {
            DatadogTracing.TagActiveSpan("correlation_id", correlationContext.GetCorrelationId().ToString("N"));
        }

        var tenantContextAccessor = context.RequestServices.GetService<ITenantContextAccessor>();
        if (tenantContextAccessor?.Current is { } tenant)
        {
            DatadogTracing.TagActiveSpan("tenant_id", tenant.TenantId.ToString());
            DatadogTracing.TagActiveSpan("company_id", tenant.CompanyId.ToString());
        }

        var userContext = context.RequestServices.GetService<IFgsUserContext>();
        if (userContext?.UserId is { } userId)
        {
            DatadogTracing.TagActiveSpan("user_id", userId.ToString());
        }

        await next(context);
    }
}
