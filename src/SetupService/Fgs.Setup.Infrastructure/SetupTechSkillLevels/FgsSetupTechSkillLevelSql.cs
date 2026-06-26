using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupTechSkillLevels;

internal static class FgsSetupTechSkillLevelSql
{
    public const string Table = "setup.\"FgsSetupTechSkillLevel\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "Description", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "Description", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "Code", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
