START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20261018152605_RenameFgsTenantMenuDisplayOrderToSortOrder') THEN
        IF EXISTS (
            SELECT 1
            FROM information_schema.columns
            WHERE table_schema = 'identity'
              AND table_name = 'FgsTenantMenu'
              AND column_name = 'DisplayOrder'
        ) AND NOT EXISTS (
            SELECT 1
            FROM information_schema.columns
            WHERE table_schema = 'identity'
              AND table_name = 'FgsTenantMenu'
              AND column_name = 'SortOrder'
        ) THEN
            ALTER TABLE identity."FgsTenantMenu" RENAME COLUMN "DisplayOrder" TO "SortOrder";
        END IF;

        ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "SortOrder" SET DEFAULT 0;
        COMMENT ON COLUMN identity."FgsTenantMenu"."SortOrder" IS 'Determines the display order of the menu item within its parent menu.';

        ALTER TABLE identity."FgsTenantMenu" ADD COLUMN IF NOT EXISTS "ParentMenuId" integer;
        COMMENT ON COLUMN identity."FgsTenantMenu"."ParentMenuId" IS 'Global parent menu id when this item is nested; NULL for top-level menus.';

        CREATE INDEX IF NOT EXISTS "IX_FgsTenantMenu_TenantId_CompanyId_ParentMenuId"
            ON identity."FgsTenantMenu" ("TenantId", "CompanyId", "ParentMenuId");

        INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20261018152605_RenameFgsTenantMenuDisplayOrderToSortOrder', '10.0.8');
    END IF;
END $EF$;

COMMIT;
