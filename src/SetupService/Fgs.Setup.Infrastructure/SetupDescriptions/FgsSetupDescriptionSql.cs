using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupDescriptions;

internal static class FgsSetupDescriptionSql
{
    public const string Table = "setup.\"FgsSetupDescription\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId", "SortOrder", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId", "SortOrder", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "DescriptionTypeCode", "Body", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "SortOrder", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"Body\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"Body\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
