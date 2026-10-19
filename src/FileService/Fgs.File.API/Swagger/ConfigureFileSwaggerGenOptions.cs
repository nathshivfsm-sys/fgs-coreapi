using System.Text.Json.Nodes;
using Fgs.File.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.File.API.Swagger;

internal sealed class ConfigureFileSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.MapType<FileEntityType>(StringEnumSchema<FileEntityType>);
        options.MapType<AttachmentCategory>(StringEnumSchema<AttachmentCategory>);
    }

    private static OpenApiSchema StringEnumSchema<TEnum>()
        where TEnum : struct, Enum =>
        new()
        {
            Type = JsonSchemaType.String,
            Enum = Enum.GetNames<TEnum>()
                .Select(name => (JsonNode)JsonValue.Create(name)!)
                .ToList()
        };
}
