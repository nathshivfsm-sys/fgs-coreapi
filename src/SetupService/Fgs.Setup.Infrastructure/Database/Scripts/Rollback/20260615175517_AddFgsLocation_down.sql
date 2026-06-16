START TRANSACTION;

DROP TABLE IF EXISTS setup."FgsLocation";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260615175517_AddFgsLocation';

COMMIT;
