START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds') THEN
    DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_Break1Id";
    DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_Break2Id";
    ALTER TABLE crm."CrmServiceLocation" DROP COLUMN IF EXISTS "Break1Id";
    ALTER TABLE crm."CrmServiceLocation" DROP COLUMN IF EXISTS "Break2Id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds') THEN
    DELETE FROM crm."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261012191520_AddCrmServiceLocationBreakIds';
    END IF;
END $EF$;

COMMIT;
