using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fgs.Foundation.Idempotency;

public static class IdempotencyServiceCollectionExtensions
{
    public static IServiceCollection AddFgsHttpIdempotency(this IServiceCollection services)
    {
        if (!services.Any(descriptor => descriptor.ServiceType == typeof(ICacheService)))
        {
            services.TryAddSingleton<ICacheService, NullCacheService>();
        }

        services.TryAddScoped<IdempotencyActionFilter>();
        services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
        {
            options.Filters.AddService<IdempotencyActionFilter>();
        });

        return services;
    }
}
