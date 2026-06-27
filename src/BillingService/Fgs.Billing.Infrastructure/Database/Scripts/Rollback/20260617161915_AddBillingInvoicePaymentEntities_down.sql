START TRANSACTION;

DROP TABLE IF EXISTS billing."FgsInvoicePaymentApplication";

DROP TABLE IF EXISTS billing."FgsPaymentTransaction";

DROP TABLE IF EXISTS billing."FgsInvoiceDetail";

DROP TABLE IF EXISTS billing."FgsInvoice";

DROP TABLE IF EXISTS billing."FgsPayment";

DROP TABLE IF EXISTS billing."FgsInvoiceBatch";

DELETE FROM billing."__EFMigrationsHistory"
WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities';

COMMIT;
