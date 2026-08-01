-- Rollback for 20260801174034_AddFgsSetupTimeSlotCapacityAndExternalFlags
START TRANSACTION;

ALTER TABLE setup."FgsSetupTimeSlot" DROP COLUMN IF EXISTS "IncludeInCapacityPlanning";

ALTER TABLE setup."FgsSetupTimeSlot" DROP COLUMN IF EXISTS "ShowToExternalSystem";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260801174034_AddFgsSetupTimeSlotCapacityAndExternalFlags';

COMMIT;
