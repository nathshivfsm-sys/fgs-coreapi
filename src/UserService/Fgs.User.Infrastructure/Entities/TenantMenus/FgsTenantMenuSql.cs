namespace Fgs.User.Infrastructure.Entities.TenantMenus;

internal static class FgsTenantMenuSql
{
    public const string Table = "identity.\"FgsTenantMenu\"";

    public const string SelectColumns = """
        "Id", "MenuId", "MenuCode", "Name", "Description", "ParentMenuId", "MenuType", "Route", "Icon",
        "DisplayOrder", "IsActive", "CreatedOn", "CreatedBy"
        """;
}
