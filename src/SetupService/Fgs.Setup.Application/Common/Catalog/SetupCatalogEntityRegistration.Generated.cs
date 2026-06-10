using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Setup.Application.Common.Catalog;

public static class SetupCatalogEntityRegistration
{
    public static IServiceCollection AddSetupCatalogEntities(this IServiceCollection services)
    {
        services.AddSingleton<IEntityRegistry>(sp =>
        {
            var registry = new EntityRegistry();
            EntityRegistryRegistration.RegisterAll(registry);
            return registry;
        });
        return services;
    }
}
