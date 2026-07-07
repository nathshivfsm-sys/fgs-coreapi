START TRANSACTION;

-- Remove tenant provisioning metadata for universal pricing matrix
DELETE FROM glo."GloSeedTableColumnMapping"
WHERE "SeedTableMappingId" IN (
    SELECT "Id"
    FROM glo."GloSeedTableMapping"
    WHERE "SeedCode" = 'ALL_GloUniversalPricingService'
);

DELETE FROM glo."GloSeedTableMapping"
WHERE "SeedCode" = 'ALL_GloUniversalPricingService';

DELETE FROM glo."GloUniversalMatrixSizeTier";
DELETE FROM glo."GloUniversalMatrixTier";
DELETE FROM glo."GloUniversalPricingService";

-- Drop setup children (FK order)
DROP TABLE IF EXISTS setup."FgsUniversalMatrixAddOn";
DROP TABLE IF EXISTS setup."FgsUniversalMatrixOneTimeFee";
DROP TABLE IF EXISTS setup."FgsUniversalMatrixFrequencyDiscount";
DROP TABLE IF EXISTS setup."FgsUniversalMatrixItem";
DROP TABLE IF EXISTS setup."FgsUniversalMatrixSizeTier";
DROP TABLE IF EXISTS setup."FgsUniversalMatrixTier";
DROP TABLE IF EXISTS setup."FgsUniversalPricingService";

-- Drop glo children then parent
DROP TABLE IF EXISTS glo."GloUniversalMatrixSizeTier";
DROP TABLE IF EXISTS glo."GloUniversalMatrixTier";
DROP TABLE IF EXISTS glo."GloUniversalPricingService";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260707141525_AddUniversalPricingMatrix';

COMMIT;
