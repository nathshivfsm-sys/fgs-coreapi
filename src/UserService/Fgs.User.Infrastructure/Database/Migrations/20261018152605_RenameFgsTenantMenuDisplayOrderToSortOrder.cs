using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameFgsTenantMenuDisplayOrderToSortOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                schema: "identity",
                table: "FgsTenantMenu",
                newName: "SortOrder");

            migrationBuilder.AlterColumn<short>(
                name: "SortOrder",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "Determines the display order of the menu item within its parent menu.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Controls the display order of the menu item for the tenant company.");

            // Databases that skipped 20260905055631 have DisplayOrder but not ParentMenuId.
            migrationBuilder.Sql(
                """
                ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "ParentMenuId" integer;
                COMMENT ON COLUMN identity."FgsTenantMenu"."ParentMenuId" IS 'Global parent menu id when this item is nested; NULL for top-level menus.';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantMenu_TenantId_CompanyId_ParentMenuId",
                schema: "identity",
                table: "FgsTenantMenu",
                columns: new[] { "TenantId", "CompanyId", "ParentMenuId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgsTenantMenu_TenantId_CompanyId_ParentMenuId",
                schema: "identity",
                table: "FgsTenantMenu");

            migrationBuilder.AlterColumn<short>(
                name: "SortOrder",
                schema: "identity",
                table: "FgsTenantMenu",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Controls the display order of the menu item for the tenant company.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0,
                oldComment: "Determines the display order of the menu item within its parent menu.");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                schema: "identity",
                table: "FgsTenantMenu",
                newName: "DisplayOrder");
        }
    }
}
