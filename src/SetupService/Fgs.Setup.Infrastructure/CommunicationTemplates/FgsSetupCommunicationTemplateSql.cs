using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.CommunicationTemplates;

internal static class FgsSetupCommunicationTemplateSql
{
    public const string Table = "setup.\"FgsSetupCommunicationTemplate\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "CommunicationChannel", "TemplateType", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
