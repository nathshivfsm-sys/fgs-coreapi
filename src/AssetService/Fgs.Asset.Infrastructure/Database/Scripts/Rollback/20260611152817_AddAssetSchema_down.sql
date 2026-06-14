START TRANSACTION;

-- Prerequisite: tables from 20260612160918_AddAssetWarrantyAndAttributeTables reference objects below.
DROP TABLE IF EXISTS asset."FgsAssetAttributeOption";

DROP TABLE IF EXISTS asset."FgsAssetWarranty";

DROP TABLE IF EXISTS asset."FgsAssetAttribute";

DROP TABLE IF EXISTS asset."FgsAsset";

DROP TABLE IF EXISTS asset."FgsAssetModel";

DROP TABLE IF EXISTS asset."FgsAssetStatus";

DROP TABLE IF EXISTS asset."FgsServiceLocationCache";

DROP TABLE IF EXISTS asset."FgsAssetManufacturer";

DROP TABLE IF EXISTS asset."FgsAssetType";

DROP TABLE IF EXISTS asset."FgsTenantCompanyCache";

DELETE FROM asset."__EFMigrationsHistory"
WHERE "MigrationId" IN (
    '20260612160918_AddAssetWarrantyAndAttributeTables',
    '20260611152817_AddAssetSchema');

COMMIT;
