using Fgs.Credentials.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Reporting.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsReportingInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-reporting-service", "DATABASE");
        return services;
    }
}
