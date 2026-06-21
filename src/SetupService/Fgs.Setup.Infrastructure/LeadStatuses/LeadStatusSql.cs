using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.LeadStatuses;

internal static class LeadStatusSql
{
    public const string Table = "setup.\"FgsLeadStatus\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "StatusCode", "StatusName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "DisplayOrder", "StatusCode", "StatusName", "Description", "IsSystem"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"StatusName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"StatusName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
