START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrix" ADD "IsDefault" boolean NOT NULL DEFAULT TRUE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixLaborTier" ADD "TechSkillLevelId" bigint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixOther" DROP CONSTRAINT "CK_FgsSetupPricingMatrixOther_MarkupPercent";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixOther" RENAME COLUMN "MarkupPercent" TO "AdjustmentValue";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    COMMENT ON COLUMN setup."FgsSetupPricingMatrixOther"."AdjustmentValue"
    IS 'Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixOther" ADD CONSTRAINT "CK_FgsSetupPricingMatrixOther_AdjustmentValue" CHECK ("AdjustmentValue" IS NULL OR "AdjustmentValue" >= 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    CREATE INDEX "IX_FgsSetupPricingMatrixLaborTier_TechSkillLevelId" ON setup."FgsSetupPricingMatrixLaborTier" ("TechSkillLevelId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    ALTER TABLE setup."FgsSetupPricingMatrixLaborTier" ADD CONSTRAINT "FK_FgsSetupPricingMatrixLaborTier_TechSkillLevel" FOREIGN KEY ("TechSkillLevelId") REFERENCES setup."FgsSetupTechSkillLevel" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260709153040_UpdatePricingMatrixSchema') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260709153040_UpdatePricingMatrixSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;
