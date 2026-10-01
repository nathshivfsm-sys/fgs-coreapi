START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200650_AddFgsBillingCategoryAccounting') THEN
    DROP TABLE setup."FgsBillingCategoryAccounting";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261001200650_AddFgsBillingCategoryAccounting') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261001200650_AddFgsBillingCategoryAccounting';
    END IF;
END $EF$;
COMMIT;

