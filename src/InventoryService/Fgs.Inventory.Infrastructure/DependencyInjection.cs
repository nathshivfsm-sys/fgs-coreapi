using Fgs.Credentials.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsInventoryInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-inventory-service", "DATABASE");
        return services;
    }
}
