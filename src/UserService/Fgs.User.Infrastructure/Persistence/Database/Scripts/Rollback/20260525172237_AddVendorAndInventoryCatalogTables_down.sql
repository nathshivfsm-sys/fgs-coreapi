START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsInventoryItemAlternate";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsInventoryItemDependency";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsInventoryStock";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsVendorInventoryItem";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."GloInventoryItemType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."GloInventorySubCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsInventoryItem";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsVendor";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."GloInventoryCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsInventoryItemType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsInventorySubCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DROP TABLE dbo."FgsInventoryCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260525172237_AddVendorAndInventoryCatalogTables';
    END IF;
END $EF$;
COMMIT;

