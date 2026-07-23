using Fgs.Foundation.Correlation;
using Microsoft.AspNetCore.Http;

namespace Fgs.Bff.Infrastructure.Http;

/// <summary>
/// Propagates the inbound <c>X-Correlation-Id</c> (or TraceIdentifier) to outbound Refit calls.
/// </summary>
public sealed class CorrelationIdPropagationHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var correlationId = httpContext?.Request.Headers[CorrelationConstants.HeaderName].FirstOrDefault()
            ?? httpContext?.Items[CorrelationConstants.HeaderName]?.ToString()
            ?? httpContext?.TraceIdentifier;

        if (!string.IsNullOrWhiteSpace(correlationId)
            && !request.Headers.Contains(CorrelationConstants.HeaderName))
        {
            request.Headers.TryAddWithoutValidation(CorrelationConstants.HeaderName, correlationId);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
