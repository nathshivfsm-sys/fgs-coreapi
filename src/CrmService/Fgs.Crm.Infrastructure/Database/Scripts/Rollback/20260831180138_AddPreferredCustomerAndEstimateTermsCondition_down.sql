-- Rollback for 20260831180138_AddPreferredCustomerAndEstimateTermsCondition
ALTER TABLE IF EXISTS crm."FgsEstimate" DROP COLUMN IF EXISTS "TermsConditionVersionId";
ALTER TABLE IF EXISTS crm."CrmCustomer" DROP COLUMN IF EXISTS "IsPreferredCustomer";
DELETE FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260831180138_AddPreferredCustomerAndEstimateTermsCondition';
