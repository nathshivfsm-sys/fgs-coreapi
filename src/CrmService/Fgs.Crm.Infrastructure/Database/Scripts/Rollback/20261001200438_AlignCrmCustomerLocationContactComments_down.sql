START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON TABLE crm."CrmServiceLocation" IS 'Physical customer locations where field service work is performed.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON TABLE crm."CrmCustomer" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON TABLE crm."CrmContact" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."UpdatedOn" IS 'Last update timestamp.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."UpdatedBy" IS 'User that last updated the record.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."TaxExempt" IS 'Indicates whether this service location is tax exempt.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."State" IS 'State or province.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."SmsAllowed" IS 'Whether SMS communication is permitted.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."ServiceLocationTypeId" IS 'Lookup to service location type.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."PostalCode" IS 'Postal or ZIP code.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."PlaceId" IS 'Google or mapping provider Place Id.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Longitude" IS 'Longitude coordinate.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."LocationSequence" IS 'Sequential location number within a customer.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Latitude" IS 'Latitude coordinate.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."IsActive" IS 'Indicates whether this service location is active.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."InvoiceSmsTemplateId" IS 'Default invoice SMS template.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."InvoiceEmailTemplateId" IS 'Default invoice email template.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."FormattedAddress" IS 'Formatted address returned by mapping provider.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."EstimateSmsTemplateId" IS 'Default estimate SMS template.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."EstimateEmailTemplateId" IS 'Default estimate email template.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."EmailAllowed" IS 'Whether email communication is permitted.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DisplayName" IS 'Display name shown to users and customers.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultPaymentMethodId" IS 'Default payment method for this location.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultOtherPricingMatrixId" IS 'Default miscellaneous pricing matrix.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultMaterialPricingMatrixId" IS 'Default material pricing matrix.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultLaborPricingMatrixId" IS 'Default labor pricing matrix.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."CreatedOn" IS 'Record creation timestamp.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."CreatedBy" IS 'User that created the record.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."County" IS 'County or district.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Country" IS 'Country.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."CompanyId" IS 'Company identifier.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."City" IS 'City.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine4" IS 'Additional address information.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine3" IS 'Additional address information.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine2" IS 'Secondary street address.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine1" IS 'Primary street address.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Id" IS 'Primary key.';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."UpdatedOn" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."UpdatedBy" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."TenantId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."TaxExemptNumber" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."TaxExempt" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."State" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."PostalCode" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."PlaceId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Name" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Longitude" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Latitude" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."LastServiceLocationSequence" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."IsActive" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."FormattedAddress" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."ExternalVersion" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."ExternalEntityId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DisplayName" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultPaymentTermId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultPORequired" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultOtherPricingMatrixId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultMaterialPricingMatrixId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultLaborPricingMatrixId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CustomerNumber" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CustomerAccountNumber" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CreatedOn" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CreatedBy" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."County" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Country" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CompanyId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."City" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine4" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine3" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine2" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine1" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Id" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."UpdatedOn" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."UpdatedBy" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."Title" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."TenantId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."ServiceLocationId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."IsDefaultContact" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."IsActive" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."DisplayOrder" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."DisplayName" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."DepartmentName" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CustomerId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CreatedOn" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CreatedBy" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CompanyId" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CanReceiveInvoices" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CanReceiveEstimates" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CanReceiveAppointments" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."Id" IS NULL;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    DELETE FROM crm."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments';
    END IF;
END $EF$;
COMMIT;

