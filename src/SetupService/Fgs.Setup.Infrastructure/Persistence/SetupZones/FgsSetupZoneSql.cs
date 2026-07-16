using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupZones;

internal static class FgsSetupZoneSql
{
    public const string Table = "setup.\"FgsSetupZone\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "Description", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "Description", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "Code", "Name", "Description"
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