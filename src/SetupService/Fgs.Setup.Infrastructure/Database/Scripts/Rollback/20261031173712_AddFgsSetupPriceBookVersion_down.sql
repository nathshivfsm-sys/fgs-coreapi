START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031173712_AddFgsSetupPriceBookVersion') THEN
    DROP TABLE IF EXISTS setup."FgsSetupPriceBookVersion";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031173712_AddFgsSetupPriceBookVersion') THEN
    DELETE FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261031173712_AddFgsSetupPriceBookVersion';
    END IF;
END $EF$;

COMMIT;
