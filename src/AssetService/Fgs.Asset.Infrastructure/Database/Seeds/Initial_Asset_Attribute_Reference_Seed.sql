-- Initial_Asset_Attribute_Reference_Seed.sql
-- Idempotent default asset types, attribute definitions, and dropdown options per tenant/company.
-- Run manually after migrations and FgsTenantCompanyCache is populated (via Setup seed).
-- Warranty records (FgsAssetWarranty) are transactional and are not seeded here.

-- Default asset types
INSERT INTO asset."FgsAssetType"
(
    "TenantId",
    "CompanyId",
    "Code",
    "Name",
    "Description",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    tc."TenantId",
    tc."CompanyId",
    t."Code",
    t."Name",
    t."Description",
    true,
    CURRENT_TIMESTAMP,
    'seed'
FROM asset."FgsTenantCompanyCache" tc
CROSS JOIN (
    VALUES
        ('HVAC', 'HVAC', 'Heating, ventilation, and air conditioning equipment.'),
        ('GARAGE_DOOR', 'Garage Door', 'Garage door systems and operators.'),
        ('GENERATOR', 'Generator', 'Standby and portable generator equipment.')
) AS t("Code", "Name", "Description")
WHERE NOT EXISTS (
    SELECT 1
    FROM asset."FgsAssetType" existing
    WHERE existing."TenantId" = tc."TenantId"
      AND existing."CompanyId" = tc."CompanyId"
      AND existing."Code" = t."Code");

-- Attribute definitions per asset type
INSERT INTO asset."FgsAssetAttribute"
(
    "TenantId",
    "CompanyId",
    "AssetTypeId",
    "AttributeCode",
    "AttributeName",
    "InputType",
    "IsRequired",
    "IsSearchable",
    "DisplayOrder",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    tc."TenantId",
    tc."CompanyId",
    at."Id",
    a."AttributeCode",
    a."AttributeName",
    a."InputType",
    a."IsRequired",
    true,
    a."DisplayOrder",
    true,
    CURRENT_TIMESTAMP,
    'seed'
FROM asset."FgsTenantCompanyCache" tc
INNER JOIN asset."FgsAssetType" at
    ON at."TenantId" = tc."TenantId"
   AND at."CompanyId" = tc."CompanyId"
CROSS JOIN (
    VALUES
        ('HVAC', 'TONNAGE', 'Tonnage', 'DECIMAL', false, 1),
        ('HVAC', 'REFRIGERANT', 'Refrigerant', 'DROPDOWN', false, 2),
        ('HVAC', 'VOLTAGE', 'Voltage', 'TEXT', false, 3),
        ('HVAC', 'SEER_RATING', 'SEER Rating', 'DECIMAL', false, 4),
        ('GARAGE_DOOR', 'DOOR_TYPE', 'Door Type', 'DROPDOWN', false, 1),
        ('GARAGE_DOOR', 'DOOR_MATERIAL', 'Door Material', 'TEXT', false, 2),
        ('GARAGE_DOOR', 'DOOR_WIDTH', 'Door Width', 'DECIMAL', false, 3),
        ('GARAGE_DOOR', 'DOOR_HEIGHT', 'Door Height', 'DECIMAL', false, 4),
        ('GENERATOR', 'FUEL_TYPE', 'Fuel Type', 'DROPDOWN', false, 1),
        ('GENERATOR', 'KW_RATING', 'KW Rating', 'DECIMAL', false, 2),
        ('GENERATOR', 'PHASE', 'Phase', 'INTEGER', false, 3)
) AS a("AssetTypeCode", "AttributeCode", "AttributeName", "InputType", "IsRequired", "DisplayOrder")
WHERE at."Code" = a."AssetTypeCode"
  AND NOT EXISTS (
    SELECT 1
    FROM asset."FgsAssetAttribute" existing
    WHERE existing."TenantId" = tc."TenantId"
      AND existing."CompanyId" = tc."CompanyId"
      AND existing."AssetTypeId" = at."Id"
      AND existing."AttributeCode" = a."AttributeCode");

-- Dropdown options for DROPDOWN attributes
INSERT INTO asset."FgsAssetAttributeOption"
(
    "TenantId",
    "CompanyId",
    "AssetAttributeId",
    "OptionCode",
    "OptionName",
    "DisplayOrder",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    attr."TenantId",
    attr."CompanyId",
    attr."Id",
    o."OptionCode",
    o."OptionName",
    o."DisplayOrder",
    true,
    CURRENT_TIMESTAMP,
    'seed'
FROM asset."FgsAssetAttribute" attr
INNER JOIN asset."FgsAssetType" at
    ON at."Id" = attr."AssetTypeId"
   AND at."TenantId" = attr."TenantId"
   AND at."CompanyId" = attr."CompanyId"
CROSS JOIN (
    VALUES
        ('HVAC', 'REFRIGERANT', 'R22', 'R22', 1),
        ('HVAC', 'REFRIGERANT', 'R410A', 'R410A', 2),
        ('HVAC', 'REFRIGERANT', 'R454B', 'R454B', 3),
        ('GARAGE_DOOR', 'DOOR_TYPE', 'ROLL_UP', 'Roll Up', 1),
        ('GARAGE_DOOR', 'DOOR_TYPE', 'SECTIONAL', 'Sectional', 2),
        ('GARAGE_DOOR', 'DOOR_TYPE', 'SLIDING', 'Sliding', 3),
        ('GENERATOR', 'FUEL_TYPE', 'NATURAL_GAS', 'Natural Gas', 1),
        ('GENERATOR', 'FUEL_TYPE', 'PROPANE', 'Propane', 2),
        ('GENERATOR', 'FUEL_TYPE', 'DIESEL', 'Diesel', 3)
) AS o("AssetTypeCode", "AttributeCode", "OptionCode", "OptionName", "DisplayOrder")
WHERE at."Code" = o."AssetTypeCode"
  AND attr."AttributeCode" = o."AttributeCode"
  AND attr."InputType" = 'DROPDOWN'
  AND NOT EXISTS (
    SELECT 1
    FROM asset."FgsAssetAttributeOption" existing
    WHERE existing."TenantId" = attr."TenantId"
      AND existing."CompanyId" = attr."CompanyId"
      AND existing."AssetAttributeId" = attr."Id"
      AND existing."OptionCode" = o."OptionCode");
