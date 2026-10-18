using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.User.API.Swagger;

public static class UserSwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddFgsUserSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureUserSwaggerGenOptions>();
        return services;
    }
}
