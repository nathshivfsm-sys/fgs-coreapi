using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.UserRoles;

internal static class FgsUserRoleSql
{
    public const string Table = "identity.\"FgsUserRole\"";

    public const string SelectColumns = """
        "Id", "UserId", "FgsRoleId", "CreatedOn", "CreatedBy"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "UserId", "FgsRoleId", "CreatedOn"
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
