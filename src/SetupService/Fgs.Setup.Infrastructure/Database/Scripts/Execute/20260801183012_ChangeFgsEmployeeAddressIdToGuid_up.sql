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
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260801183012_ChangeFgsEmployeeAddressIdToGuid') THEN
    -- Existing bigint values cannot map to FgsLocation (uuid). Clear then convert.
    ALTER TABLE setup."FgsEmployee"
    ALTER COLUMN "AddressId" DROP DEFAULT;

    ALTER TABLE setup."FgsEmployee"
    ALTER COLUMN "AddressId" TYPE uuid
    USING NULL;

    COMMENT ON COLUMN setup."FgsEmployee"."AddressId"
        IS 'Optional reference to the employee mailing or home address in FgsLocation. No FK by design.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260801183012_ChangeFgsEmployeeAddressIdToGuid') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260801183012_ChangeFgsEmployeeAddressIdToGuid', '10.0.8');
    END IF;
END $EF$;
COMMIT;
