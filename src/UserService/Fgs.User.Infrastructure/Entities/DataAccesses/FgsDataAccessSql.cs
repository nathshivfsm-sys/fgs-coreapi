using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.DataAccesses;

internal static class FgsDataAccessSql
{
    public const string Table = "identity.\"FgsDataAccess\"";

    public const string SelectDetailColumns = """
        "Id", "DataAccessCode", "Name", "Description", "IsBuiltIn", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "DataAccessCode", "Name", "Description", "IsBuiltIn", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "DataAccessCode", "Name", "IsBuiltIn", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "DataAccessCode", "Name", "IsBuiltIn", "DisplayOrder", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir}, \"DataAccessCode\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
