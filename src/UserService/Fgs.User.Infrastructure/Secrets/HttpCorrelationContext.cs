using Fgs.User.Application.Abstractions.Credentials;
using Microsoft.AspNetCore.Http;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class HttpCorrelationContext(IHttpContextAccessor httpContextAccessor) : ICorrelationContext
{
    public const string HeaderName = "X-Correlation-Id";

    public Guid GetCorrelationId()
    {
        var context = httpContextAccessor.HttpContext;
        if (context is null)
        {
            return Guid.NewGuid();
        }

        if (context.Items.TryGetValue(HeaderName, out var item) && item is string s
            && Guid.TryParse(s, out var fromItem))
        {
            return fromItem;
        }

        if (Guid.TryParse(context.TraceIdentifier, out var fromTrace))
        {
            return fromTrace;
        }

        return Guid.NewGuid();
    }
}
