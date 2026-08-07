using Fgs.Persistence.Abstractions;
using Fgs.Persistence.HealthChecks;
using Fgs.Persistence.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fgs.Persistence.Extensions;

public static class PersistenceServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="EfUnitOfWork{TDbContext}"/> for the given DbContext.
    /// When <paramref name="unitOfWorkServiceType"/> is null, maps to <see cref="IUnitOfWork"/>.
    /// Otherwise maps the custom type (e.g. ISetupUnitOfWork) to the same implementation.
    /// </summary>
    public static IServiceCollection AddFgsPersistence<TDbContext>(
        this IServiceCollection services,
        Type? unitOfWorkServiceType = null)
        where TDbContext : DbContext
    {
        services.AddScoped<EfUnitOfWork<TDbContext>>();

        if (unitOfWorkServiceType is null)
        {
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EfUnitOfWork<TDbContext>>());
        }
        else
        {
            if (!typeof(IUnitOfWork).IsAssignableFrom(unitOfWorkServiceType))
            {
                throw new ArgumentException(
                    $"{unitOfWorkServiceType.Name} must extend {nameof(IUnitOfWork)}.",
                    nameof(unitOfWorkServiceType));
            }

            services.AddScoped(unitOfWorkServiceType, sp => sp.GetRequiredService<EfUnitOfWork<TDbContext>>());
        }

        return services;
    }

    public static IServiceCollection AddFgsPersistence<TDbContext, TUnitOfWork>(this IServiceCollection services)
        where TDbContext : DbContext
        where TUnitOfWork : class, IUnitOfWork =>
        services.AddFgsPersistence<TDbContext>(typeof(TUnitOfWork));

    /// <summary>
    /// Registers a readiness health check that verifies the DbContext can connect.
    /// </summary>
    public static IServiceCollection AddFgsDbContextReadyCheck<TDbContext>(
        this IServiceCollection services,
        string name = "postgres")
        where TDbContext : DbContext
    {
        services.AddHealthChecks().Add(new HealthCheckRegistration(
            name,
            sp => new DbContextReadyHealthCheck<TDbContext>(sp.GetRequiredService<IServiceScopeFactory>()),
            failureStatus: HealthStatus.Unhealthy,
            tags: ["ready"]));
        return services;
    }
}
