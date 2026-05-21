-- =============================================================================
-- Migration: 20260522140000_FgsSetupTaxExternalSyncColumns
-- Adds ExternalSystemId, SyncToken, and ShowTaxDetail to FgsSetupTax.
-- Pair with: Database/Migrations/20260522140000_FgsSetupTaxExternalSyncColumns.cs
-- =============================================================================

START TRANSACTION;

ALTER TABLE dbo."FgsSetupTax"
    ADD COLUMN IF NOT EXISTS "ExternalSystemId" character varying(200);

ALTER TABLE dbo."FgsSetupTax"
    ADD COLUMN IF NOT EXISTS "SyncToken" character varying(100);

ALTER TABLE dbo."FgsSetupTax"
    ADD COLUMN IF NOT EXISTS "ShowTaxDetail" boolean NOT NULL DEFAULT false;

INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260522140000_FgsSetupTaxExternalSyncColumns', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
