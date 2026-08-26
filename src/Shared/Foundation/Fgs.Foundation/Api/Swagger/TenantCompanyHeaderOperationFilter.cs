using Fgs.Foundation.Api;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Foundation.Api.Swagger;

/// <summary>
/// Adds <c>X-Tenant-Id</c> and <c>X-Company-Id</c> header parameters to every operation
/// so Swagger UI "Try it out" can send multi-tenant context.
/// </summary>
internal sealed class TenantCompanyHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        AddHeaderIfMissing(
            operation,
            FgsApiHeaders.TenantId,
            "Tenant id (long). Required for tenant-scoped APIs.");

        AddHeaderIfMissing(
            operation,
            FgsApiHeaders.CompanyId,
            "Company id (long). Required for tenant-scoped APIs.");
    }

    private static void AddHeaderIfMissing(
        OpenApiOperation operation,
        string name,
        string description)
    {
        var parameters = operation.Parameters ??= [];

        if (parameters.Any(p =>
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        parameters.Add(new OpenApiParameter
        {
            Name = name,
            In = ParameterLocation.Header,
            Required = false,
            Description = description,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String
            }
        });
    }
}
