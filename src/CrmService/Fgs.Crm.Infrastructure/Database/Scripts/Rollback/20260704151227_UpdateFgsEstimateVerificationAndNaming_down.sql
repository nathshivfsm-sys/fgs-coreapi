START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260704151227_UpdateFgsEstimateVerificationAndNaming') THEN
    ALTER TABLE IF EXISTS crm."FgsEstimate" RENAME COLUMN "EstimateDescription" TO "QuoteDescription";
    COMMENT ON COLUMN crm."FgsEstimate"."QuoteDescription" IS 'Detailed quote description presented to the customer.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260704151227_UpdateFgsEstimateVerificationAndNaming') THEN
    ALTER TABLE IF EXISTS crm."FgsEstimate" DROP COLUMN IF EXISTS "InternalNotes";
    ALTER TABLE IF EXISTS crm."FgsEstimate" DROP COLUMN IF EXISTS "InstallationDescription";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260704151227_UpdateFgsEstimateVerificationAndNaming') THEN
    ALTER TABLE IF EXISTS crm."FgsEstimate" RENAME COLUMN "EstimateName" TO "QuoteName";
    COMMENT ON COLUMN crm."FgsEstimate"."QuoteName" IS 'User-facing quote name.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM crm."__EFMigrationsHistory" WHERE "MigrationId" = '20260704151227_UpdateFgsEstimateVerificationAndNaming') THEN
    ALTER TABLE IF EXISTS crm."FgsEstimate" DROP COLUMN IF EXISTS "RecommendedByEmployeeId";
    ALTER TABLE IF EXISTS crm."FgsEstimate" DROP COLUMN IF EXISTS "VerificationRequired";
    ALTER TABLE IF EXISTS crm."FgsEstimate" DROP COLUMN IF EXISTS "VerifiedByEmployeeId";
    ALTER TABLE IF EXISTS crm."FgsEstimate" DROP COLUMN IF EXISTS "VerifiedOn";
    END IF;
END $EF$;

DELETE FROM crm."__EFMigrationsHistory"
WHERE "MigrationId" = '20260704151227_UpdateFgsEstimateVerificationAndNaming';

COMMIT;
