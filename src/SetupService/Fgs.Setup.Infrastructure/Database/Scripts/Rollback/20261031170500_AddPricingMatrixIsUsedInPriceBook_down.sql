START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031170500_AddPricingMatrixIsUsedInPriceBook') THEN
    ALTER TABLE setup."FgsSetupPricingMatrix" DROP COLUMN IF EXISTS "IsUsedInPriceBook";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031170500_AddPricingMatrixIsUsedInPriceBook') THEN
    DELETE FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031170500_AddPricingMatrixIsUsedInPriceBook';
    END IF;
END $EF$;

COMMIT;
