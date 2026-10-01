START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261001205445_AddFgsJobTypeAccounting') THEN
    DROP TABLE setup."FgsJobTypeAccounting";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261001205445_AddFgsJobTypeAccounting') THEN
    DROP TABLE setup."FgsJobTypeBillingCategoryAccounting";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20261001205445_AddFgsJobTypeAccounting') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20261001205445_AddFgsJobTypeAccounting';
    END IF;
END $EF$;
COMMIT;

