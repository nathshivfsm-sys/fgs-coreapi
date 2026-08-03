START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260803093000_ChangeFgsEmployeeUserIdToGuid') THEN
    DROP INDEX IF EXISTS setup."UX_FgsEmployee_UserId";

    ALTER TABLE setup."FgsEmployee"
    ALTER COLUMN "UserId" DROP DEFAULT;

    ALTER TABLE setup."FgsEmployee"
    ALTER COLUMN "UserId" TYPE bigint
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
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260803093000_ChangeFgsEmployeeUserIdToGuid') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260803093000_ChangeFgsEmployeeUserIdToGuid';
    END IF;
END $EF$;
COMMIT;
