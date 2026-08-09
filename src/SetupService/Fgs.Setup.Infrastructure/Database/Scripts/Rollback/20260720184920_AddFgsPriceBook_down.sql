-- Rollback for 20260720184920_AddFgsPriceBook

START TRANSACTION;

DROP TABLE IF EXISTS setup."FgsPriceBookItem";
DROP TABLE IF EXISTS setup."FgsPriceBook";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260720184920_AddFgsPriceBook';

COMMIT;
