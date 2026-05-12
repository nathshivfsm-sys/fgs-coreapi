-- =============================================================================
-- Migration: 20260512144449_SchemaCleanupAlignment
-- Script:   20260512144449_SchemaCleanupAlignment_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Applies SchemaCleanupAlignment DDL only when MigrationId is not yet recorded.
--   2. Inserts MigrationId '20260512144449_SchemaCleanupAlignment' and ProductVersion
--      into "__EFMigrationsHistory" after successful DDL (see end of script).
--
-- Notes:
--   - CompanyId columns on tenant-scoped tables are converted from bigint (legacy
--     FgsTenantCompany.Id) to uuid using FgsTenantCompany.CompanyGuid (see embedded
--     ALTER ... USING statements generated from the EF migration).
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    DROP TABLE dbo."FgsSetupCommunicationToken";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    DROP TABLE dbo."FgsTenantCompanyConfiguration";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    DROP INDEX dbo."IX_FgsCredentialProvider_TenantId_Code";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" DROP COLUMN "PaymentMethodType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "LogoLocationId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP COLUMN "GloMasterEntityTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "KmsKeyArn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "RegionName";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "Remarks";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "SecretArn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "VaultProvider";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "Description";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD "LogoUrl" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsLocation" RENAME COLUMN "EntityTypeId" TO "MasterEntityTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER INDEX dbo."IX_FgsLocation_EntityTypeId" RENAME TO "IX_FgsLocation_MasterEntityTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "RotationEnabled";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD "IsRevoked" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" RENAME COLUMN "RotatedOn" TO "LastRotatedOn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD "ExpiresOn" timestamptz;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "LastValidatedOn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN

    -- PostgreSQL does not allow subqueries in ALTER COLUMN ... USING.
    -- Convert legacy bigint CompanyId to uuid (FgsTenantCompany.CompanyGuid).

    DROP INDEX IF EXISTS dbo."IX_FgsSetupZone_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupTimeSlot_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupTechTrade_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupTechSkillLevel_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupTaxDetail_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupTaxAuthority_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupTax_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetType_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetModelSerialDescription_TenantId_Company~";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetMedia_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetOther_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetMaterialRange_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetLaborTier_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetLabor_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheet_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPostalCode_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPaymentTerm_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPaymentMethod_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupGLBreak_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupDescription_TenantId_CompanyId";
    DROP INDEX IF EXISTS dbo."IX_FgsSetupCommunicationTemplate_TenantId_CompanyId";

    ALTER TABLE dbo."FgsSetupZone" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupZone" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupZone" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupZone" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupZone" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupTitleOfCourtesy" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupTitleOfCourtesy" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupTitleOfCourtesy" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupTitleOfCourtesy" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupTitleOfCourtesy" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupTimeSlot" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupTimeSlot" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupTimeSlot" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupTimeSlot" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupTimeSlot" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupTechTrade" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupTechTrade" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupTechTrade" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupTechTrade" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupTechTrade" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupTechSkillLevel" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupTechSkillLevel" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupTechSkillLevel" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupTechSkillLevel" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupTechSkillLevel" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupTaxDetail" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupTaxDetail" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupTaxDetail" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupTaxDetail" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupTaxDetail" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupTaxAuthority" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupTaxAuthority" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupTaxAuthority" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupTaxAuthority" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupTaxAuthority" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupTax" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupTax" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupTax" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupTax" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupTax" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupServiceAssetType" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupServiceAssetType" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetType" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetType" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetType" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupServiceAssetModelSerialDescription" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupServiceAssetMedia" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupServiceAssetMedia" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetMedia" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetMedia" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetMedia" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupServiceAssetManufacturer" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPriceSheetOther" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPriceSheetOther" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetOther" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetOther" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetOther" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPriceSheetMaterialRange" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPriceSheetMaterial" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPriceSheetMaterial" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetMaterial" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetMaterial" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetMaterial" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPriceSheetLaborTier" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPriceSheetLabor" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPriceSheetLabor" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetLabor" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetLabor" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheetLabor" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPriceSheet" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPriceSheet" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheet" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheet" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPriceSheet" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPostalCode" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPostalCode" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPostalCode" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPostalCode" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPostalCode" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPaymentTerm" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPaymentTerm" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPaymentTerm" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPaymentTerm" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPaymentTerm" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupPaymentMethod" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupPaymentMethod" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupPaymentMethod" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupPaymentMethod" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupGLBreak" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupGLBreak" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupGLBreak" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupGLBreak" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupDescription" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupDescription" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupDescription" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupDescription" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupDescription" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsSetupCommunicationTemplate" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" ALTER COLUMN "CompanyId" SET NOT NULL;

    ALTER TABLE dbo."FgsCredentialProvider" ADD COLUMN "__EF_CompanyIdUuid" uuid;
    UPDATE dbo."FgsCredentialProvider" p SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = p."TenantId" AND p."CompanyId" IS NOT NULL AND c."Id" = p."CompanyId";
    UPDATE dbo."FgsCredentialProvider" p SET "__EF_CompanyIdUuid" = d."CompanyGuid" FROM (
        SELECT DISTINCT ON (t."TenantId") t."TenantId", t."CompanyGuid" FROM dbo."FgsTenantCompany" t ORDER BY t."TenantId", t."Id"
    ) d WHERE p."__EF_CompanyIdUuid" IS NULL AND p."TenantId" = d."TenantId";
    ALTER TABLE dbo."FgsCredentialProvider" DROP COLUMN "CompanyId";
    ALTER TABLE dbo."FgsCredentialProvider" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
    ALTER TABLE dbo."FgsCredentialProvider" ALTER COLUMN "CompanyId" SET NOT NULL;

    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD "GloPaymentMethodTypeId" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD "BreakLabel" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsLocation" ADD "AddressLine3" character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsLocation" ADD "AddressLine4" character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsLocation" ADD "CompanyId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsLocation" ADD "TenantId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ALTER COLUMN "SecretName" TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD "CompanyId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD "EncryptedDek" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD "EncryptedSecretValue" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD "EncryptionKeyId" character varying(500) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD "TenantId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialProviderConfiguration" ADD "CompanyId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialProviderConfiguration" ADD "TenantId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    UPDATE dbo."FgsCredentialProvider" SET "TenantId" = '00000000-0000-0000-0000-000000000000' WHERE "TenantId" IS NULL;
    ALTER TABLE dbo."FgsCredentialProvider" ALTER COLUMN "TenantId" SET NOT NULL;
    ALTER TABLE dbo."FgsCredentialProvider" ALTER COLUMN "TenantId" SET DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialAudit" ADD "CompanyId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialAudit" ADD "TenantId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsTenantCompany" ADD CONSTRAINT "AK_FgsTenantCompany_TenantId_CompanyGuid" UNIQUE ("TenantId", "CompanyGuid");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE TABLE dbo."GloCommunicationToken" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "TokenCode" text NOT NULL,
        "DisplayName" text NOT NULL,
        "SourceTableName" text NOT NULL,
        "SourceColumnName" text NOT NULL,
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_GloCommunicationToken" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE TABLE dbo."GloMasterEntityType" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Code" character varying(100) NOT NULL,
        "IsDocumentAllowed" boolean NOT NULL,
        "IsActive" boolean NOT NULL,
        "SortOrder" integer NOT NULL,
        "CreatedOn" timestamptz,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_GloMasterEntityType" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE TABLE dbo."GloPaymentMethodType" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "DisplayName" text NOT NULL,
        "IsActive" boolean NOT NULL,
        "SortOrder" integer NOT NULL,
        CONSTRAINT "PK_GloPaymentMethodType" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE TABLE dbo."GloTimeCardOption" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        CONSTRAINT "PK_GloTimeCardOption" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE TABLE dbo."FgsTenantServiceSetup" (
        "TenantId" uuid NOT NULL,
        "CompanyId" uuid NOT NULL,
        "GloTimeCardOptionId" integer NOT NULL,
        "AccountingIntegrationTypeId" integer,
        "UseExternalTaxCalculationProvider" boolean NOT NULL,
        "EnableCallBookingWidget" boolean NOT NULL,
        "EnablePaymentWidget" boolean NOT NULL,
        "EnableCustomerPortal" boolean NOT NULL,
        "EnableRulesManagement" boolean NOT NULL,
        "EnableAutoArrive" boolean NOT NULL,
        "WorkLocationRadiusForAutoArrive" integer,
        "OTStartTime" interval,
        "OTEndTime" interval,
        "DTStartTime" interval,
        "DTEndTime" interval,
        "BillHoursFromDispatchOrArrive" character varying(20) NOT NULL,
        "SourceCodeRequiredOnWorkOrder" boolean NOT NULL,
        "SourceCodeRequiredOnServiceLocation" boolean NOT NULL,
        "BillToStartNumber" bigint NOT NULL,
        "POStartNumber" bigint NOT NULL,
        "QuoteStartNumber" bigint NOT NULL,
        "WorkOrderStartNumber" bigint NOT NULL,
        "InvoiceNumberPrefix" character varying(20),
        "QuoteNumberPrefix" character varying(20),
        "PONumberPrefix" character varying(20),
        "WorkOrderNumberPrefix" character varying(20),
        "InvoiceBatchNumberFormat" character varying(200),
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsTenantServiceSetup" PRIMARY KEY ("TenantId", "CompanyId"),
        CONSTRAINT "FK_FgsTenantServiceSetup_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsTenantServiceSetup_GloTimeCardOption_GloTimeCardOptionId" FOREIGN KEY ("GloTimeCardOptionId") REFERENCES dbo."GloTimeCardOption" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupZone_TenantId_CompanyId_Code" ON dbo."FgsSetupZone" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId_Code" ON dbo."FgsSetupTitleOfCourtesy" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupTimeSlot_TenantId_CompanyId_Code" ON dbo."FgsSetupTimeSlot" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupTechTrade_TenantId_CompanyId_TradeCode" ON dbo."FgsSetupTechTrade" ("TenantId", "CompanyId", "TradeCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupTechSkillLevel_TenantId_CompanyId_Code" ON dbo."FgsSetupTechSkillLevel" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupTaxAuthority_TenantId_CompanyId_Code" ON dbo."FgsSetupTaxAuthority" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupTax_TenantId_CompanyId_TaxCode" ON dbo."FgsSetupTax" ("TenantId", "CompanyId", "TaxCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupServiceAssetType_TenantId_CompanyId_Code" ON dbo."FgsSetupServiceAssetType" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId_Code" ON dbo."FgsSetupServiceAssetManufacturer" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupPriceSheetOther_TenantId_CompanyId_CategoryCode" ON dbo."FgsSetupPriceSheetOther" ("TenantId", "CompanyId", "CategoryCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId_Code" ON dbo."FgsSetupPriceSheetMaterial" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupPriceSheet_TenantId_CompanyId_Code" ON dbo."FgsSetupPriceSheet" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupPostalCode_TenantId_CompanyId_PostalCode" ON dbo."FgsSetupPostalCode" ("TenantId", "CompanyId", "PostalCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsSetupPaymentTerm_TenantId_CompanyId_Name" ON dbo."FgsSetupPaymentTerm" ("TenantId", "CompanyId", "Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId" ON dbo."FgsSetupPaymentMethod" ("GloPaymentMethodTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsSetupPaymentMethod_TenantId_CompanyId_GloPaymentMethodTy~" ON dbo."FgsSetupPaymentMethod" ("TenantId", "CompanyId", "GloPaymentMethodTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsSetupGLBreak_TenantId_CompanyId_Code" ON dbo."FgsSetupGLBreak" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsSetupDescription_TenantId_CompanyId_DescriptionTypeCode" ON dbo."FgsSetupDescription" ("TenantId", "CompanyId", "DescriptionTypeCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTy~" ON dbo."FgsSetupCommunicationTemplate" ("TenantId", "CompanyId", "TemplateType", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsLocation_TenantId_CompanyId_MasterEntityTypeId_EntityNum~" ON dbo."FgsLocation" ("TenantId", "CompanyId", "MasterEntityTypeId", "EntityNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialSecret_CredentialProviderId" ON dbo."FgsCredentialSecret" ("CredentialProviderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialSecret_IsActive" ON dbo."FgsCredentialSecret" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialSecret_TenantId_CompanyId" ON dbo."FgsCredentialSecret" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId" ON dbo."FgsCredentialSecret" ("TenantId", "CompanyId", "CredentialProviderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId~" ON dbo."FgsCredentialSecret" ("TenantId", "CompanyId", "CredentialProviderId", "SecretName", "VersionNo");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialProviderConfiguration_CredentialProviderId" ON dbo."FgsCredentialProviderConfiguration" ("CredentialProviderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId" ON dbo."FgsCredentialProviderConfiguration" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Cred~1" ON dbo."FgsCredentialProviderConfiguration" ("TenantId", "CompanyId", "CredentialProviderId", "ConfigurationKey", "Environment");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Crede~" ON dbo."FgsCredentialProviderConfiguration" ("TenantId", "CompanyId", "CredentialProviderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsCredentialProvider_TenantId_CompanyId_Code" ON dbo."FgsCredentialProvider" ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialAudit_CredentialSecretId" ON dbo."FgsCredentialAudit" ("CredentialSecretId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialAudit_TenantId_CompanyId" ON dbo."FgsCredentialAudit" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId" ON dbo."FgsCredentialAudit" ("TenantId", "CompanyId", "CredentialSecretId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId_Ac~" ON dbo."FgsCredentialAudit" ("TenantId", "CompanyId", "CredentialSecretId", "ActionType", "NewVersionNo");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE INDEX "IX_FgsTenantServiceSetup_GloTimeCardOptionId" ON dbo."FgsTenantServiceSetup" ("GloTimeCardOptionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_GloCommunicationToken_TokenCode" ON dbo."GloCommunicationToken" ("TokenCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_GloMasterEntityType_Code" ON dbo."GloMasterEntityType" ("Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_GloPaymentMethodType_Code" ON dbo."GloPaymentMethodType" ("Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    CREATE UNIQUE INDEX "IX_GloTimeCardOption_Code" ON dbo."GloTimeCardOption" ("Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialAudit" ADD CONSTRAINT "FK_FgsCredentialAudit_FgsCredentialSecret_CredentialSecretId" FOREIGN KEY ("CredentialSecretId") REFERENCES dbo."FgsCredentialSecret" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialAudit" ADD CONSTRAINT "FK_FgsCredentialAudit_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialProvider" ADD CONSTRAINT "FK_FgsCredentialProvider_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialProviderConfiguration" ADD CONSTRAINT "FK_FgsCredentialProviderConfiguration_FgsCredentialProvider_Cr~" FOREIGN KEY ("CredentialProviderId") REFERENCES dbo."FgsCredentialProvider" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialProviderConfiguration" ADD CONSTRAINT "FK_FgsCredentialProviderConfiguration_FgsTenantCompany_TenantI~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD CONSTRAINT "FK_FgsCredentialSecret_FgsCredentialProvider_CredentialProvide~" FOREIGN KEY ("CredentialProviderId") REFERENCES dbo."FgsCredentialProvider" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsCredentialSecret" ADD CONSTRAINT "FK_FgsCredentialSecret_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsLocation" ADD CONSTRAINT "FK_FgsLocation_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsLocation" ADD CONSTRAINT "FK_FgsLocation_GloMasterEntityType_MasterEntityTypeId" FOREIGN KEY ("MasterEntityTypeId") REFERENCES dbo."GloMasterEntityType" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD CONSTRAINT "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_Com~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupDescription" ADD CONSTRAINT "FK_FgsSetupDescription_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD CONSTRAINT "FK_FgsSetupGLBreak_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD CONSTRAINT "FK_FgsSetupPaymentMethod_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD CONSTRAINT "FK_FgsSetupPaymentMethod_GloPaymentMethodType_GloPaymentMethod~" FOREIGN KEY ("GloPaymentMethodTypeId") REFERENCES dbo."GloPaymentMethodType" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPaymentTerm" ADD CONSTRAINT "FK_FgsSetupPaymentTerm_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPostalCode" ADD CONSTRAINT "FK_FgsSetupPostalCode_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPriceSheet" ADD CONSTRAINT "FK_FgsSetupPriceSheet_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPriceSheetLabor" ADD CONSTRAINT "FK_FgsSetupPriceSheetLabor_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" ADD CONSTRAINT "FK_FgsSetupPriceSheetLaborTier_FgsTenantCompany_TenantId_Compa~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPriceSheetMaterial" ADD CONSTRAINT "FK_FgsSetupPriceSheetMaterial_FgsTenantCompany_TenantId_Compan~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" ADD CONSTRAINT "FK_FgsSetupPriceSheetMaterialRange_FgsTenantCompany_TenantId_C~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupPriceSheetOther" ADD CONSTRAINT "FK_FgsSetupPriceSheetOther_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" ADD CONSTRAINT "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompany_TenantId_~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupServiceAssetMedia" ADD CONSTRAINT "FK_FgsSetupServiceAssetMedia_FgsTenantCompany_TenantId_Company~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" ADD CONSTRAINT "FK_FgsSetupServiceAssetModelSerialDescription_FgsTenantCompany~" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupServiceAssetType" ADD CONSTRAINT "FK_FgsSetupServiceAssetType_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupTax" ADD CONSTRAINT "FK_FgsSetupTax_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupTaxAuthority" ADD CONSTRAINT "FK_FgsSetupTaxAuthority_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupTaxDetail" ADD CONSTRAINT "FK_FgsSetupTaxDetail_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupTechSkillLevel" ADD CONSTRAINT "FK_FgsSetupTechSkillLevel_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupTechTrade" ADD CONSTRAINT "FK_FgsSetupTechTrade_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupTimeSlot" ADD CONSTRAINT "FK_FgsSetupTimeSlot_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupTitleOfCourtesy" ADD CONSTRAINT "FK_FgsSetupTitleOfCourtesy_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    ALTER TABLE dbo."FgsSetupZone" ADD CONSTRAINT "FK_FgsSetupZone_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260512144449_SchemaCleanupAlignment', '10.0.0');
    END IF;
END $EF$;
COMMIT;

