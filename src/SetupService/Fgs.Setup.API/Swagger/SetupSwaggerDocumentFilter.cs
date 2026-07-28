using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Setup.API.Swagger;

/// <summary>
/// Adds grouped tag descriptions for setup catalog endpoints in Swagger UI.
/// </summary>
public sealed class SetupSwaggerDocumentFilter : IDocumentFilter
{
    private static readonly Dictionary<string, string> TagDescriptions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Setup - Billing"] = "Tenant billing category and payment configuration.",
        ["Setup - Business"] = "Business type and operational setup catalogs.",
        ["Setup - Tax"] = "Tax codes, authorities, details, and postal code mappings.",
        ["Setup - Zone"] = "Service zones and appointment time slots.",
        ["Setup - Technician"] = "Technician trades, skill levels, titles, and descriptions.",
        ["Setup - Pricing"] = "Pricing matrices, labor tiers, and material adjustments.",
        ["Setup - GL"] = "General ledger break configuration.",
        ["Setup - ServiceAssets"] = "Service asset types, manufacturers, and model references.",
        ["Setup - Communication"] = "Communication templates and resolution codes.",
        ["Setup - JobTypes"] = "Job categories, job types, job type category mappings, and tasks.",
        ["Setup - Vehicles"] = "Fleet vehicles and maintenance records.",
        ["Setup - Tags"] = "Tenant tag definitions."
    };

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Tags is null)
        {
            return;
        }

        foreach (var tag in swaggerDoc.Tags)
        {
            if (tag.Name is not null && TagDescriptions.TryGetValue(tag.Name, out var description))
            {
                tag.Description = description;
            }
        }
    }
}
