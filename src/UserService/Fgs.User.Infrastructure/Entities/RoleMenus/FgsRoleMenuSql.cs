namespace Fgs.User.Infrastructure.Entities.RoleMenus;

internal static class FgsRoleMenuSql
{
    public const string Table = "identity.\"FgsRoleMenu\"";

    public const string SelectColumns = """
        "Id", "RoleId", "MenuId", "DisplayOrder", "IsActive", "CreatedOn", "CreatedBy"
        """;
}
