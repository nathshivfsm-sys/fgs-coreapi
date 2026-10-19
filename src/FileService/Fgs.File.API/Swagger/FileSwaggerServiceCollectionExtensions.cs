using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.File.API.Swagger;

public static class FileSwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddFgsFileSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureFileSwaggerGenOptions>();
        return services;
    }
}
