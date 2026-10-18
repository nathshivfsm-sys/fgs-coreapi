using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class EnsureFgsTenantMenuCatalogColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Databases that skipped 20260905055631 have FgsTenantMenu without catalog columns.
            // ALL_GloMenu provisioning copies MenuCode and the other catalog fields.
            migrationBuilder.Sql(
                """
                ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "MenuCode" character varying(50);
                ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "Name" character varying(100);
                ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "Description" character varying(255);
                ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "MenuType" character varying(20);
                ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "Route" character varying(255);
                ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "Icon" character varying(100);

                COMMENT ON COLUMN identity."FgsTenantMenu"."MenuCode" IS 'Unique system-defined code identifying the menu item (copied from global catalog).';
                COMMENT ON COLUMN identity."FgsTenantMenu"."Name" IS 'Display name of the menu item shown to users.';
                COMMENT ON COLUMN identity."FgsTenantMenu"."Description" IS 'Description of the menu item and its purpose.';
                COMMENT ON COLUMN identity."FgsTenantMenu"."MenuType" IS 'Defines the type of menu item, such as a menu group or navigable page.';
                COMMENT ON COLUMN identity."FgsTenantMenu"."Route" IS 'Application route used to navigate to the menu item when applicable.';
                COMMENT ON COLUMN identity."FgsTenantMenu"."Icon" IS 'UI icon identifier associated with the menu item.';

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

                ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "MenuCode" SET NOT NULL;
                ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "Name" SET NOT NULL;
                ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "MenuType" SET NOT NULL;

                CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsTenantMenu_TenantId_CompanyId_MenuCode"
                    ON identity."FgsTenantMenu" ("TenantId", "CompanyId", "MenuCode");
                """);

            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM pg_catalog.pg_constraint WHERE conname = 'CK_FgsTenantMenu_MenuCode_NotEmpty'
                    ) THEN
                        ALTER TABLE identity."FgsTenantMenu"
                            ADD CONSTRAINT "CK_FgsTenantMenu_MenuCode_NotEmpty" CHECK (length(trim("MenuCode")) > 0);
                    END IF;

                    IF NOT EXISTS (
                        SELECT 1 FROM pg_catalog.pg_constraint WHERE conname = 'CK_FgsTenantMenu_MenuType_NotEmpty'
                    ) THEN
                        ALTER TABLE identity."FgsTenantMenu"
                            ADD CONSTRAINT "CK_FgsTenantMenu_MenuType_NotEmpty" CHECK (length(trim("MenuType")) > 0);
                    END IF;

                    IF NOT EXISTS (
                        SELECT 1 FROM pg_catalog.pg_constraint WHERE conname = 'CK_FgsTenantMenu_Name_NotEmpty'
                    ) THEN
                        ALTER TABLE identity."FgsTenantMenu"
                            ADD CONSTRAINT "CK_FgsTenantMenu_Name_NotEmpty" CHECK (length(trim("Name")) > 0);
                    END IF;
                END $$;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE identity."FgsTenantMenu" DROP CONSTRAINT IF EXISTS "CK_FgsTenantMenu_MenuCode_NotEmpty";
                ALTER TABLE identity."FgsTenantMenu" DROP CONSTRAINT IF EXISTS "CK_FgsTenantMenu_MenuType_NotEmpty";
                ALTER TABLE identity."FgsTenantMenu" DROP CONSTRAINT IF EXISTS "CK_FgsTenantMenu_Name_NotEmpty";
                DROP INDEX IF EXISTS identity."IX_FgsTenantMenu_TenantId_CompanyId_MenuCode";
                ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Icon";
                ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Route";
                ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "MenuType";
                ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Description";
                ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Name";
                ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "MenuCode";
                """);
        }
    }
}
