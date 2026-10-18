START TRANSACTION;

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

DELETE FROM identity."__EFMigrationsHistory"
WHERE "MigrationId" = '20261018184011_EnsureFgsTenantMenuCatalogColumns';

COMMIT;
