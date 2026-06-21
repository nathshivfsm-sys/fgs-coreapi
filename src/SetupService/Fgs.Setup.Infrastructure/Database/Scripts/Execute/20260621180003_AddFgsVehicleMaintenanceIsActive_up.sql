DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'setup') THEN
        CREATE SCHEMA setup;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS setup."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260621180003_AddFgsVehicleMaintenanceIsActive') THEN
    ALTER TABLE setup."FgsVehicleMaintenance" ADD "IsActive" boolean NOT NULL DEFAULT TRUE;
    COMMENT ON COLUMN setup."FgsVehicleMaintenance"."IsActive" IS 'Indicates whether the maintenance record is active and available for use.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260621180003_AddFgsVehicleMaintenanceIsActive') THEN
    CREATE INDEX "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsActive" ON setup."FgsVehicleMaintenance" ("TenantId", "CompanyId", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260621180003_AddFgsVehicleMaintenanceIsActive') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260621180003_AddFgsVehicleMaintenanceIsActive', '10.0.8');
    END IF;
END $EF$;
COMMIT;
