-- Initial_Inventory_Reference_Seed.sql
-- Idempotent per-tenant inventory reference data (item types and default location).
-- Run manually AFTER:
--   1. dotnet ef database update (InventoryService)
--   2. inventory.FgsTenantCompanyCache is populated (tenant provisioning or TENANT_FgsTenantCompany_inventory_cache seed)
-- NOT part of EF migrations.

-- System inventory item types (INV/NS/SERV/TOOL/EQUIP)
INSERT INTO inventory."FgsInventoryItemType"
(
    "TenantId",
    "CompanyId",
    "ItemTypeCode",
    "Name",
    "Description",
    "TracksQuantity",
    "DisplayOrder",
    "IsSystem",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    tc."TenantId",
    tc."CompanyId",
    s."ItemTypeCode",
    s."Name",
    s."Description",
    s."TracksQuantity",
    s."DisplayOrder",
    true,
    true,
    timezone('utc', now()),
    'seed'
FROM inventory."FgsTenantCompanyCache" tc
CROSS JOIN (
    VALUES
        ('INV',   'Inventory Part',   'Standard inventory part that tracks quantity on hand.',              true,  1::smallint),
        ('NS',    'Non-Stock Part',   'Item used for purchasing or selling without quantity tracking.',    false, 2::smallint),
        ('SERV',  'Service',          'Labor or service item with no inventory tracking.',                  false, 3::smallint),
        ('TOOL',  'Tool',             'Operational tool item that tracks quantity.',                          true,  4::smallint),
        ('EQUIP', 'Equipment',        'Equipment item that tracks quantity.',                               true,  5::smallint)
) AS s("ItemTypeCode", "Name", "Description", "TracksQuantity", "DisplayOrder")
WHERE NOT EXISTS (
    SELECT 1
    FROM inventory."FgsInventoryItemType" existing
    WHERE existing."TenantId" = tc."TenantId"
      AND existing."CompanyId" = tc."CompanyId"
      AND existing."ItemTypeCode" = s."ItemTypeCode"
);

-- Default main warehouse location per company
INSERT INTO inventory."FgsInventoryLocation"
(
    "TenantId",
    "CompanyId",
    "InventoryLocationCode",
    "Name",
    "InventoryLocationType",
    "Description",
    "DisplayOrder",
    "IsDefault",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    tc."TenantId",
    tc."CompanyId",
    'MAIN',
    'Main Warehouse',
    'WAREHOUSE',
    'Default company warehouse location.',
    1::smallint,
    true,
    true,
    timezone('utc', now()),
    'seed'
FROM inventory."FgsTenantCompanyCache" tc
WHERE NOT EXISTS (
    SELECT 1
    FROM inventory."FgsInventoryLocation" existing
    WHERE existing."TenantId" = tc."TenantId"
      AND existing."CompanyId" = tc."CompanyId"
      AND existing."InventoryLocationCode" = 'MAIN'
);
