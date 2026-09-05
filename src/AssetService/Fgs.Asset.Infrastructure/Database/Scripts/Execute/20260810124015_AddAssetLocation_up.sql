START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM asset."__EFMigrationsHistory" WHERE "MigrationId" = '20260810124015_AddAssetLocation') THEN

    ALTER TABLE asset."FgsAsset"
    ADD COLUMN IF NOT EXISTS "AssetLocation" character varying(200);

    COMMENT ON COLUMN asset."FgsAsset"."AssetLocation"
    IS 'Physical location of the asset within the service location or unit, such as Roof - Northeast Corner, Mechanical Room, 2nd Floor West Wing, or Basement.';

    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM asset."__EFMigrationsHistory" WHERE "MigrationId" = '20260810124015_AddAssetLocation') THEN
    INSERT INTO asset."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260810124015_AddAssetLocation', '10.0.8');
    END IF;
END $EF$;
COMMIT;
