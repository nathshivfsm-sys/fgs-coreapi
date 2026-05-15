-- =============================================================================
-- Migration: 20260515065507_GloGeoAndBillingCatalog
-- Script:   20260515065507_GloGeoAndBillingCatalog_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback: reverses the Up migration while MigrationId is still recorded
--           (idempotent DO blocks). Removes the history row last.
--
-- Notes:
--   - Drops GloBillingCategory seed data with the table.
--   - Restores prior GloCountry / GloLanguage / GloStateProvince shape.
--   - Matches Down() in: 20260515065507_GloGeoAndBillingCatalog.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP CONSTRAINT "FK_GloStateProvince_Country";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    DROP TABLE dbo."GloBillingCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    DROP INDEX dbo."UQ_GloStateProvince";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" DROP CONSTRAINT "PK_GloLanguage";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" DROP CONSTRAINT "PK_GloCountry";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP COLUMN "CountryCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP COLUMN "StateProvinceCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" DROP COLUMN "StateProvinceName";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" DROP COLUMN "LanguageName";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" DROP COLUMN "CurrencyCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ALTER COLUMN "IsActive" DROP DEFAULT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ALTER COLUMN "Id" TYPE bigint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD "CreatedOn" timestamptz NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD "GloCountryId" bigint NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD "RegionCode" character varying(25) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD "RegionName" character varying(200) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ALTER COLUMN "IsActive" DROP DEFAULT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ALTER COLUMN "LanguageCode" TYPE character varying(25);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ADD "Id" integer GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ADD "CreatedOn" timestamptz NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ADD "Name" character varying(200) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ADD "UpdatedOn" timestamptz;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ALTER COLUMN "IsActive" DROP DEFAULT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ALTER COLUMN "CountryName" TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ALTER COLUMN "CountryCode" TYPE character varying(10);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ADD "Id" bigint GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ADD "CreatedOn" timestamptz NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloLanguage" ADD CONSTRAINT "PK_GloLanguage" PRIMARY KEY ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloCountry" ADD CONSTRAINT "PK_GloCountry" PRIMARY KEY ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    CREATE INDEX "IX_GloStateProvince_GloCountryId" ON dbo."GloStateProvince" ("GloCountryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    CREATE UNIQUE INDEX "IX_GloLanguage_LanguageCode" ON dbo."GloLanguage" ("LanguageCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    ALTER TABLE dbo."GloStateProvince" ADD CONSTRAINT "FK_GloStateProvince_GloCountry_GloCountryId" FOREIGN KEY ("GloCountryId") REFERENCES dbo."GloCountry" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260515065507_GloGeoAndBillingCatalog';
    END IF;
END $EF$;

COMMIT;
