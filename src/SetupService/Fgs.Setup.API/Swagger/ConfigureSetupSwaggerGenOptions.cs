using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Setup.API.Swagger;

internal sealed class ConfigureSetupSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.DocumentFilter<SetupSwaggerDocumentFilter>();

        var applicationXml = Path.Combine(AppContext.BaseDirectory, "Fgs.Setup.Application.xml");
        if (File.Exists(applicationXml))
        {
            options.IncludeXmlComments(applicationXml, includeControllerXmlComments: true);
        }
    }
}
