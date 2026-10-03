START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261003171040_AddCrmCustomerWebsite') THEN
    ALTER TABLE crm."CrmCustomer" ADD "Website" character varying(500);
    COMMENT ON COLUMN crm."CrmCustomer"."Website" IS 'Customer website.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261003171040_AddCrmCustomerWebsite') THEN
    ALTER TABLE crm."CrmServiceLocation" ADD "CustomerType" bigint NULL;
    ALTER TABLE crm."CrmServiceLocation" ADD CONSTRAINT "CK_CrmServiceLocation_CustomerType" CHECK ("CustomerType" IS NULL OR "CustomerType" IN (1, 2, 3, 4, 5, 6));
    COMMENT ON COLUMN crm."CrmServiceLocation"."CustomerType" IS 'Classifies the service location customer type (Residential, Commercial, Property Management, Builder, HOA, Other).';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20261003171040_AddCrmCustomerWebsite') THEN
    INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261003171040_AddCrmCustomerWebsite', '10.0.8');
    END IF;
END $EF$;
COMMIT;
