using System.Reflection;
using Microsoft.OpenApi;

namespace Fgs.Platform.API.Swagger;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddFgsPlatformSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FGS Platform Service",
                Version = "v1",
                Description =
                    "Shared platform capabilities: notifications (email/SMS/push), integrations, audit, background jobs, and reporting foundations."
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });

        return services;
    }

    internal static bool IsSwaggerEnabled(this IConfiguration configuration, IWebHostEnvironment environment) =>
        environment.IsDevelopment() || configuration.GetValue("Swagger:Enabled", false);
}
