-- =============================================================================
-- Migration: 20260517094959_AddCompanySizeToTenantCompany
-- Script:   20260517094959_AddCompanySizeToTenantCompany_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Adds nullable CompanySize (integer) to FgsTenantCompany for signup headcount band.
--   2. Records MigrationId in "__EFMigrationsHistory" when not yet present.
--
-- Notes:
--   - Idempotent DO blocks (matches: dotnet ef migrations script --idempotent).
--   - Matches: 20260517094959_AddCompanySizeToTenantCompany.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517094959_AddCompanySizeToTenantCompany') THEN
    ALTER TABLE dbo."FgsTenantCompany" ADD "CompanySize" integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517094959_AddCompanySizeToTenantCompany') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260517094959_AddCompanySizeToTenantCompany', '10.0.0');
    END IF;
END $EF$;

COMMIT;
