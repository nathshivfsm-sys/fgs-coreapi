-- =============================================================================
-- Migration: 20260517094959_AddCompanySizeToTenantCompany
-- Script:   20260517094959_AddCompanySizeToTenantCompany_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback:
--   1. Drops CompanySize from FgsTenantCompany while MigrationId is still recorded.
--   2. Removes the history row last.
--
-- Notes:
--   - Idempotent DO blocks.
--   - Matches Down() in: 20260517094959_AddCompanySizeToTenantCompany.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517094959_AddCompanySizeToTenantCompany') THEN
    ALTER TABLE dbo."FgsTenantCompany" DROP COLUMN "CompanySize";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260517094959_AddCompanySizeToTenantCompany') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260517094959_AddCompanySizeToTenantCompany';
    END IF;
END $EF$;

COMMIT;
