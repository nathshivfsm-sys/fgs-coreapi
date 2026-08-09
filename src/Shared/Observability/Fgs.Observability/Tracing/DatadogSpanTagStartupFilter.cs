using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Fgs.Observability.Tracing;

internal sealed class DatadogSpanTagStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) =>
        app =>
        {
            app.UseMiddleware<DatadogSpanTagMiddleware>();
            next(app);
        };
}
