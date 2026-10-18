START TRANSACTION;

DROP INDEX IF EXISTS identity."IX_FgsTenantMenu_TenantId_CompanyId_ParentMenuId";

DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_schema = 'identity'
          AND table_name = 'FgsTenantMenu'
          AND column_name = 'SortOrder'
    ) THEN
        ALTER TABLE identity."FgsTenantMenu" RENAME COLUMN "SortOrder" TO "DisplayOrder";
    END IF;

    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_schema = 'identity'
          AND table_name = 'FgsTenantMenu'
          AND column_name = 'DisplayOrder'
    ) THEN
        ALTER TABLE identity."FgsTenantMenu" ALTER COLUMN "DisplayOrder" SET DEFAULT 1;
        COMMENT ON COLUMN identity."FgsTenantMenu"."DisplayOrder" IS 'Controls the display order of the menu item for the tenant company.';
    END IF;
END $$;

DELETE FROM identity."__EFMigrationsHistory"
WHERE "MigrationId" = '20261018152605_RenameFgsTenantMenuDisplayOrderToSortOrder';

COMMIT;
