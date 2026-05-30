-- =============================================================================
-- Migration: 20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource
-- Script:   20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource_down.sql
-- Path:     Persistence/Database/Scripts/Rollback
-- Database: PostgreSQL (schema: dbo)
--
-- Reverses billing category AllowToPick/ShowToFieldTech columns, GloCommunicationToken
-- source metadata columns, and restores the prior Fgs billing category unique constraint.
-- =============================================================================

START TRANSACTION;

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260530114834_AddBillingCategoryAllowToPickAndGloCommunicationTokenSource';

ALTER TABLE dbo."FgsBillingCategory" DROP CONSTRAINT IF EXISTS "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType";

ALTER TABLE dbo."FgsBillingCategory" DROP COLUMN IF EXISTS "AllowToPick";

ALTER TABLE dbo."GloBillingCategory" DROP COLUMN IF EXISTS "AllowToPick";
ALTER TABLE dbo."GloBillingCategory" DROP COLUMN IF EXISTS "ShowToFieldTech";

ALTER TABLE dbo."GloCommunicationToken" DROP COLUMN IF EXISTS "SourceDatabaseName";
ALTER TABLE dbo."GloCommunicationToken" DROP COLUMN IF EXISTS "SourceSchemaName";

ALTER TABLE dbo."FgsBillingCategory" ALTER COLUMN "ShowToFieldTech" SET DEFAULT TRUE;

ALTER TABLE dbo."FgsBillingCategory"
    ADD CONSTRAINT "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType"
    UNIQUE ("TenantId", "CompanyId", "BillingCategoryType");

COMMIT;
