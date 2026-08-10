START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM asset."__EFMigrationsHistory" WHERE "MigrationId" = '20260810124015_AddAssetLocation') THEN

    ALTER TABLE asset."FgsAsset"
    DROP COLUMN IF EXISTS "AssetLocation";

    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM asset."__EFMigrationsHistory" WHERE "MigrationId" = '20260810124015_AddAssetLocation') THEN
    DELETE FROM asset."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260810124015_AddAssetLocation';
    END IF;
END $EF$;
COMMIT;
