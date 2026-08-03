START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM asset."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193653_AddAssetUnitNumberAttributeValueAndRenameIsOurInstallation') THEN

    DROP TABLE IF EXISTS asset."FgsAssetAttributeValue";

    ALTER TABLE asset."FgsAsset"
    DROP COLUMN IF EXISTS "UnitNumber";

    ALTER TABLE asset."FgsAsset"
    DROP COLUMN IF EXISTS "IsOurInstallation";

    -- ServiceLocationId may contain NULLs if any were introduced after the up migration.
    UPDATE asset."FgsAsset" SET "ServiceLocationId" = 0 WHERE "ServiceLocationId" IS NULL;
    ALTER TABLE asset."FgsAsset"
    ALTER COLUMN "ServiceLocationId" SET NOT NULL;

    COMMENT ON COLUMN asset."FgsAsset"."ServiceLocationId"
    IS 'Service location where the asset is installed.';

    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM asset."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193653_AddAssetUnitNumberAttributeValueAndRenameIsOurInstallation') THEN
    DELETE FROM asset."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260803193653_AddAssetUnitNumberAttributeValueAndRenameIsOurInstallation';
    END IF;
END $EF$;
COMMIT;
