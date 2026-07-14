using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.Permissions;

internal static class FgsPermissionSql
{
    public const string Table = "identity.\"FgsPermission\"";

    public const string SelectDetailColumns = """
        "Id", "PermissionCode", "Module", "Resource", "Action", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "PermissionCode", "Module", "Resource", "Action", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "PermissionCode", "Module", "Resource", "Action", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "PermissionCode", "Module", "Resource", "Action", "Name", "DisplayOrder", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir}, \"PermissionCode\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
