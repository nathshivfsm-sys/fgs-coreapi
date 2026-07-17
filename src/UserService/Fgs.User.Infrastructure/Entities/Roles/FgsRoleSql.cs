using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.Roles;

internal static class FgsRoleSql
{
    public const string Table = "identity.\"FgsRole\"";

    public const string SelectDetailColumns = """
        "Id", "RoleCode", "Name", "Description", "ParentRoleId", "IsBuiltIn", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "RoleCode", "Name", "Description", "ParentRoleId", "IsBuiltIn", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "RoleCode", "Name", "IsBuiltIn", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "RoleCode", "Name", "ParentRoleId", "IsBuiltIn", "DisplayOrder", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir}, \"RoleCode\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
