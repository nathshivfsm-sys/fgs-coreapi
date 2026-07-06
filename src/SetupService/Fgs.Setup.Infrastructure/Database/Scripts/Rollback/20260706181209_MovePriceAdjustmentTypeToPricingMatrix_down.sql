START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix') THEN
    ALTER TABLE setup."FgsSetupPricingMatrix" DROP CONSTRAINT IF EXISTS "CK_FgsSetupPricingMatrix_PriceAdjustmentTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixMaterialTier" ADD COLUMN IF NOT EXISTS "PriceAdjustmentTypeId" smallint NOT NULL DEFAULT 1;
    COMMENT ON COLUMN setup."FgsSetupPricingMatrixMaterialTier"."PriceAdjustmentTypeId" IS 'Pricing adjustment method. Valid values: 1=Markup Percent, 2=Markup Amount, 3=Multiplier.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix') THEN
    UPDATE setup."FgsSetupPricingMatrixMaterialTier" mt
    SET "PriceAdjustmentTypeId" = pm."PriceAdjustmentTypeId"
    FROM setup."FgsSetupPricingMatrix" pm
    WHERE mt."PricingMatrixId" = pm."Id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix') THEN
    ALTER TABLE setup."FgsSetupPricingMatrix" DROP COLUMN IF EXISTS "PriceAdjustmentTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix') THEN
    CREATE INDEX IF NOT EXISTS "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId_PriceAdjustmentTypeId"
        ON setup."FgsSetupPricingMatrixMaterialTier" ("TenantId", "CompanyId", "PriceAdjustmentTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixMaterialTier" ADD CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId"
        CHECK ("PriceAdjustmentTypeId" BETWEEN 1 AND 3);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixMaterialTier" ALTER COLUMN "PriceAdjustmentTypeId" DROP DEFAULT;
    END IF;
END $EF$;

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260706181209_MovePriceAdjustmentTypeToPricingMatrix';

COMMIT;
