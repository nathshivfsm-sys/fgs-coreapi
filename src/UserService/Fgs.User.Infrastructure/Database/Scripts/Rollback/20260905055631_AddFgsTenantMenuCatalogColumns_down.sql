-- Rollback for 20260905055631_AddFgsTenantMenuCatalogColumns
ALTER TABLE identity."FgsTenantMenu" DROP CONSTRAINT IF EXISTS "CK_FgsTenantMenu_Name_NotEmpty";
ALTER TABLE identity."FgsTenantMenu" DROP CONSTRAINT IF EXISTS "CK_FgsTenantMenu_MenuType_NotEmpty";
ALTER TABLE identity."FgsTenantMenu" DROP CONSTRAINT IF EXISTS "CK_FgsTenantMenu_MenuCode_NotEmpty";
DROP INDEX IF EXISTS identity."IX_FgsTenantMenu_TenantId_CompanyId_MenuCode";
ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Route";
ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "ParentMenuId";
ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Name";
ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "MenuType";
ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "MenuCode";
ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Icon";
ALTER TABLE identity."FgsTenantMenu" DROP COLUMN IF EXISTS "Description";
