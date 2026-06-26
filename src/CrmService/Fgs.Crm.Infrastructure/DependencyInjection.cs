using Fgs.Credentials.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Crm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsCrmInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-crm-service", "DATABASE");
        return services;
    }
}
