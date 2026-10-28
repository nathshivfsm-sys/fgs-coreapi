-- Rollback for 20261026194728_AddFgsARCustomerPaymentInformation
BEGIN;

DROP TABLE IF EXISTS billing."FgsARCustomerPaymentInformation";

DELETE FROM billing."__EFMigrationsHistory"
WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation';

COMMIT;
