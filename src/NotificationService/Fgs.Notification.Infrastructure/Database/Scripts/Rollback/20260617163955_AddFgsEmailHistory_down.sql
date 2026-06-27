START TRANSACTION;

DROP TABLE IF EXISTS notification."FgsEmailHistory";

DELETE FROM notification."__EFMigrationsHistory"
WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory';

COMMIT;
