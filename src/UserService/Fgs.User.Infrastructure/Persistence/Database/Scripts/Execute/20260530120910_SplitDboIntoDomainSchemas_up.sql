-- =============================================================================
-- Migration: 20260530120910_SplitDboIntoDomainSchemas
-- Script:   20260530120910_SplitDboIntoDomainSchemas_up.sql
-- Path:     Persistence/Database/Scripts/Execute
-- Database: PostgreSQL
--
-- Creates domain schemas and moves all tables from dbo into glo, identity, tenant,
-- crm, dispatch, billing, inventory, shared, audit, integration, and notification.
-- Idempotent (dotnet ef migrations script --idempotent).
-- Pre-step: if upgrading from dbo, ensure shared.__EFMigrationsHistory exists (see bootstrap block below).
-- =============================================================================

START TRANSACTION;

-- Bootstrap: relocate migration history from dbo to shared when upgrading existing databases.
DO $bootstrap$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'shared') THEN
        CREATE SCHEMA shared;
    END IF;

    IF EXISTS (
        SELECT 1 FROM information_schema.tables
        WHERE table_schema = 'shared' AND table_name = '__EFMigrationsHistory')
       AND NOT EXISTS (SELECT 1 FROM shared."__EFMigrationsHistory") THEN
        DROP TABLE shared."__EFMigrationsHistory";
    END IF;

    IF EXISTS (
        SELECT 1 FROM information_schema.tables
        WHERE table_schema = 'dbo' AND table_name = '__EFMigrationsHistory') THEN
        ALTER TABLE dbo."__EFMigrationsHistory" SET SCHEMA shared;
    END IF;
END $bootstrap$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'billing') THEN
            CREATE SCHEMA billing;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dispatch') THEN
            CREATE SCHEMA dispatch;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'audit') THEN
            CREATE SCHEMA audit;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'integration') THEN
            CREATE SCHEMA integration;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'shared') THEN
            CREATE SCHEMA shared;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'inventory') THEN
            CREATE SCHEMA inventory;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'identity') THEN
            CREATE SCHEMA identity;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'crm') THEN
            CREATE SCHEMA crm;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'notification') THEN
            CREATE SCHEMA notification;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'tenant') THEN
            CREATE SCHEMA tenant;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'glo') THEN
            CREATE SCHEMA glo;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE IF EXISTS dbo."__EFMigrationsHistory" SET SCHEMA shared;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloZone" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloUnitOfMeasure" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloTrade" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloTitleOfCourtesy" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloTimeCardOption" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloTag" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloStateProvince" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloSkill" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloSetupTenantStatus" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloSetupPaymentTerm" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloSetupLaborRateType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloSetupDescriptionType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloSeedTableMapping" SET SCHEMA tenant;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloSeedTableColumnMapping" SET SCHEMA tenant;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloRole" SET SCHEMA identity;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloResolutionType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloPaymentMethodType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloOutboxMessage" SET SCHEMA shared;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloMasterEntityType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloLocationType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloLeadSource" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloLanguage" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloJobTypeSubCategory" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloJobTypeCategory" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloInventorySubCategory" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloInventoryItemType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloInventoryCategory" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloCredentialProviderType" SET SCHEMA integration;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloCredentialCategory" SET SCHEMA integration;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloCountry" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloCommunicationToken" SET SCHEMA notification;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloBusinessType" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloBillingCategory" SET SCHEMA glo;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."GloAccountingIntegrationType" SET SCHEMA integration;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsVendorInventoryItem" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsVendor" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsUserRole" SET SCHEMA identity;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsUser" SET SCHEMA identity;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsTenantServiceSetup" SET SCHEMA tenant;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsTenantCompany" SET SCHEMA tenant;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsTenant" SET SCHEMA tenant;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsTagEntityType" SET SCHEMA shared;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsTag" SET SCHEMA shared;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupZone" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupTitleOfCourtesy" SET SCHEMA crm;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupTimeSlot" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupTechTrade" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupTechSkillLevel" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupTaxDetail" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupTaxAuthority" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupTax" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupServiceAssetType" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupServiceAssetModelReference" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPricingMatrixOther" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPricingMatrixMaterialTier" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPricingMatrixLaborTier" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPricingMatrixLabor" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPricingMatrix" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPostalCode" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPaymentTerm" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupLaborRateType" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupGLBreakTrade" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupDescription" SET SCHEMA notification;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" SET SCHEMA notification;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsRole" SET SCHEMA identity;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsResolutionCode" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsLocation" SET SCHEMA shared;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsLeadSource" SET SCHEMA crm;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsJobTypeSubCategory" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsJobTypeCategory" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsJobType" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInvitation" SET SCHEMA identity;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInventorySubCategory" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInventoryStock" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInventoryItemType" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInventoryItemDependency" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInventoryItemAlternate" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInventoryItem" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsInventoryCategory" SET SCHEMA inventory;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsFile" SET SCHEMA shared;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsEntityTag" SET SCHEMA shared;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsCredentialSecret" SET SCHEMA integration;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsCredentialProviderConfiguration" SET SCHEMA integration;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsCredentialProvider" SET SCHEMA integration;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsCredentialAudit" SET SCHEMA audit;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsBusinessType" SET SCHEMA dispatch;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    ALTER TABLE dbo."FgsBillingCategory" SET SCHEMA billing;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas') THEN
    INSERT INTO shared."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260530120910_SplitDboIntoDomainSchemas', '10.0.8');
    END IF;
END $EF$;
COMMIT;

