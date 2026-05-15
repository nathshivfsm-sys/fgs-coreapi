using System.Reflection;
using Microsoft.OpenApi;

namespace Fgs.User.API.Swagger;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddFgsUserSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FGS User Service",
                Version = "v1",
                Description =
                    "Multi-tenant company onboarding (signup), email invitations, Microsoft Entra External ID callback, "
                    + "transactional outbox, and platform user management.",
                Contact = new OpenApiContact
                {
                    Name = "FGS Platform",
                },
            });

            options.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });

        return services;
    }

    internal static bool IsSwaggerEnabled(this IConfiguration configuration, IWebHostEnvironment environment)
    {
        return environment.IsDevelopment()
            || configuration.GetValue("Swagger:Enabled", false);
    }
}
