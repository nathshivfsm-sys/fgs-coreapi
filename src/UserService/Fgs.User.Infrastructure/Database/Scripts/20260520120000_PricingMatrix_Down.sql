-- =============================================================================
-- Revert: 20260520120000_PricingMatrix
-- =============================================================================

START TRANSACTION;

DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixLaborTier" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixLabor" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixMaterialTier" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixOther" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrix" CASCADE;

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260520120000_PricingMatrix';

COMMIT;
