START TRANSACTION;

DROP TABLE IF EXISTS asset."FgsAssetAttributeOption";

DROP TABLE IF EXISTS asset."FgsAssetWarranty";

DROP TABLE IF EXISTS asset."FgsAssetAttribute";

DELETE FROM asset."__EFMigrationsHistory"
WHERE "MigrationId" = '20260612160918_AddAssetWarrantyAndAttributeTables';

COMMIT;
