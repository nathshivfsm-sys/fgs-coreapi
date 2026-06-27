START TRANSACTION;

DROP TABLE IF EXISTS integration."FgsPaymentTransactionPayload";

DELETE FROM integration."__EFMigrationsHistory"
WHERE "MigrationId" = '20260617162140_AddFgsPaymentTransactionPayload';

COMMIT;
