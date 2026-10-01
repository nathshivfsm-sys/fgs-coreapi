START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON TABLE crm."CrmServiceLocation" IS 'Physical customer location where field service work is performed.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON TABLE crm."CrmCustomer" IS 'Represents a customer account that can own one or more service locations and is responsible for billing.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON TABLE crm."CrmContact" IS 'Contact associated with either a customer or a service location. A contact belongs to one owner: either a customer or a service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."UpdatedOn" IS 'Timestamp when the service location was last updated.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."UpdatedBy" IS 'User or process that last updated the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."TaxExempt" IS 'Indicates whether the service location is tax exempt.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."State" IS 'Service location state or province.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."SmsAllowed" IS 'Indicates whether SMS communication is permitted for the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."ServiceLocationTypeId" IS 'Identifier of the service location type.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."PostalCode" IS 'Service location postal or ZIP code.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."PlaceId" IS 'Place identifier returned by the address or mapping provider.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Longitude" IS 'Longitude coordinate associated with the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."LocationSequence" IS 'Sequential service location number within a customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Latitude" IS 'Latitude coordinate associated with the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."IsActive" IS 'Indicates whether the service location is active.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."InvoiceSmsTemplateId" IS 'Default SMS template used when sending invoices for the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."InvoiceEmailTemplateId" IS 'Default email template used when sending invoices for the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."FormattedAddress" IS 'Formatted service location address returned or constructed by the address or mapping provider.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."EstimateSmsTemplateId" IS 'Default SMS template used when sending estimates for the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."EstimateEmailTemplateId" IS 'Default email template used when sending estimates for the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."EmailAllowed" IS 'Indicates whether email communication is permitted for the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DisplayName" IS 'Service location name displayed to users and customers.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultPaymentMethodId" IS 'Default payment method assigned to the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultOtherPricingMatrixId" IS 'Default miscellaneous or other pricing matrix assigned to the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultMaterialPricingMatrixId" IS 'Default material pricing matrix assigned to the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultLaborPricingMatrixId" IS 'Default labor pricing matrix assigned to the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."CreatedOn" IS 'Timestamp when the service location was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."CreatedBy" IS 'User or process that created the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."County" IS 'Service location county or district.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Country" IS 'Service location country.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."CompanyId" IS 'Company identifier within the tenant.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."City" IS 'Service location city.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine4" IS 'Additional service location address information.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine3" IS 'Additional service location address information.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine2" IS 'Secondary service location address line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine1" IS 'Primary service location address line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmServiceLocation"."Id" IS 'Primary key for the service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."UpdatedOn" IS 'Timestamp when the customer was last updated.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."UpdatedBy" IS 'User or process that last updated the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."TenantId" IS 'Tenant identifier.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."TaxExemptNumber" IS 'Tax exemption certificate or reference number associated with the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."TaxExempt" IS 'Indicates whether the customer is tax exempt.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."State" IS 'Customer state or province.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."PostalCode" IS 'Customer postal or ZIP code.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."PlaceId" IS 'Place identifier returned by the address or mapping provider.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Name" IS 'Customer name.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Longitude" IS 'Longitude coordinate associated with the customer address.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Latitude" IS 'Latitude coordinate associated with the customer address.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."LastServiceLocationSequence" IS 'Last service location sequence number assigned to this customer. Used to generate the next service location sequence.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."IsActive" IS 'Indicates whether the customer is active.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."FormattedAddress" IS 'Formatted customer address returned or constructed by the address or mapping provider.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."ExternalVersion" IS 'Version or synchronization version associated with the customer in an external system.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."ExternalEntityId" IS 'Identifier of the customer in an external system.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DisplayName" IS 'Customer name displayed to users and customers.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultPaymentTermId" IS 'Default payment term assigned to the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultPORequired" IS 'Indicates whether a purchase order is required by default for transactions for this customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultOtherPricingMatrixId" IS 'Default miscellaneous or other pricing matrix assigned to the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultMaterialPricingMatrixId" IS 'Default material pricing matrix assigned to the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."DefaultLaborPricingMatrixId" IS 'Default labor pricing matrix assigned to the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CustomerNumber" IS 'Unique business identifier assigned to the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CustomerAccountNumber" IS 'Customer account number maintained by the customer or an external accounting system.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CreatedOn" IS 'Timestamp when the customer was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CreatedBy" IS 'User or process that created the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."County" IS 'Customer county or district.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Country" IS 'Customer country.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."CompanyId" IS 'Company identifier within the tenant.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."City" IS 'Customer city.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine4" IS 'Additional customer address information.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine3" IS 'Additional customer address information.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine2" IS 'Secondary customer address line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."AddressLine1" IS 'Primary customer address line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmCustomer"."Id" IS 'Primary key for the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."UpdatedOn" IS 'Timestamp when the contact was last updated.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."UpdatedBy" IS 'User or process that last updated the contact.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."Title" IS 'Job title or position of the contact.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."TenantId" IS 'Tenant identifier.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."ServiceLocationId" IS 'Service location associated with the contact. NULL when the contact belongs to a customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."IsDefaultContact" IS 'Indicates whether the contact is the default contact for the associated customer or service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."IsActive" IS 'Indicates whether the contact is active.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."DisplayOrder" IS 'Order in which the contact is displayed relative to other contacts for the same owner.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."DisplayName" IS 'Contact name displayed to users and customers.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."DepartmentName" IS 'Department or organizational area of the contact.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CustomerId" IS 'Customer associated with the contact. NULL when the contact belongs to a service location.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CreatedOn" IS 'Timestamp when the contact was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CreatedBy" IS 'User or process that created the contact.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CompanyId" IS 'Company identifier within the tenant.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CanReceiveInvoices" IS 'Indicates whether the contact is permitted to receive invoices.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CanReceiveEstimates" IS 'Indicates whether the contact is permitted to receive estimates.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."CanReceiveAppointments" IS 'Indicates whether the contact is permitted to receive appointment notifications.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    COMMENT ON COLUMN crm."CrmContact"."Id" IS 'Primary key for the contact.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200438_AlignCrmCustomerLocationContactComments') THEN
    INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261001200438_AlignCrmCustomerLocationContactComments', '10.0.8');
    END IF;
END $EF$;
COMMIT;

