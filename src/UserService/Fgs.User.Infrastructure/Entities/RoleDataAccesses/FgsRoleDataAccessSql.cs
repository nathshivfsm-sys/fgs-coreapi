using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.RoleDataAccesses;

internal static class FgsRoleDataAccessSql
{
    public const string Table = "identity.\"FgsRoleDataAccess\"";

    public const string SelectColumns = """
        "Id", "FgsRoleId", "FgsDataAccessId", "CreatedOn", "CreatedBy"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "FgsRoleId", "FgsDataAccessId", "CreatedOn"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"CreatedOn\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
