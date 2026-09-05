using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsTenantMenuCatalogColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "Description of the menu item and its purpose.");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "UI icon identifier associated with the menu item.");

            migrationBuilder.AddColumn<string>(
                name: "MenuCode",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Unique system-defined code identifying the menu item (copied from global catalog).");

            migrationBuilder.AddColumn<string>(
                name: "MenuType",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Defines the type of menu item, such as a menu group or navigable page.");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Display name of the menu item shown to users.");

            migrationBuilder.AddColumn<int>(
                name: "ParentMenuId",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "integer",
                nullable: true,
                comment: "Global parent menu id when this item is nested; NULL for top-level menus.");

            migrationBuilder.AddColumn<string>(
                name: "Route",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "Application route used to navigate to the menu item when applicable.");

            // Backfill any pre-existing rows before NOT NULL / unique / check constraints.
            migrationBuilder.Sql(
                """
                UPDATE identity."FgsTenantMenu"
                SET
                    "MenuCode" = COALESCE(NULLIF(trim("MenuCode"), ''), 'MENU_' || "MenuId"::text),
                    "Name" = COALESCE(NULLIF(trim("Name"), ''), 'Menu ' || "MenuId"::text),
                    "MenuType" = COALESCE(NULLIF(trim("MenuType"), ''), 'PAGE')
                WHERE "MenuCode" IS NULL
                   OR trim("MenuCode") = ''
                   OR "Name" IS NULL
                   OR trim("Name") = ''
                   OR "MenuType" IS NULL
                   OR trim("MenuType") = '';
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "MenuCode" SET NOT NULL;
                ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "Name" SET NOT NULL;
                ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "MenuType" SET NOT NULL;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantMenu_TenantId_CompanyId_MenuCode",
                schema: "identity",
                table: "FgsTenantMenu",
                columns: new[] { "TenantId", "CompanyId", "MenuCode" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsTenantMenu_MenuCode_NotEmpty",
                schema: "identity",
                table: "FgsTenantMenu",
                sql: "length(trim(\"MenuCode\")) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsTenantMenu_MenuType_NotEmpty",
                schema: "identity",
                table: "FgsTenantMenu",
                sql: "length(trim(\"MenuType\")) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsTenantMenu_Name_NotEmpty",
                schema: "identity",
                table: "FgsTenantMenu",
                sql: "length(trim(\"Name\")) > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgsTenantMenu_TenantId_CompanyId_MenuCode",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsTenantMenu_MenuCode_NotEmpty",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsTenantMenu_MenuType_NotEmpty",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsTenantMenu_Name_NotEmpty",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropColumn(
                name: "MenuCode",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropColumn(
                name: "MenuType",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropColumn(
                name: "ParentMenuId",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.DropColumn(
                name: "Route",
                schema: "identity",
                table: "FgsTenantMenu");
        }
    }
}
