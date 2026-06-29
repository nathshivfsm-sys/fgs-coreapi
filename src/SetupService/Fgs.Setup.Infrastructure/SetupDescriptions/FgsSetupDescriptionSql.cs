using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.SetupDescriptions;

internal static class FgsSetupDescriptionSql
{
    public const string Table = "setup.\"FgsSetupDescription\"";

    public const string SelectDetailColumns = """
        "Id", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "DescriptionTypeCode", "Body", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"Body\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}