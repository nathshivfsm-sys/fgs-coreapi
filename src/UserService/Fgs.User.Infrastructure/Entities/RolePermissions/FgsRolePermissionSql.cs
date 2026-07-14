using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.RolePermissions;

internal static class FgsRolePermissionSql
{
    public const string Table = "identity.\"FgsRolePermission\"";

    public const string SelectColumns = """
        "Id", "FgsRoleId", "FgsPermissionId", "CreatedOn", "CreatedBy"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "FgsRoleId", "FgsPermissionId", "CreatedOn"
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
