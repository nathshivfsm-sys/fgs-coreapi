-- Rollback for 20261031184320_MoveFgsVehicleFromSetup
START TRANSACTION;

DROP TABLE IF EXISTS inventory."FgsVehicleMaintenance";
DROP TABLE IF EXISTS inventory."FgsVehicle";

DELETE FROM inventory."__EFMigrationsHistory"
WHERE "MigrationId" = '20261031184320_MoveFgsVehicleFromSetup';

COMMIT;
