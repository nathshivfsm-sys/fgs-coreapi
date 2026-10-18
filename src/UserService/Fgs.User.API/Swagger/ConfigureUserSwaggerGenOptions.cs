using System.Text.Json.Nodes;
using Fgs.User.Application.Features.TenantMenus;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.User.API.Swagger;

internal sealed class ConfigureUserSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.MapType<FgsTenantMenuType>(() => new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Enum =
            [
                JsonValue.Create(FgsTenantMenuTypeConverter.TopMenu)!,
                JsonValue.Create(FgsTenantMenuTypeConverter.LeftMenu)!
            ]
        });
    }
}
