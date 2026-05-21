-- =============================================================================
-- Migration: 20260521145233_GloSetupTenantStatusAndTenantIdBigint (Down)
-- Reverses GloSetupTenantStatus and FgsTenant.FgsTenantStatusId only.
-- TenantId uuid restore is not included; re-apply Initial_Migration if required.
-- =============================================================================

START TRANSACTION;

ALTER TABLE IF EXISTS dbo."FgsTenant"
    DROP CONSTRAINT IF EXISTS "FK_FgsTenant_GloSetupTenantStatus";

DROP INDEX IF EXISTS dbo."IX_FgsTenant_FgsTenantStatusId";

ALTER TABLE IF EXISTS dbo."FgsTenant"
    DROP COLUMN IF EXISTS "FgsTenantStatusId";

DROP TABLE IF EXISTS dbo."GloSetupTenantStatus";

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260521145233_GloSetupTenantStatusAndTenantIdBigint';

COMMIT;
