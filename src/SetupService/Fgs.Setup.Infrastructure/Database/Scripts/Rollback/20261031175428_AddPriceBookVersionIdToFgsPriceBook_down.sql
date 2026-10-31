START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    ALTER TABLE setup."FgsPriceBook" DROP CONSTRAINT IF EXISTS "FK_FgsPriceBook_FgsSetupPriceBookVersion_PriceBookVersionId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    DROP INDEX IF EXISTS setup."IX_FgsPriceBook_PriceBookVersionId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    ALTER TABLE setup."FgsPriceBook" DROP COLUMN IF EXISTS "PriceBookVersionId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook') THEN
    DELETE FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031175428_AddPriceBookVersionIdToFgsPriceBook';
    END IF;
END $EF$;

COMMIT;
