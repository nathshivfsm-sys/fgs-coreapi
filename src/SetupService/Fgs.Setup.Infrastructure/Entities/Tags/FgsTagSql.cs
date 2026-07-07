using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.Tags;

internal static class FgsTagSql
{
    public const string Table = "setup.\"FgsTag\"";

    public const string SelectDetailColumns = """
        "Id", "TagCode", "Name", "Description", "BackgroundColor", "TextColor", "IconFileId", "UsageCount", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TagCode", "Name", "Description", "BackgroundColor", "TextColor", "IconFileId", "UsageCount", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "TagCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "TagCode", "Name", "Description", "BackgroundColor", "TextColor", "IconFileId", "UsageCount"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}