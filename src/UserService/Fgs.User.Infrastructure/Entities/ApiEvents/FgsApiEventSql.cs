using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.ApiEvents;

internal static class FgsApiEventSql
{
    public const string Table = "identity.\"FgsApiEvent\"";

    public const string SelectDetailColumns = """
        "Id", "EventCode", "EventCategory", "Name", "Description", "EventVersion", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "EventCode", "EventCategory", "Name", "Description", "EventVersion", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "EventCode", "EventCategory", "Name", "EventVersion", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "EventCode", "EventCategory", "Name", "EventVersion", "DisplayOrder", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir}, \"EventCode\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
