using Fgs.Credentials.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsInventoryInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-inventory-service";
                options.RequiredProviders = ["DATABASE"];
            });

        services.AddFgsApiSecurity(configuration);
        return services;
    }
}
