-- =============================================================================
-- Migration: 20260518163137_Initial_Migration
-- Script:   20260518163137_Initial_Migration_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Reverts full User Service schema created by Initial_Migration.
-- Idempotent (matches: dotnet ef migrations script ... 0).
-- =============================================================================

START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsCredentialAudit";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsCredentialProviderConfiguration";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsInvitation";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsLocation";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsOutboxMessage";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsResolutionCode";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupCommunicationTemplate";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupDescription";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupGLBreakTechTrade";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPaymentMethod";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPaymentTerm";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPostalCode";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPriceSheet";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPriceSheetLabor";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPriceSheetLaborTier";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPriceSheetMaterial";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupPriceSheetOther";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupServiceAssetModelReference";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupTaxDetail";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupTechSkillLevel";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupTimeSlot";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupTitleOfCourtesy";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsTenantServiceSetup";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsUserRole";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloAccountingIntegrationType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloBillingCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloBusinessType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloCommunicationToken";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloCredentialCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloCredentialProviderType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloLanguage";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloLocationType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloSetupDescriptionType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloStateProvince";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsCredentialSecret";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloMasterEntityType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloResolutionType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupGLBreak";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupTechTrade";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloPaymentMethodType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloSetupLaborRateType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupServiceAssetType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupServiceAssetManufacturer";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupTax";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupTaxAuthority";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsSetupZone";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloTimeCardOption";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsRole";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsUser";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloCountry";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsCredentialProvider";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."GloRole";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsTenant";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DROP TABLE dbo."FgsTenantCompany";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518163137_Initial_Migration') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260518163137_Initial_Migration';
    END IF;
END $EF$;
COMMIT;

