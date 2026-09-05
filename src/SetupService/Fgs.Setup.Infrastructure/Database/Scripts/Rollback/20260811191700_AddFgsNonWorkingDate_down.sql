-- Rollback for 20260811191700_AddFgsNonWorkingDate

START TRANSACTION;

DROP TABLE IF EXISTS setup."FgsNonWorkingDate";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260811191700_AddFgsNonWorkingDate';

COMMIT;
