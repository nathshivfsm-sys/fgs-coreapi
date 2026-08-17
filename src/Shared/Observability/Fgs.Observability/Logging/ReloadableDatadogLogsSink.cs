using Fgs.Observability.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Datadog.Logs;

namespace Fgs.Observability.Logging;

/// <summary>
/// Serilog sink that rebuilds the Datadog HTTP sink when ApiKey/Site change
/// (e.g. after credential snapshot reload).
/// </summary>
internal sealed class ReloadableDatadogLogsSink : ILogEventSink, IDisposable
{
    private readonly object _sync = new();
    private readonly string _serviceName;
    private readonly string _env;
    private readonly string _version;
    private readonly Func<DatadogLogShippingState> _resolveState;
    private ILogEventSink? _inner;
    private string? _fingerprint;
    private bool _disposed;

    public ReloadableDatadogLogsSink(
        string serviceName,
        string env,
        string version,
        Func<DatadogLogShippingState> resolveState)
    {
        _serviceName = serviceName;
        _env = env;
        _version = version;
        _resolveState = resolveState;
    }

    public void Emit(LogEvent logEvent)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var state = _resolveState();
        if (!state.Enabled || string.IsNullOrWhiteSpace(state.ApiKey))
        {
            return;
        }

        EnsureSink(state).Emit(logEvent);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        lock (_sync)
        {
            (_inner as IDisposable)?.Dispose();
            _inner = null;
            _fingerprint = null;
        }
    }

    private ILogEventSink EnsureSink(DatadogLogShippingState state)
    {
        var fingerprint = $"{state.ApiKey}|{state.Site}";
        lock (_sync)
        {
            if (_inner is not null
                && string.Equals(_fingerprint, fingerprint, StringComparison.Ordinal))
            {
                return _inner;
            }

            var previous = _inner;
            var intakeUrl = $"https://http-intake.logs.{state.Site}";
            _inner = DatadogSink.Create(
                apiKey: state.ApiKey!,
                source: "csharp",
                service: _serviceName,
                host: Environment.MachineName,
                tags: [$"env:{_env}", $"version:{_version}"],
                config: new DatadogConfiguration(url: intakeUrl),
                batchPeriod: TimeSpan.FromSeconds(2),
                exceptionHandler: ex =>
                    Console.Error.WriteLine(
                        $"[DatadogLogs] Failed to send logs to {intakeUrl}: {ex.Message}"));
            _fingerprint = fingerprint;

            var action = previous is null ? "enabled" : "reloaded";
            Console.WriteLine(
                $"[DatadogLogs] Sink {action} for service={_serviceName} site={state.Site} env={_env}");

            (previous as IDisposable)?.Dispose();
            return _inner;
        }
    }

    internal static DatadogLogShippingState ResolveState(
        IServiceProvider services,
        DatadogOptions startupOptions)
    {
        var options = services.GetService<IOptionsMonitor<DatadogOptions>>()?.CurrentValue
            ?? startupOptions;

        // ApiKey: prefer bound options (credential hot-reload), then process env bootstrap.
        var apiKey = FirstNonEmpty(
            options.ApiKey,
            Environment.GetEnvironmentVariable("DD_API_KEY"),
            Environment.GetEnvironmentVariable("Datadog__ApiKey"));

        // Site: prefer deployment env (pins US5/EU/etc.), then credential/options, then US1 default.
        var site = FirstNonEmpty(
            Environment.GetEnvironmentVariable("DD_SITE"),
            Environment.GetEnvironmentVariable("Datadog__Site"),
            options.Site,
            "datadoghq.com")!;

        return new DatadogLogShippingState(
            Enabled: options.Enabled,
            ApiKey: apiKey,
            Site: site.Trim().TrimEnd('/'));
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return null;
    }
}

internal readonly record struct DatadogLogShippingState(
    bool Enabled,
    string? ApiKey,
    string Site);
