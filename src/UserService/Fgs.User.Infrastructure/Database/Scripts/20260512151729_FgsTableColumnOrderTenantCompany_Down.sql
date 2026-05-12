-- =============================================================================
-- Migration: 20260512151729_FgsTableColumnOrderTenantCompany
-- Script:   20260512151729_FgsTableColumnOrderTenantCompany_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback: removes the history row for MigrationId
--           '20260512151729_FgsTableColumnOrderTenantCompany'. No table DDL is
--           reversed because the Up migration did not change PostgreSQL column order.
--           Apply only if this migration was recorded in "__EFMigrationsHistory".
-- =============================================================================

START TRANSACTION;

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260512151729_FgsTableColumnOrderTenantCompany';

COMMIT;
