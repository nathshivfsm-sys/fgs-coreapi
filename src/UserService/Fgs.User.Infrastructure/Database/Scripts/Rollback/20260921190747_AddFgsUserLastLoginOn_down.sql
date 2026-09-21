START TRANSACTION;

ALTER TABLE identity."FgsUser"
    DROP COLUMN IF EXISTS "LastLoginOn";

DELETE FROM identity."__EFMigrationsHistory"
WHERE "MigrationId" = '20260921190747_AddFgsUserLastLoginOn';

COMMIT;
