-- =============================================================================
-- Migration: 20260512151729_FgsTableColumnOrderTenantCompany
-- Script:   20260512151729_FgsTableColumnOrderTenantCompany_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Records MigrationId '20260512151729_FgsTableColumnOrderTenantCompany' in
--      "__EFMigrationsHistory" when not yet present (idempotent with other scripts).
--
-- Notes:
--   The EF migration only attaches Relational:ColumnOrder metadata on existing columns.
--   Npgsql does not emit physical DDL for this; column order in PostgreSQL is unchanged.
--   Run this script after applying the same migration via "dotnet ef database update"
--   if you maintain history using SQL scripts instead of EF only.
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260512151729_FgsTableColumnOrderTenantCompany') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260512151729_FgsTableColumnOrderTenantCompany', '10.0.0');
    END IF;
END $EF$;

COMMIT;
