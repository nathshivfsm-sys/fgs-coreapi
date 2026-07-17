-- Glo_UniversalPricingMatrix_Seed.sql
-- Idempotent seed for glo universal pricing matrix reference data and tenant provisioning mappings.
-- Run after migration 20260707141525_AddUniversalPricingMatrix.

-- =============================================================================
-- GloUniversalPricingService
-- =============================================================================
INSERT INTO glo."GloUniversalPricingService"
(
    "Id",
    "ServiceCode",
    "Name",
    "DisplayOrder",
    "CreatedOn"
)
SELECT
    v."Id",
    v."ServiceCode",
    v."Name",
    v."DisplayOrder",
    timezone('utc', now())
FROM (
    VALUES
        (1::smallint, 'HOUSE_CLEANING',   'House Cleaning',   1::smallint),
        (2::smallint, 'PRESSURE_WASHING', 'Pressure Washing', 2::smallint),
        (3::smallint, 'PEST_CONTROL',     'Pest Control',     3::smallint),
        (4::smallint, 'HOLIDAY_LIGHTING', 'Holiday Lighting', 4::smallint)
) AS v("Id", "ServiceCode", "Name", "DisplayOrder")
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloUniversalPricingService" existing
    WHERE existing."ServiceCode" = v."ServiceCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloUniversalPricingService"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloUniversalPricingService"), 1),
    true);

-- =============================================================================
-- GloUniversalMatrixTier
-- =============================================================================
INSERT INTO glo."GloUniversalMatrixTier"
(
    "UniversalPricingServiceId",
    "Name",
    "Multiplier",
    "DisplayOrder",
    "CreatedOn"
)
SELECT gps."Id", v."Name", v."Multiplier", v."DisplayOrder", timezone('utc', now())
FROM (
    VALUES
        ('HOUSE_CLEANING', 'Standard',   1.0000::numeric(8,4), 1::smallint),
        ('HOUSE_CLEANING', 'Luxury',     1.5000::numeric(8,4), 2::smallint),
        ('HOUSE_CLEANING', 'Deep Clean', 2.0000::numeric(8,4), 3::smallint),
        ('HOUSE_CLEANING', 'Premium',    1.7500::numeric(8,4), 4::smallint),
        ('PEST_CONTROL',   'Standard',   1.0000::numeric(8,4), 1::smallint),
        ('PEST_CONTROL',   'Organic',    1.1000::numeric(8,4), 2::smallint)
) AS v("ServiceCode", "Name", "Multiplier", "DisplayOrder")
INNER JOIN glo."GloUniversalPricingService" gps ON gps."ServiceCode" = v."ServiceCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloUniversalMatrixTier" existing
    WHERE existing."UniversalPricingServiceId" = gps."Id"
      AND existing."Name" = v."Name"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloUniversalMatrixTier"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloUniversalMatrixTier"), 1),
    true);

-- =============================================================================
-- GloUniversalMatrixSizeTier
-- =============================================================================
INSERT INTO glo."GloUniversalMatrixSizeTier"
(
    "UniversalPricingServiceId",
    "Name",
    "Multiplier",
    "DisplayOrder",
    "CreatedOn"
)
SELECT gps."Id", v."Name", v."Multiplier", v."DisplayOrder", timezone('utc', now())
FROM (
    VALUES
        ('HOUSE_CLEANING', 'Regular', 1.0000::numeric(8,4), 1::smallint),
        ('HOUSE_CLEANING', 'Medium',  1.2500::numeric(8,4), 2::smallint),
        ('HOUSE_CLEANING', 'Large',   1.5000::numeric(8,4), 3::smallint),
        ('HOUSE_CLEANING', 'XL',      2.0000::numeric(8,4), 4::smallint),
        ('PEST_CONTROL',   'Regular', 1.0000::numeric(8,4), 1::smallint),
        ('PEST_CONTROL',   'Medium',  1.0500::numeric(8,4), 2::smallint),
        ('PEST_CONTROL',   'Large',   1.1000::numeric(8,4), 3::smallint),
        ('PEST_CONTROL',   'XL',      1.1500::numeric(8,4), 4::smallint)
) AS v("ServiceCode", "Name", "Multiplier", "DisplayOrder")
INNER JOIN glo."GloUniversalPricingService" gps ON gps."ServiceCode" = v."ServiceCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloUniversalMatrixSizeTier" existing
    WHERE existing."UniversalPricingServiceId" = gps."Id"
      AND existing."Name" = v."Name"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloUniversalMatrixSizeTier"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloUniversalMatrixSizeTier"), 1),
    true);

-- =============================================================================
-- GloSeedTableMapping / GloSeedTableColumnMapping
-- Tenant provisioning: GloUniversalPricingService -> FgsUniversalPricingService
-- =============================================================================
INSERT INTO glo."GloSeedTableMapping"
(
    "SeedCode",
    "SourceDatabaseName",
    "SourceSchemaName",
    "SourceTableName",
    "TargetDatabaseName",
    "TargetSchemaName",
    "TargetTableName",
    "SeedOrder",
    "Description",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    v."SeedCode",
    v."SourceDatabaseName",
    v."SourceSchemaName",
    v."SourceTableName",
    v."TargetDatabaseName",
    v."TargetSchemaName",
    v."TargetTableName",
    v."SeedOrder",
    v."Description",
    v."IsActive",
    timezone('utc', now()),
    'System'
FROM (
    VALUES
        ('ALL_GloUniversalPricingService', 'fgs_dev_db', 'glo', 'GloUniversalPricingService', 'fgs_dev_db', 'setup', 'FgsUniversalPricingService', 510, 'Universal Pricing Service', true)
) AS v("SeedCode", "SourceDatabaseName", "SourceSchemaName", "SourceTableName", "TargetDatabaseName", "TargetSchemaName", "TargetTableName", "SeedOrder", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1 FROM glo."GloSeedTableMapping" m WHERE m."SeedCode" = v."SeedCode"
);

INSERT INTO glo."GloSeedTableColumnMapping"
(
    "SeedTableMappingId",
    "SourceColumnName",
    "TargetColumnName",
    "TransformationType",
    "StaticValue",
    "ColumnOrder",
    "IsRequired",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    m."Id",
    c."SourceColumnName",
    c."TargetColumnName",
    c."TransformationType",
    c."StaticValue",
    c."ColumnOrder",
    c."IsRequired",
    c."IsActive",
    timezone('utc', now()),
    'System'
FROM glo."GloSeedTableMapping" m
INNER JOIN (
    VALUES
        ('ALL_GloUniversalPricingService', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloUniversalPricingService', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloUniversalPricingService', 'ServiceCode', 'UniversalPricingServiceCode', NULL, NULL, 3, true, true),
        ('ALL_GloUniversalPricingService', 'DisplayOrder', 'DisplayOrder', NULL, NULL, 4, true, true),
        ('ALL_GloUniversalPricingService', NULL, 'IsActive', 'STATIC', 'true', 5, true, true),
        ('ALL_GloUniversalPricingService', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 6, true, true),
        ('ALL_GloUniversalPricingService', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 7, false, true)
) AS c("SeedCode", "SourceColumnName", "TargetColumnName", "TransformationType", "StaticValue", "ColumnOrder", "IsRequired", "IsActive")
    ON c."SeedCode" = m."SeedCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloSeedTableColumnMapping" existing
    WHERE existing."SeedTableMappingId" = m."Id"
      AND existing."TargetColumnName" = c."TargetColumnName"
);
