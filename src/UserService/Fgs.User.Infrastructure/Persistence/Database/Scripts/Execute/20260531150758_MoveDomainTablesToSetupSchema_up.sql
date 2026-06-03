START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'setup') THEN
            CREATE SCHEMA setup;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsVendorInventoryItem" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsVendor" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupZone" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE crm."FgsSetupTitleOfCourtesy" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupTimeSlot" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupTechTrade" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupTechSkillLevel" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupTaxDetail" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupTaxAuthority" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupTax" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupServiceAssetType" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupServiceAssetModelReference" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupServiceAssetManufacturer" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupPricingMatrixOther" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupPricingMatrixMaterialTier" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupPricingMatrixLaborTier" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupPricingMatrixLabor" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupPricingMatrix" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsSetupPostalCode" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupPaymentTerm" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupPaymentMethod" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupLaborRateType" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupGLBreakTrade" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsSetupGLBreak" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE notification."FgsSetupDescription" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE notification."FgsSetupCommunicationTemplate" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsResolutionCode" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE crm."FgsLeadSource" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsJobTypeSubCategory" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsJobTypeCategory" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsJobType" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsInventorySubCategory" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsInventoryStock" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsInventoryItemType" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsInventoryItemDependency" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsInventoryItemAlternate" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsInventoryItem" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE inventory."FgsInventoryCategory" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE integration."FgsCredentialSecret" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE integration."FgsCredentialProviderConfiguration" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE integration."FgsCredentialProvider" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE dispatch."FgsBusinessType" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE billing."FgsBillingCategory" SET SCHEMA setup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    UPDATE glo."GloSeedTableMapping"
    SET "TargetSchemaName" = 'setup'
    WHERE "TargetSchemaName" IN ('billing', 'crm', 'dispatch', 'integration', 'inventory', 'notification');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    DROP SCHEMA IF EXISTS billing;
    DROP SCHEMA IF EXISTS crm;
    DROP SCHEMA IF EXISTS dispatch;
    DROP SCHEMA IF EXISTS integration;
    DROP SCHEMA IF EXISTS inventory;
    DROP SCHEMA IF EXISTS notification;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    INSERT INTO shared."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260531150758_MoveDomainTablesToSetupSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

