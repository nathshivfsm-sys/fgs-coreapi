using Fgs.Foundation.CatalogCrud.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Foundation.CatalogCrud;

public static class CatalogCrudServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogCrudApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCatalogEntityCommand<,>).Assembly));
        return services;
    }
}
