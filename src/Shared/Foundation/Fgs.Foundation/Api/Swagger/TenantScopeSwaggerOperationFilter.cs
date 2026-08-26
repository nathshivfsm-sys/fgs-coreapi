using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Foundation.Api.Swagger;

/// <summary>
/// Adds tenant scope headers to every Swagger operation so Try-it-out sends X-Tenant-Id and X-Company-Id.
/// </summary>
internal sealed class TenantScopeSwaggerOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        AddHeaderIfMissing(
            operation.Parameters,
            FgsApiHeaders.TenantId,
            "Tenant identifier for multi-tenant scope.",
            example: 52);

        AddHeaderIfMissing(
            operation.Parameters,
            FgsApiHeaders.CompanyId,
            "Company identifier within the tenant.",
            example: 1);
    }

    private static void AddHeaderIfMissing(
        IList<IOpenApiParameter> parameters,
        string name,
        string description,
        long example)
    {
        if (parameters.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        parameters.Add(new OpenApiParameter
        {
            Name = name,
            In = ParameterLocation.Header,
            Required = true,
            Description = description,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.Integer,
                Format = "int64"
            },
            Example = JsonValue.Create(example)
        });
    }
}
