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
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260801174034_AddFgsSetupTimeSlotCapacityAndExternalFlags') THEN
    ALTER TABLE setup."FgsSetupTimeSlot" ADD "IncludeInCapacityPlanning" boolean NOT NULL DEFAULT FALSE;
    COMMENT ON COLUMN setup."FgsSetupTimeSlot"."IncludeInCapacityPlanning"
        IS 'Indicates whether this time slot is considered during capacity planning and scheduling calculations. When false, the time slot is excluded from capacity planning.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260801174034_AddFgsSetupTimeSlotCapacityAndExternalFlags') THEN
    ALTER TABLE setup."FgsSetupTimeSlot" ADD "ShowToExternalSystem" boolean NOT NULL DEFAULT FALSE;
    COMMENT ON COLUMN setup."FgsSetupTimeSlot"."ShowToExternalSystem"
        IS 'Indicates whether this time slot is exposed to third-party integrations and external systems. When false, the time slot remains internal to the application.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260801174034_AddFgsSetupTimeSlotCapacityAndExternalFlags') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260801174034_AddFgsSetupTimeSlotCapacityAndExternalFlags', '10.0.8');
    END IF;
END $EF$;
COMMIT;
