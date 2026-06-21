using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.TitlesOfCourtesy;

internal static class TitleOfCourtesySql
{
    public const string Table = "setup.\"FgsSetupTitleOfCourtesy\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "Code", "DisplayName", "SortOrder",
        "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "Code", "DisplayName", "SortOrder",
        "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "DisplayName", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Code", "DisplayName", "SortOrder", "CreatedOn", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"DisplayName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"{column}\" {dir} NULLS LAST, \"DisplayName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
