START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031170500_AddPricingMatrixIsUsedInPriceBook') THEN
    ALTER TABLE setup."FgsSetupPricingMatrix"
        ADD COLUMN IF NOT EXISTS "IsUsedInPriceBook" boolean NOT NULL DEFAULT false;
    COMMENT ON COLUMN setup."FgsSetupPricingMatrix"."IsUsedInPriceBook"
        IS 'Indicates whether this pricing matrix is used by the Price Book for pricing calculations.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031170500_AddPricingMatrixIsUsedInPriceBook') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261031170500_AddPricingMatrixIsUsedInPriceBook', '10.0.8');
    END IF;
END $EF$;

COMMIT;
