using Fgs.Foundation.Correlation;
using Microsoft.AspNetCore.Http;

namespace Fgs.Foundation.Middleware;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationConstants.HeaderName].FirstOrDefault()
            ?? Guid.NewGuid().ToString("N");

        context.TraceIdentifier = correlationId;
        context.Response.Headers[CorrelationConstants.HeaderName] = correlationId;
        context.Items[CorrelationConstants.HeaderName] = correlationId;

        await _next(context);
    }
}
