using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Fgs.Foundation.Api.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Foundation.Api;

public static class ApiVersioningExtensions
{
    public static IServiceCollection AddFgsApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = FgsApiVersions.Default;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader(FgsApiHeaders.Version),
                    new QueryStringApiVersionReader("api-version"));
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    public static IServiceCollection AddFgsSwagger(
        this IServiceCollection services,
        Action<FgsSwaggerOptions> configure)
    {
        services.AddOptions<FgsSwaggerOptions>()
            .Configure(configure)
            .Configure<IConfiguration>((options, configuration) =>
            {
                var routePrefix = configuration[$"{FgsSwaggerOptions.ConfigurationSectionName}:RoutePrefix"];
                if (!string.IsNullOrWhiteSpace(routePrefix))
                {
                    options.RoutePrefix = routePrefix.Trim().Trim('/');
                }
            })
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Title),
                "Swagger title is required.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.RoutePrefix),
                "Swagger route prefix is required.");

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);
        });
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

        return services;
    }

    public static WebApplication UseFgsSwagger(this WebApplication app)
    {
        if (!app.Configuration.IsFgsSwaggerEnabled(app.Environment))
        {
            return app;
        }

        var swagger = app.Services.GetRequiredService<IOptions<FgsSwaggerOptions>>().Value;
        var versionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        var routePrefix = swagger.RoutePrefix.Trim().Trim('/');
        if (string.IsNullOrWhiteSpace(routePrefix))
        {
            routePrefix = "swagger";
        }

        app.UseSwagger(options =>
        {
            options.RouteTemplate = $"{routePrefix}/{{documentName}}/swagger.json";
        });

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = routePrefix;

            foreach (var description in versionProvider.ApiVersionDescriptions.OrderByDescending(d => d.ApiVersion))
            {
                options.SwaggerEndpoint(
                    $"{description.GroupName}/swagger.json",
                    $"{swagger.Title} {description.GroupName.ToUpperInvariant()}");
            }

            options.DocumentTitle = swagger.Title;
            options.DisplayRequestDuration();
        });

        return app;
    }

    public static bool IsFgsSwaggerEnabled(this IConfiguration configuration, IHostEnvironment environment) =>
        environment.IsDevelopment()
        || configuration.GetValue($"{FgsSwaggerOptions.ConfigurationSectionName}:Enabled", false);
}
