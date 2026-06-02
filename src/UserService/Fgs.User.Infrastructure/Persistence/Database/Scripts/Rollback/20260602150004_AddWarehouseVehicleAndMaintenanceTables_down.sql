-- =============================================================================
-- Migration: 20260602150004_AddWarehouseVehicleAndMaintenanceTables
-- Script:   20260602150004_AddWarehouseVehicleAndMaintenanceTables_down.sql
-- Path:     Persistence/Database/Scripts/Rollback
-- Database: PostgreSQL
-- Idempotent (dotnet ef migrations script --idempotent).
-- =============================================================================

START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260602150004_AddWarehouseVehicleAndMaintenanceTables') THEN
    DELETE FROM glo."GloMasterEntityType"
    WHERE "Code" IN ('Warehouse', 'Vehicle', 'VehicleMaintenance');
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260602150004_AddWarehouseVehicleAndMaintenanceTables') THEN
    DROP TABLE setup."FgsVehicleMaintenance";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260602150004_AddWarehouseVehicleAndMaintenanceTables') THEN
    DROP TABLE setup."FgsVehicle";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260602150004_AddWarehouseVehicleAndMaintenanceTables') THEN
    DROP TABLE glo."GloVehicleMaintenanceType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260602150004_AddWarehouseVehicleAndMaintenanceTables') THEN
    DROP TABLE setup."FgsWarehouse";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260602150004_AddWarehouseVehicleAndMaintenanceTables') THEN
    DELETE FROM shared."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260602150004_AddWarehouseVehicleAndMaintenanceTables';
    END IF;
END $EF$;
COMMIT;

