using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Foundation.Api.Swagger;

/// <summary>
/// Adds optional tenant scope headers in Swagger for operations that require tenant context.
/// Skips auth, signup, invite, and paths listed in <c>TenantScope:SkipPathPrefixes</c>.
/// Registered for every service via <see cref="ConfigureSwaggerGenOptions" />.
/// </summary>
internal sealed class TenantScopeSwaggerOperationFilter(IConfiguration configuration) : IOperationFilter
{
    private readonly IReadOnlyList<string> _skipPathPrefixes =
        TenantScopeSwaggerRules.ResolveSkipPathPrefixes(configuration);

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (TenantScopeSwaggerRules.ShouldSkipTenantScopeHeaders(
                context.ApiDescription.RelativePath,
                context.MethodInfo,
                _skipPathPrefixes))
        {
            return;
        }

        operation.Parameters ??= [];

        AddHeaderIfMissing(
            operation.Parameters,
            FgsApiHeaders.TenantId,
            "Tenant identifier for multi-tenant scope (required for tenant-scoped operations).",
            example: 52,
            required: false);

        AddHeaderIfMissing(
            operation.Parameters,
            FgsApiHeaders.CompanyId,
            "Company identifier within the tenant (required for company-scoped operations).",
            example: 1,
            required: false);
    }

    private static void AddHeaderIfMissing(
        IList<IOpenApiParameter> parameters,
        string name,
        string description,
        long example,
        bool required)
    {
        if (parameters.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        parameters.Add(new OpenApiParameter
        {
            Name = name,
            In = ParameterLocation.Header,
            Required = required,
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
