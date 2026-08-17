using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Fgs.Observability.Tracing;

internal sealed class ActivitySpanTagStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) =>
        app =>
        {
            app.UseMiddleware<ActivitySpanTagMiddleware>();
            next(app);
        };
}
