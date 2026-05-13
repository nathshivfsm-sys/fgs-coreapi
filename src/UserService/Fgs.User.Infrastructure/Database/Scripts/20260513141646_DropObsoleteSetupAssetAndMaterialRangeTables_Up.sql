-- =============================================================================
-- Migration: 20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables
-- Script:   20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Drops obsolete setup tables:
--        dbo."FgsSetupPriceSheetMaterialRange"
--        dbo."FgsSetupServiceAssetMedia"
--        dbo."FgsSetupServiceAssetModelSerialDescription"
--   2. Applies remaining model changes from the same EF migration (column adds).
--   3. Records MigrationId in "__EFMigrationsHistory" when not yet present.
--
-- Notes:
--   - Idempotent DO blocks (matches: dotnet ef migrations script ... --idempotent).
--   - No seed/reference data is inserted.
--   - Matches: 20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    DROP TABLE dbo."FgsSetupPriceSheetMaterialRange";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    DROP TABLE dbo."FgsSetupServiceAssetMedia";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    DROP TABLE dbo."FgsSetupServiceAssetModelSerialDescription";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    ALTER TABLE dbo."FgsSetupTimeSlot" ADD "FgsSetupZoneId" bigint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    ALTER TABLE dbo."FgsSetupTaxDetail" ADD "IsExternalSystemRecord" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    ALTER TABLE dbo."FgsSetupTaxAuthority" ADD "IsExternalSystemRecord" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    ALTER TABLE dbo."FgsSetupTax" ADD "IsExternalSystemRecord" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    ALTER TABLE dbo."FgsSetupPriceSheetLabor" ADD "FgsSetupLaborRateTypeId" integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    ALTER TABLE dbo."FgsSetupPriceSheet" ADD "IsLaborRateBySkillLevel" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    ALTER TABLE dbo."FgsSetupPriceSheet" ADD "IsLaborTierStructure" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260513141646_DropObsoleteSetupAssetAndMaterialRangeTables', '10.0.0');
    END IF;
END $EF$;

COMMIT;
