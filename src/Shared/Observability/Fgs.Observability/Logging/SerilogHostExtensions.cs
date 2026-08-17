using Fgs.Observability.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Fgs.Observability.Logging;

public static class SerilogHostExtensions
{
    public static ILogger CreateFgsBootstrapLogger() =>
        new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console(new RenderedCompactJsonFormatter())
            .CreateLogger();

    public static WebApplicationBuilder AddFgsSerilog(
        this WebApplicationBuilder builder,
        string serviceName,
        ObservabilityOptions observabilityOptions,
        DatadogOptions datadogOptions)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<FgsLogEnricher>();

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithProperty("ServiceName", serviceName)
                .Enrich.WithProperty("Environment", observabilityOptions.Env)
                .Enrich.WithProperty("Version", observabilityOptions.Version)
                .Enrich.With(services.GetRequiredService<FgsLogEnricher>())
                .Destructure.With<SensitiveDataDestructuringPolicy>()
                .WriteTo.Console(new RenderedCompactJsonFormatter());

            // Reloadable sink: ApiKey/Site refresh when credential snapshot updates Datadog options.
            configuration.WriteTo.Sink(
                new ReloadableDatadogLogsSink(
                    serviceName,
                    observabilityOptions.Env,
                    observabilityOptions.Version,
                    () => ReloadableDatadogLogsSink.ResolveState(services, datadogOptions)),
                restrictedToMinimumLevel: LogEventLevel.Information);
        });

        return builder;
    }
}
