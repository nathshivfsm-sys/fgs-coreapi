START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'billing') THEN
            CREATE SCHEMA billing;
        END IF;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dispatch') THEN
            CREATE SCHEMA dispatch;
        END IF;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'integration') THEN
            CREATE SCHEMA integration;
        END IF;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'inventory') THEN
            CREATE SCHEMA inventory;
        END IF;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'crm') THEN
            CREATE SCHEMA crm;
        END IF;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'notification') THEN
            CREATE SCHEMA notification;
        END IF;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsVendorInventoryItem" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsVendor" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupZone" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupTitleOfCourtesy" SET SCHEMA crm;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupTimeSlot" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupTechTrade" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupTechSkillLevel" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupTaxDetail" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupTaxAuthority" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupTax" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupServiceAssetType" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupServiceAssetModelReference" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupServiceAssetManufacturer" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixOther" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixMaterialTier" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixLaborTier" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixLabor" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrix" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPostalCode" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPaymentTerm" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupPaymentMethod" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupLaborRateType" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupGLBreakTrade" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupGLBreak" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupDescription" SET SCHEMA notification;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsSetupCommunicationTemplate" SET SCHEMA notification;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsResolutionCode" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsLeadSource" SET SCHEMA crm;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsJobTypeSubCategory" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsJobTypeCategory" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsJobType" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsInventorySubCategory" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsInventoryStock" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsInventoryItemType" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsInventoryItemDependency" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsInventoryItemAlternate" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsInventoryItem" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsInventoryCategory" SET SCHEMA inventory;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsCredentialSecret" SET SCHEMA integration;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsCredentialProviderConfiguration" SET SCHEMA integration;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsCredentialProvider" SET SCHEMA integration;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsBusinessType" SET SCHEMA dispatch;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    ALTER TABLE setup."FgsBillingCategory" SET SCHEMA billing;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    UPDATE glo."GloSeedTableMapping"
    SET "TargetSchemaName" = CASE "TargetTableName"
        WHEN 'FgsBillingCategory' THEN 'billing'
        WHEN 'FgsSetupPaymentMethod' THEN 'billing'
        WHEN 'FgsSetupLaborRateType' THEN 'billing'
        WHEN 'FgsSetupPaymentTerm' THEN 'billing'
        WHEN 'FgsLeadSource' THEN 'crm'
        WHEN 'FgsSetupTitleOfCourtesy' THEN 'crm'
        WHEN 'FgsJobTypeCategory' THEN 'dispatch'
        WHEN 'FgsJobTypeSubCategory' THEN 'dispatch'
        WHEN 'FgsResolutionCode' THEN 'dispatch'
        WHEN 'FgsSetupTechSkillLevel' THEN 'dispatch'
        WHEN 'FgsSetupTechTrade' THEN 'dispatch'
        WHEN 'FgsSetupZone' THEN 'dispatch'
        ELSE "TargetSchemaName"
    END
    WHERE "TargetSchemaName" = 'setup';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema') THEN
    DELETE FROM shared."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260531150758_MoveDomainTablesToSetupSchema';
    END IF;
END $EF$;
COMMIT;

