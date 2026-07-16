using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupLaborRateTypes;

internal static class FgsSetupLaborRateTypeSql
{
    public const string Table = "setup.\"FgsSetupLaborRateType\"";

    public const string SelectDetailColumns = """
        "Id", "Name", "Description", "SortOrder", "IsSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Name", "Description", "SortOrder", "IsSystem", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Name", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "Name", "Description", "IsSystem"
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