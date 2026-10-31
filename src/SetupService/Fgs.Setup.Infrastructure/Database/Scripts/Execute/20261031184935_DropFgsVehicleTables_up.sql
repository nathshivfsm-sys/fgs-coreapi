START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031184935_DropFgsVehicleTables') THEN
    DROP TABLE IF EXISTS setup."FgsVehicleMaintenance";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031184935_DropFgsVehicleTables') THEN
    DROP TABLE IF EXISTS setup."FgsVehicle";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031184935_DropFgsVehicleTables') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261031184935_DropFgsVehicleTables', '10.0.8');
    END IF;
END $EF$;
COMMIT;
