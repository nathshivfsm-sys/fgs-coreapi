-- =============================================================================
-- Migration: 20260518144909_GlobalRolesSetupDescriptionAndLaborRate
-- Script:   20260518144909_GlobalRolesSetupDescriptionAndLaborRate_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback:
--   1. Removes seeded reference data.
--   2. Restores FgsSetupGLBreak.FgsSetupTechTradeId from junction (first trade per break).
--   3. Drops new tables, FK, ShortNote column.
--   4. Removes the history row last.
--
-- Notes:
--   - Matches Down() in: 20260518144909_GlobalRolesSetupDescriptionAndLaborRate.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DELETE FROM dbo."GloSetupLaborRateType"
    WHERE "Name" IN ('Regular', 'Overtime', 'Double-Time', 'Holiday', 'Weekend');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DELETE FROM dbo."GloSetupDescriptionType"
    WHERE "Code" IN ('ReasonForCall', 'Recommendations', 'WorkSummary', 'AgreementDescription');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DELETE FROM dbo."GloRole"
    WHERE "RoleCode" IN (
        'SYSTEM_ADMIN', 'IMPLEMENTATION_SPECIALIST', 'SUPPORT_AGENT', 'BILLING_ADMIN', 'SALES_ADMIN',
        'READONLY_AUDITOR', 'TENANT_ADMIN', 'COMPANY_ADMIN', 'OPERATIONS_MANAGER', 'DISPATCHER',
        'BILLING', 'CSR', 'OFFICE_USER', 'SERVICE_MANAGER', 'FIELD_SUPERVISOR', 'FIELD_TECH');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    ALTER TABLE dbo."FgsSetupPriceSheetLabor" DROP CONSTRAINT IF EXISTS "FK_FgsSetupPriceSheetLabor_GloSetupLaborRateType_FgsSetupLaborRateTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD COLUMN IF NOT EXISTS "FgsSetupTechTradeId" bigint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    UPDATE dbo."FgsSetupGLBreak" b
    SET "FgsSetupTechTradeId" = j."FgsSetupTechTradeId"
    FROM (
        SELECT DISTINCT ON ("FgsSetupGLBreakId")
            "FgsSetupGLBreakId",
            "FgsSetupTechTradeId"
        FROM dbo."FgsSetupGLBreakTechTrade"
        ORDER BY "FgsSetupGLBreakId", "Id"
    ) j
    WHERE b."Id" = j."FgsSetupGLBreakId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DROP TABLE IF EXISTS dbo."FgsSetupGLBreakTechTrade";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DROP TABLE IF EXISTS dbo."FgsUserRole";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DROP TABLE IF EXISTS dbo."GloSetupDescriptionType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DROP TABLE IF EXISTS dbo."GloSetupLaborRateType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DROP TABLE IF EXISTS dbo."FgsRole";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DROP TABLE IF EXISTS dbo."GloRole";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetLabor_FgsSetupLaborRateTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    ALTER TABLE dbo."FgsSetupDescription" DROP COLUMN IF EXISTS "ShortNote";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260518144909_GlobalRolesSetupDescriptionAndLaborRate';
    END IF;
END $EF$;

COMMIT;
