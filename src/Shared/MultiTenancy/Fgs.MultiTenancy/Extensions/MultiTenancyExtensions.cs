using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.MultiTenancy.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFgsMultiTenancy(this IServiceCollection services)
    {
        services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();
        services.AddSingleton<ITenantResolver, HeaderAndClaimTenantResolver>();
        return services;
    }
}

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFgsTenantResolution(this IApplicationBuilder app) =>
        app.UseMiddleware<TenantResolutionMiddleware>();
}
