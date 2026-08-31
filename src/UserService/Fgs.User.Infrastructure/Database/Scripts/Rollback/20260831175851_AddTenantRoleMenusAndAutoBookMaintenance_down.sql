-- Rollback for 20260831175851_AddTenantRoleMenusAndAutoBookMaintenance
DROP TABLE IF EXISTS identity."FgsRoleMenu";
DROP TABLE IF EXISTS identity."FgsTenantMenu";
ALTER TABLE IF EXISTS tenant."FgsTenantServiceSetup" DROP COLUMN IF EXISTS "AutoBookMaintenanceScheduleCalls";
DELETE FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260831175851_AddTenantRoleMenusAndAutoBookMaintenance';
