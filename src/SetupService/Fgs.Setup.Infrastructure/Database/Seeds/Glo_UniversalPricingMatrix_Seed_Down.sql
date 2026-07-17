-- Glo_UniversalPricingMatrix_Seed_Down.sql
-- Reverses Glo_UniversalPricingMatrix_Seed.sql

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

DELETE FROM glo."GloUniversalPricingService"
WHERE "ServiceCode" IN (
    'HOUSE_CLEANING',
    'PRESSURE_WASHING',
    'PEST_CONTROL',
    'HOLIDAY_LIGHTING'
);
