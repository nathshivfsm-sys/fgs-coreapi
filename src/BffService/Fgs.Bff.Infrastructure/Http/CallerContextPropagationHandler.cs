using Microsoft.AspNetCore.Http;

namespace Fgs.Bff.Infrastructure.Http;

/// <summary>
/// Forwards Authorization, X-Tenant-Id, and X-Company-Id from the inbound request to outbound HttpClient calls.
/// </summary>
public sealed class CallerContextPropagationHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    public const string TenantIdHeader = "X-Tenant-Id";
    public const string CompanyIdHeader = "X-Company-Id";

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return base.SendAsync(request, cancellationToken);
        }

        CopyHeader(httpContext, request, "Authorization");
        CopyHeader(httpContext, request, TenantIdHeader);
        CopyHeader(httpContext, request, CompanyIdHeader);

        return base.SendAsync(request, cancellationToken);
    }

    private static void CopyHeader(HttpContext httpContext, HttpRequestMessage request, string headerName)
    {
        if (request.Headers.Contains(headerName))
        {
            return;
        }

        var value = httpContext.Request.Headers[headerName].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(value))
        {
            request.Headers.TryAddWithoutValidation(headerName, value);
        }
    }
}
