-- =============================================================================
-- Migration: 20260522140000_FgsSetupTaxExternalSyncColumns (Down)
-- Removes ExternalSystemId, SyncToken, and ShowTaxDetail from FgsSetupTax.
-- Pair with: Database/Migrations/20260522140000_FgsSetupTaxExternalSyncColumns.cs
-- =============================================================================

START TRANSACTION;

ALTER TABLE dbo."FgsSetupTax"
    DROP COLUMN IF EXISTS "ShowTaxDetail";

ALTER TABLE dbo."FgsSetupTax"
    DROP COLUMN IF EXISTS "SyncToken";

ALTER TABLE dbo."FgsSetupTax"
    DROP COLUMN IF EXISTS "ExternalSystemId";

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260522140000_FgsSetupTaxExternalSyncColumns';

COMMIT;
