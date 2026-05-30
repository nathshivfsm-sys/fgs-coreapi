using Microsoft.AspNetCore.Http;

namespace Fgs.Foundation.Correlation;

public sealed class HttpCorrelationContext(IHttpContextAccessor httpContextAccessor) : ICorrelationContext
{
    public Guid GetCorrelationId()
    {
        var context = httpContextAccessor.HttpContext;
        if (context is null)
        {
            return Guid.NewGuid();
        }

        if (context.Items.TryGetValue(CorrelationConstants.HeaderName, out var item) && item is string s
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
