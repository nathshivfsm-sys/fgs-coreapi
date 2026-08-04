START TRANSACTION;

DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_IsActive";
DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_PlaceId";
DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_PostalCode";
DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_State";
DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_City";
DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_DisplayName";
DROP INDEX IF EXISTS crm."IX_CrmServiceLocation_Name";

ALTER TABLE crm."CrmServiceLocation"
    DROP COLUMN IF EXISTS "UpdatedBy",
    DROP COLUMN IF EXISTS "UpdatedOn",
    DROP COLUMN IF EXISTS "CreatedBy",
    DROP COLUMN IF EXISTS "CreatedOn",
    DROP COLUMN IF EXISTS "IsActive",
    DROP COLUMN IF EXISTS "SmsAllowed",
    DROP COLUMN IF EXISTS "EmailAllowed",
    DROP COLUMN IF EXISTS "TaxExempt",
    DROP COLUMN IF EXISTS "EstimateSmsTemplateId",
    DROP COLUMN IF EXISTS "InvoiceSmsTemplateId",
    DROP COLUMN IF EXISTS "EstimateEmailTemplateId",
    DROP COLUMN IF EXISTS "InvoiceEmailTemplateId",
    DROP COLUMN IF EXISTS "DefaultOtherPricingMatrixId",
    DROP COLUMN IF EXISTS "DefaultLaborPricingMatrixId",
    DROP COLUMN IF EXISTS "DefaultMaterialPricingMatrixId",
    DROP COLUMN IF EXISTS "DefaultPaymentMethodId",
    DROP COLUMN IF EXISTS "PlaceId",
    DROP COLUMN IF EXISTS "Longitude",
    DROP COLUMN IF EXISTS "Latitude",
    DROP COLUMN IF EXISTS "FormattedAddress",
    DROP COLUMN IF EXISTS "PostalCode",
    DROP COLUMN IF EXISTS "Country",
    DROP COLUMN IF EXISTS "County",
    DROP COLUMN IF EXISTS "State",
    DROP COLUMN IF EXISTS "City",
    DROP COLUMN IF EXISTS "AddressLine4",
    DROP COLUMN IF EXISTS "AddressLine3",
    DROP COLUMN IF EXISTS "AddressLine2",
    DROP COLUMN IF EXISTS "AddressLine1",
    DROP COLUMN IF EXISTS "ServiceLocationTypeId",
    DROP COLUMN IF EXISTS "DisplayName",
    DROP COLUMN IF EXISTS "Name";

COMMENT ON TABLE crm."CrmServiceLocation" IS NULL;

DELETE FROM crm."__EFMigrationsHistory"
WHERE "MigrationId" = '20260804042818_ExpandCrmServiceLocation';

COMMIT;
