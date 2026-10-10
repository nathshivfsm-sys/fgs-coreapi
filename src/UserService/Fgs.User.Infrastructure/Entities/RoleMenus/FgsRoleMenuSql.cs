using Fgs.User.Infrastructure.Entities.TenantMenus;

namespace Fgs.User.Infrastructure.Entities.RoleMenus;

internal static class FgsRoleMenuSql
{
    public const string Table = "identity.\"FgsRoleMenu\"";

    public const string SelectDetailColumns = """
        rm."Id", rm."RoleId", rm."MenuId", rm."DisplayOrder", rm."IsActive", rm."CreatedOn", rm."CreatedBy",
        tm."Id" AS "TenantMenuId", tm."MenuCode", tm."Name" AS "MenuName", tm."Description" AS "MenuDescription",
        tm."ParentMenuId", tm."MenuType", tm."Route", tm."Icon", tm."DisplayOrder" AS "MenuDisplayOrder",
        tm."IsActive" AS "MenuIsActive", tm."CreatedOn" AS "MenuCreatedOn", tm."CreatedBy" AS "MenuCreatedBy"
        """;

    public static readonly string DetailFrom = $"""
        {Table} rm
        LEFT JOIN {FgsTenantMenuSql.Table} tm
          ON tm."MenuId" = rm."MenuId"
         AND tm."TenantId" = rm."TenantId"
         AND tm."CompanyId" = rm."CompanyId"
        """;
}
