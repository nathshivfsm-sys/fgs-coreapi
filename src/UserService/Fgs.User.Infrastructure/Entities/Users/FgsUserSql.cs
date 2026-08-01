using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.Users;

internal static class FgsUserSql
{
    public const string UserTable = "identity.\"FgsUser\"";
    public const string UserRoleTable = "identity.\"FgsUserRole\"";
    public const string RoleTable = "identity.\"FgsRole\"";
    public const string InvitationTable = "identity.\"FgsInvitation\"";

    public const string SelectDetailColumns = """
        u."Id", u."DisplayName", u."Email", u."IsActive",
        ur."FgsRoleId" AS "RoleId", r."Name" AS "RoleName",
        inv."Status" AS "InvitationStatus",
        CASE WHEN EXISTS (
            SELECT 1 FROM identity."FgsInvitation" ai
            WHERE ai."UserId" = u."Id" AND ai."Status" = 'Accepted'
        ) THEN TRUE ELSE FALSE END AS "HasAcceptedInvitation"
        """;

    public const string SelectSummaryColumns = """
        u."Id", u."DisplayName", u."Email", u."IsActive",
        ur."FgsRoleId" AS "RoleId", r."Name" AS "RoleName",
        inv."Status" AS "InvitationStatus"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "DisplayName", "Email", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY u.\"DisplayName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY u.\"{column}\" {dir}";
    }
}
