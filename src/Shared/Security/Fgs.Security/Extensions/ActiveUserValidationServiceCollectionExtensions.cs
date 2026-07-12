using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Hosting;
using Fgs.Security.Authorization;
using Fgs.Security.Middleware;
using Fgs.Security.UserAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Fgs.Security.Extensions;

public sealed class ConfigureActiveUserValidationHostOptions : IConfigureOptions<FgsApiHostOptions>
{
    public void Configure(FgsApiHostOptions options)
    {
        options.PostAuthenticationMiddleware = app =>
            app.UseMiddleware<ActiveUserAuthorizationMiddleware>();
    }
}

public static class ActiveUserValidationServiceCollectionExtensions
{
    public static IServiceCollection AddFgsActiveUserValidation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (!services.Any(descriptor => descriptor.ServiceType == typeof(ICacheService)))
        {
            services.TryAddSingleton<ICacheService, NullCacheService>();
        }

        services.Configure<UserAuthCacheOptions>(configuration.GetSection(UserAuthCacheOptions.SectionName));
        services.Configure<TenantScopeOptions>(configuration.GetSection(TenantScopeOptions.SectionName));
        services.Configure<InternalServiceKeyOptions>(
            configuration.GetSection(InternalServiceKeyOptions.SectionName));
        services.TryAddScoped<IUserAuthProfileStore, UserAuthProfileStore>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IConfigureOptions<FgsApiHostOptions>, ConfigureActiveUserValidationHostOptions>());
        return services;
    }

    public static IApplicationBuilder UseFgsActiveUserValidation(this IApplicationBuilder app) =>
        app.UseMiddleware<ActiveUserAuthorizationMiddleware>();
}
