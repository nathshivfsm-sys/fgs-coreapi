using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Observability.Extensions;

/// <summary>
/// Backward-compatible aliases. Prefer <see cref="ObservabilityHostExtensions.AddFgsObservability(Microsoft.AspNetCore.Builder.WebApplicationBuilder, string?)"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFgsObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string? serviceName = null) =>
        ObservabilityHostExtensions.AddFgsObservability(services, configuration, serviceName);
}
