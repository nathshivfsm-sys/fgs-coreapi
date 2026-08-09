START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM inventory."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193809_AddInventorySerialAndItemSerializedFlag') THEN

    DROP TABLE IF EXISTS inventory."FgsInventorySerial";

    DROP INDEX IF EXISTS inventory."IX_FgsInventoryTransaction_TenantId_CompanyId_SerialNumber";

    ALTER TABLE inventory."FgsInventoryTransaction"
    DROP COLUMN IF EXISTS "SerialNumber";

    ALTER TABLE inventory."FgsInventoryItem"
    DROP COLUMN IF EXISTS "IsSerialized";

    ALTER TABLE inventory."FgsInventoryItem"
    ADD COLUMN IF NOT EXISTS "DefaultTaxable" boolean NOT NULL DEFAULT TRUE;

    DROP TYPE IF EXISTS inventory."FgsInventorySerialStatus";

    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM inventory."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193809_AddInventorySerialAndItemSerializedFlag') THEN
    DELETE FROM inventory."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260803193809_AddInventorySerialAndItemSerializedFlag';
    END IF;
END $EF$;
COMMIT;
