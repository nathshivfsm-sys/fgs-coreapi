-- =============================================================================
-- Migration: 20260515065507_GloGeoAndBillingCatalog
-- Script:   20260515065507_GloGeoAndBillingCatalog_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Restructures GloCountry, GloLanguage, and GloStateProvince to code-keyed
--      geo catalogs (CountryCode / LanguageCode PKs; StateProvince by CountryCode).
--   2. Creates GloBillingCategory and seeds reference rows.
--   3. Records MigrationId in "__EFMigrationsHistory" when not yet present.
--
-- Notes:
--   - Idempotent DO blocks (matches: dotnet ef migrations script --idempotent).
--   - Matches: 20260515065507_GloGeoAndBillingCatalog.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP CONSTRAINT "FK_GloStateProvince_GloCountry_GloCountryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    DROP INDEX dbo."IX_GloStateProvince_GloCountryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" DROP CONSTRAINT "PK_GloLanguage";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    DROP INDEX dbo."IX_GloLanguage_LanguageCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" DROP CONSTRAINT "PK_GloCountry";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP COLUMN "CreatedOn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP COLUMN "GloCountryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP COLUMN "RegionCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP COLUMN "RegionName";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" DROP COLUMN "Id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" DROP COLUMN "CreatedOn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" DROP COLUMN "Name";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" DROP COLUMN "UpdatedOn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" DROP COLUMN "Id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" DROP COLUMN "CreatedOn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ALTER COLUMN "IsActive" SET DEFAULT TRUE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ALTER COLUMN "Id" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD "CountryCode" character varying(2) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD "StateProvinceCode" character varying(10) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD "StateProvinceName" character varying(100) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ALTER COLUMN "LanguageCode" TYPE character varying(5);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ALTER COLUMN "IsActive" SET DEFAULT TRUE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ADD "LanguageName" character varying(100) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ALTER COLUMN "IsActive" SET DEFAULT TRUE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ALTER COLUMN "CountryName" TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ALTER COLUMN "CountryCode" TYPE character varying(2);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ADD "CurrencyCode" character varying(3);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ADD CONSTRAINT "PK_GloLanguage" PRIMARY KEY ("LanguageCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ADD CONSTRAINT "PK_GloCountry" PRIMARY KEY ("CountryCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    CREATE TABLE dbo."GloBillingCategory" (
        "BillingCategoryType" character varying(2) NOT NULL,
        "BillingCategoryName" character varying(100) NOT NULL,
        CONSTRAINT "PK_GloBillingCategory" PRIMARY KEY ("BillingCategoryType")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    INSERT INTO dbo."GloBillingCategory" ("BillingCategoryType", "BillingCategoryName")
    VALUES
        ('EQ', 'Equipment'),
        ('MT', 'Material'),
        ('LB', 'Labor'),
        ('SB', 'Sub Contractor'),
        ('SF', 'Service Fee'),
        ('SH', 'Shipping'),
        ('TX', 'Tax'),
        ('DS', 'Discount'),
        ('OT', 'Other')
    ON CONFLICT ("BillingCategoryType") DO NOTHING;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    CREATE UNIQUE INDEX "UQ_GloStateProvince" ON dbo."GloStateProvince" ("CountryCode", "StateProvinceCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD CONSTRAINT "FK_GloStateProvince_Country" FOREIGN KEY ("CountryCode") REFERENCES dbo."GloCountry" ("CountryCode") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260515065507_GloGeoAndBillingCatalog', '10.0.0');
    END IF;
END $EF$;

COMMIT;
