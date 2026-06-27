-- Rollback for 20260621180003_AddFgsVehicleMaintenanceIsActive
START TRANSACTION;

DROP INDEX IF EXISTS setup."IX_FgsVehicleMaintenance_TenantId_CompanyId_IsActive";

ALTER TABLE setup."FgsVehicleMaintenance" DROP COLUMN IF EXISTS "IsActive";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260621180003_AddFgsVehicleMaintenanceIsActive';

COMMIT;
