using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Setup.API.Swagger;

public static class SetupSwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddFgsSetupSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSetupSwaggerGenOptions>();
        return services;
    }
}
