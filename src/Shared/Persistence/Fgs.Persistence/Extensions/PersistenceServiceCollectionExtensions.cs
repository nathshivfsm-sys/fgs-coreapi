using Fgs.Persistence.Abstractions;
using Fgs.Persistence.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
}
