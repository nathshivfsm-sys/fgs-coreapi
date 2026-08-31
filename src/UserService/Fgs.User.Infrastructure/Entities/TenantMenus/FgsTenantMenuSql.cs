namespace Fgs.User.Infrastructure.Entities.TenantMenus;

internal static class FgsTenantMenuSql
{
    public const string Table = "identity.\"FgsTenantMenu\"";

    public const string SelectColumns = """
        "Id", "MenuId", "DisplayOrder", "IsActive", "CreatedOn", "CreatedBy"
        """;
}
