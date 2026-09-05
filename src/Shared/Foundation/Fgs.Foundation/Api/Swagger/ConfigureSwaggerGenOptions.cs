using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Foundation.Api.Swagger;

internal sealed class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly FgsSwaggerOptions _swagger;

    public ConfigureSwaggerGenOptions(
        IApiVersionDescriptionProvider provider,
        IOptions<FgsSwaggerOptions> swagger)
    {
        _provider = provider;
        _swagger = swagger.Value;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
        }

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Microsoft Entra External ID access token."
        });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });

        options.OperationFilter<TenantScopeSwaggerOperationFilter>();

        if (_swagger.XmlCommentsAssembly is not null)
        {
            var xmlFile = $"{_swagger.XmlCommentsAssembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        }
    }

    private OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = _swagger.Title,
            Version = description.ApiVersion.ToString(),
            Description = BuildDescription(description)
        };

        if (!string.IsNullOrWhiteSpace(_swagger.ContactName))
        {
            info.Contact = new OpenApiContact { Name = _swagger.ContactName };
        }

        return info;
    }

    private string? BuildDescription(ApiVersionDescription description)
    {
        if (string.IsNullOrWhiteSpace(_swagger.Description))
        {
            return description.IsDeprecated ? "This API version is deprecated." : null;
        }

        return description.IsDeprecated
            ? $"{_swagger.Description} (deprecated)"
            : _swagger.Description;
    }
}
