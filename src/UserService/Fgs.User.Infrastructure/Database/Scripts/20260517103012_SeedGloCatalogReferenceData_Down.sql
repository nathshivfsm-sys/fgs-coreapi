-- =============================================================================
-- Migration: 20260517103012_SeedGloCatalogReferenceData
-- Script:   20260517103012_SeedGloCatalogReferenceData_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback:
--   1. Removes seeded catalog rows (by Code / LanguageCode).
--   2. Drops GloLanguage.CultureCode.
--   3. Removes the history row last.
--
-- Notes:
--   - Idempotent DO blocks.
--   - Matches Down() in: 20260517103012_SeedGloCatalogReferenceData.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    DELETE FROM dbo."GloBusinessType"
    WHERE "Code" IN ('HVAC', 'PLUMBING', 'ELECTRICAL', 'PESTCONTROL', 'LAWNCARE', 'TRASHPICKUP', 'GARAGEDOOR', 'HOUSECLEANING', 'PAINTING');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    DELETE FROM dbo."GloTimeCardOption"
    WHERE "Code" IN ('NONE', 'DISPATCHARRIVECOMPLETE', 'CHECKINCHECKOUT');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    DELETE FROM dbo."GloAccountingIntegrationType"
    WHERE "Code" IN ('NONE', 'QUICKBOOKSONLINE', 'SAGEINTACCT');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    DELETE FROM dbo."GloLanguage"
    WHERE "LanguageCode" IN ('EN', 'ES', 'FR');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    DELETE FROM dbo."GloMasterEntityType"
    WHERE "Code" IN ('TENANT', 'COMPANY', 'SERVICELOCATION', 'BILLTO', 'VENDOR', 'SUBCONTRACTOR', 'LEAD', 'PROPOSAL', 'CUSTOMER', 'WORKORDER', 'INVOICE');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    DELETE FROM dbo."GloLocationType"
    WHERE "Code" IN ('BILLING', 'SHIPPING', 'PHYSICAL', 'SERVICE', 'MAILING', 'HQ', 'REMITTO', 'JOBSITE');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    ALTER TABLE dbo."GloLanguage" DROP COLUMN IF EXISTS "CultureCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260517103012_SeedGloCatalogReferenceData';
    END IF;
END $EF$;

COMMIT;
