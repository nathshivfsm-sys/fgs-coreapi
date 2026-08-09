using Datadog.Trace;
using Fgs.Foundation.Correlation;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Fgs.Observability.Logging;

public sealed class FgsLogEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            EnrichTrace(logEvent, propertyFactory);
            return;
        }

        Add(logEvent, propertyFactory, "RequestPath", httpContext.Request.Path.Value);

        var correlationId = ResolveCorrelationId(httpContext);
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            Add(logEvent, propertyFactory, "CorrelationId", correlationId);
        }

        var services = httpContext.RequestServices;

        var tenant = services.GetService(typeof(ITenantContextAccessor)) as ITenantContextAccessor;
        if (tenant?.Current is { } current)
        {
            Add(logEvent, propertyFactory, "TenantId", current.TenantId);
            Add(logEvent, propertyFactory, "CompanyId", current.CompanyId);
        }

        var user = services.GetService(typeof(IFgsUserContext)) as IFgsUserContext;
        if (user?.UserId is { } userId)
        {
            Add(logEvent, propertyFactory, "UserId", userId);
        }

        EnrichTrace(logEvent, propertyFactory);
    }

    private static void EnrichTrace(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var span = Tracer.Instance?.ActiveScope?.Span;
        if (span is null)
        {
            return;
        }

        Add(logEvent, propertyFactory, "TraceId", span.TraceId.ToString());
        Add(logEvent, propertyFactory, "SpanId", span.SpanId.ToString());
    }

    private static string? ResolveCorrelationId(HttpContext httpContext)
    {
        if (httpContext.RequestServices.GetService(typeof(ICorrelationContext)) is ICorrelationContext correlation)
        {
            return correlation.GetCorrelationId().ToString("N");
        }

        return httpContext.Request.Headers[CorrelationConstants.HeaderName].FirstOrDefault()
            ?? httpContext.Items[CorrelationConstants.HeaderName]?.ToString()
            ?? httpContext.TraceIdentifier;
    }

    private static void Add(LogEvent logEvent, ILogEventPropertyFactory factory, string name, object? value)
    {
        if (value is null)
        {
            return;
        }

        logEvent.AddPropertyIfAbsent(factory.CreateProperty(name, value));
    }
}
