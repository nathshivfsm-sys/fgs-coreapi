using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SalesActivityTypes;

internal static class FgsSalesActivityTypeSql
{
    public const string Table = "setup.\"FgsSalesActivityType\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "ActivityTypeCode", "ActivityTypeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "AllowManualSelection", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "ActivityTypeCode", "ActivityTypeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "AllowManualSelection", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "ActivityTypeCode", "ActivityTypeName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "DisplayOrder", "ActivityTypeCode", "ActivityTypeName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"ActivityTypeName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"ActivityTypeName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
