-- Rollback for 20261003171040_AddCrmCustomerWebsite
ALTER TABLE IF EXISTS crm."CrmServiceLocation" DROP CONSTRAINT IF EXISTS "CK_CrmServiceLocation_CustomerType";
ALTER TABLE IF EXISTS crm."CrmServiceLocation" DROP COLUMN IF EXISTS "CustomerType";
ALTER TABLE IF EXISTS crm."CrmCustomer" DROP COLUMN IF EXISTS "Website";
DELETE FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261003171040_AddCrmCustomerWebsite';
