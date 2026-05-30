-- =============================================================================
-- Migration: 20260530120910_SplitDboIntoDomainSchemas
-- Script:   20260530120910_SplitDboIntoDomainSchemas_down.sql
-- Path:     Persistence/Database/Scripts/Rollback
-- Database: PostgreSQL
--
-- Moves all domain tables back to dbo and restores migration history location.
-- =============================================================================

START TRANSACTION;

DELETE FROM shared."__EFMigrationsHistory"
WHERE "MigrationId" = '20260530120910_SplitDboIntoDomainSchemas';

ALTER TABLE IF EXISTS billing."FgsBillingCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsBusinessType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS audit."FgsCredentialAudit" SET SCHEMA dbo;
ALTER TABLE IF EXISTS integration."FgsCredentialProvider" SET SCHEMA dbo;
ALTER TABLE IF EXISTS integration."FgsCredentialProviderConfiguration" SET SCHEMA dbo;
ALTER TABLE IF EXISTS integration."FgsCredentialSecret" SET SCHEMA dbo;
ALTER TABLE IF EXISTS shared."FgsEntityTag" SET SCHEMA dbo;
ALTER TABLE IF EXISTS shared."FgsFile" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsInventoryCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsInventoryItem" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsInventoryItemAlternate" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsInventoryItemDependency" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsInventoryItemType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsInventoryStock" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsInventorySubCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS identity."FgsInvitation" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsJobType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsJobTypeCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsJobTypeSubCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS crm."FgsLeadSource" SET SCHEMA dbo;
ALTER TABLE IF EXISTS shared."FgsLocation" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsResolutionCode" SET SCHEMA dbo;
ALTER TABLE IF EXISTS identity."FgsRole" SET SCHEMA dbo;
ALTER TABLE IF EXISTS notification."FgsSetupCommunicationTemplate" SET SCHEMA dbo;
ALTER TABLE IF EXISTS notification."FgsSetupDescription" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupGLBreak" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupGLBreakTrade" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupLaborRateType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupPaymentMethod" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupPaymentTerm" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupPostalCode" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupPricingMatrix" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupPricingMatrixLabor" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupPricingMatrixLaborTier" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupPricingMatrixMaterialTier" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupPricingMatrixOther" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupServiceAssetManufacturer" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupServiceAssetModelReference" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupServiceAssetType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupTax" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupTaxAuthority" SET SCHEMA dbo;
ALTER TABLE IF EXISTS billing."FgsSetupTaxDetail" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupTechSkillLevel" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupTechTrade" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupTimeSlot" SET SCHEMA dbo;
ALTER TABLE IF EXISTS crm."FgsSetupTitleOfCourtesy" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."FgsSetupZone" SET SCHEMA dbo;
ALTER TABLE IF EXISTS shared."FgsTag" SET SCHEMA dbo;
ALTER TABLE IF EXISTS shared."FgsTagEntityType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS tenant."FgsTenant" SET SCHEMA dbo;
ALTER TABLE IF EXISTS tenant."FgsTenantCompany" SET SCHEMA dbo;
ALTER TABLE IF EXISTS tenant."FgsTenantServiceSetup" SET SCHEMA dbo;
ALTER TABLE IF EXISTS identity."FgsUser" SET SCHEMA dbo;
ALTER TABLE IF EXISTS identity."FgsUserRole" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsVendor" SET SCHEMA dbo;
ALTER TABLE IF EXISTS inventory."FgsVendorInventoryItem" SET SCHEMA dbo;
ALTER TABLE IF EXISTS integration."GloAccountingIntegrationType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloBillingCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloBusinessType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS notification."GloCommunicationToken" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloCountry" SET SCHEMA dbo;
ALTER TABLE IF EXISTS integration."GloCredentialCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS integration."GloCredentialProviderType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloInventoryCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloInventoryItemType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloInventorySubCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloJobTypeCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloJobTypeSubCategory" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloLanguage" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloLeadSource" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloLocationType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloMasterEntityType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS shared."GloOutboxMessage" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloPaymentMethodType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloResolutionType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS identity."GloRole" SET SCHEMA dbo;
ALTER TABLE IF EXISTS tenant."GloSeedTableColumnMapping" SET SCHEMA dbo;
ALTER TABLE IF EXISTS tenant."GloSeedTableMapping" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloSetupDescriptionType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloSetupLaborRateType" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloSetupPaymentTerm" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloSetupTenantStatus" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloSkill" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloStateProvince" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloTag" SET SCHEMA dbo;
ALTER TABLE IF EXISTS dispatch."GloTimeCardOption" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloTitleOfCourtesy" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloTrade" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloUnitOfMeasure" SET SCHEMA dbo;
ALTER TABLE IF EXISTS glo."GloZone" SET SCHEMA dbo;

ALTER TABLE IF EXISTS shared."__EFMigrationsHistory" SET SCHEMA dbo;

COMMIT;