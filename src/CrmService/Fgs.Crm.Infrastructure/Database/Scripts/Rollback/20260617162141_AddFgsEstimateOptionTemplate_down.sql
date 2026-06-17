START TRANSACTION;

DROP TABLE IF EXISTS crm."FgsEstimateOptionTemplate";

DELETE FROM crm."__EFMigrationsHistory"
WHERE "MigrationId" = '20260617162141_AddFgsEstimateOptionTemplate';

COMMIT;
