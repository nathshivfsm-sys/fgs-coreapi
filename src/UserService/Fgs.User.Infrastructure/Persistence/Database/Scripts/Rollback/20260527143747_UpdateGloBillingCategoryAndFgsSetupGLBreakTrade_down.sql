START TRANSACTION;

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260527143747_UpdateGloBillingCategoryAndFgsSetupGLBreakTrade';

DROP TABLE IF EXISTS dbo."FgsSetupGLBreakTrade";

ALTER TABLE dbo."GloBillingCategory" DROP COLUMN IF EXISTS "DisplayOrder";
ALTER TABLE dbo."GloBillingCategory" DROP COLUMN IF EXISTS "Description";

ALTER TABLE dbo."FgsSetupGLBreak" ADD COLUMN IF NOT EXISTS "Trades" text[];

COMMIT;
