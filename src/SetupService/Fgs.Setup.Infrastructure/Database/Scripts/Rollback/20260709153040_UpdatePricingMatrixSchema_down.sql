START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixLaborTier" DROP CONSTRAINT IF EXISTS "FK_FgsSetupPricingMatrixLaborTier_TechSkillLevel";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixOther" DROP CONSTRAINT IF EXISTS "CK_FgsSetupPricingMatrixOther_AdjustmentValue";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    DROP INDEX IF EXISTS setup."IX_FgsSetupPricingMatrixLaborTier_TechSkillLevelId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixOther" RENAME COLUMN "AdjustmentValue" TO "MarkupPercent";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    COMMENT ON COLUMN setup."FgsSetupPricingMatrixOther"."MarkupPercent"
    IS 'Markup percentage applied to the base cost.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixOther" ADD CONSTRAINT "CK_FgsSetupPricingMatrixOther_MarkupPercent"
        CHECK ("MarkupPercent" IS NULL OR "MarkupPercent" >= 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixLaborTier" DROP COLUMN IF EXISTS "TechSkillLevelId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrix" DROP COLUMN IF EXISTS "IsDefault";
    END IF;
END $EF$;

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema';

COMMIT;
