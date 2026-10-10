using Fgs.Foundation.Paging;
using Fgs.User.Infrastructure.Entities.Permissions;

namespace Fgs.User.Infrastructure.Entities.RolePermissions;

internal static class FgsRolePermissionSql
{
    public const string Table = "identity.\"FgsRolePermission\"";

    public const string SelectDetailColumns = """
        rp."Id", rp."FgsRoleId", rp."FgsPermissionId", rp."CreatedOn", rp."CreatedBy",
        p."Id" AS "PermissionId", p."PermissionCode", p."Module", p."Resource", p."Action",
        p."Name" AS "PermissionName", p."Description" AS "PermissionDescription",
        p."DisplayOrder" AS "PermissionDisplayOrder", p."IsActive" AS "PermissionIsActive"
        """;

    public static readonly string DetailFrom = $"""
        {Table} rp
        LEFT JOIN {FgsPermissionSql.Table} p
          ON p."Id" = rp."FgsPermissionId"
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
