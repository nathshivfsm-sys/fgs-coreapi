-- Initial_Inventory_Reference_Seed_Down.sql
-- Removes rows inserted by Initial_Inventory_Reference_Seed.sql

DELETE FROM inventory."FgsInventoryLocation"
WHERE "InventoryLocationCode" = 'MAIN'
  AND "CreatedBy" = 'seed';

DELETE FROM inventory."FgsInventoryItemType"
WHERE "ItemTypeCode" IN ('INV', 'NS', 'SERV', 'TOOL', 'EQUIP')
  AND "IsSystem" = true
  AND "CreatedBy" = 'seed';
