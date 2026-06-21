using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.LeadSources;

internal static class LeadSourceSql
{
    public const string Table = "setup.\"FgsLeadSource\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "SourceCode", "SourceName", "Description", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "SourceCode", "SourceName", "Description", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "SourceCode", "SourceName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "SourceCode", "SourceName", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"SourceName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"SourceName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
