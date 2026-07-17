using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.DataAccessScopes;

internal static class FgsDataAccessScopeSql
{
    public const string Table = "identity.\"FgsDataAccessScope\"";

    public const string SelectDetailColumns = """
        "Id", "FgsDataAccessId", "ScopeType", "Operator", "ScopeValue", "DisplayOrder"
        """;

    public const string SelectSummaryColumns = """
        "Id", "FgsDataAccessId", "ScopeType", "Operator", "ScopeValue", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "FgsDataAccessId", "ScopeType", "Operator", "DisplayOrder"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir}, \"ScopeType\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
