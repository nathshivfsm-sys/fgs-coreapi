using Fgs.Foundation.CatalogCrud.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Persistence.CatalogCrud;

public sealed class CatalogCrudInfrastructureBuilder(IServiceCollection services)
{
    public IServiceCollection WithReadConnectionFactory<TConnectionFactory>()
        where TConnectionFactory : class, ICatalogReadConnectionFactory
    {
        services.AddScoped<ICatalogReadConnectionFactory, TConnectionFactory>();
        return services;
    }
}

public static class CatalogCrudServiceCollectionExtensions
{
    public static CatalogCrudInfrastructureBuilder AddCatalogCrudInfrastructure<TDbContext>(
        this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddScoped<IEntityReadRepository, CatalogEntityReadRepository>();
        services.AddScoped<IEntityWriteService, CatalogEntityWriteService<TDbContext>>();
        services.AddScoped<IEntityAuditStamper, CatalogEntityAuditStamper>();
        return new CatalogCrudInfrastructureBuilder(services);
    }
}
