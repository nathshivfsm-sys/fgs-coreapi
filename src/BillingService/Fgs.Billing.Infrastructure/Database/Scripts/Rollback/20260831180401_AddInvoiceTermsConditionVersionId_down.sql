-- Rollback for 20260831180401_AddInvoiceTermsConditionVersionId
ALTER TABLE IF EXISTS billing."FgsInvoice" DROP COLUMN IF EXISTS "TermsConditionVersionId";
DELETE FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260831180401_AddInvoiceTermsConditionVersionId';
