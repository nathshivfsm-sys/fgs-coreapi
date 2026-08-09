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
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260803093000_ChangeFgsEmployeeUserIdToGuid') THEN
    -- Existing bigint values cannot map to identity FgsUser (uuid). Clear then convert.
    DROP INDEX IF EXISTS setup."UX_FgsEmployee_UserId";

    ALTER TABLE setup."FgsEmployee"
    ALTER COLUMN "UserId" DROP DEFAULT;

    ALTER TABLE setup."FgsEmployee"
    ALTER COLUMN "UserId" TYPE uuid
    USING NULL;

    COMMENT ON COLUMN setup."FgsEmployee"."UserId"
        IS 'Optional reference to the system user account associated with this employee. One user may be linked to only one employee. References identity service; no FK by design.';

    CREATE UNIQUE INDEX "UX_FgsEmployee_UserId"
        ON setup."FgsEmployee" ("UserId")
        WHERE "UserId" IS NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260803093000_ChangeFgsEmployeeUserIdToGuid') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260803093000_ChangeFgsEmployeeUserIdToGuid', '10.0.8');
    END IF;
END $EF$;
COMMIT;
