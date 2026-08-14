using Fgs.Observability.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Datadog.Logs;

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

            if (datadogOptions.Enabled
                && !string.IsNullOrWhiteSpace(datadogOptions.ApiKey))
            {
                var config = new DatadogConfiguration(
                    url: $"https://http-intake.logs.{datadogOptions.Site}");

                configuration.WriteTo.DatadogLogs(
                    apiKey: datadogOptions.ApiKey,
                    source: "csharp",
                    service: serviceName,
                    host: Environment.MachineName,
                    tags: [$"env:{observabilityOptions.Env}", $"version:{observabilityOptions.Version}"],
                    configuration: config,
                    logLevel: LogEventLevel.Information);
            }
        });

        return builder;
    }
}
