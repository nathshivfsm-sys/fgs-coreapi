START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260526140737_AddFgsJobTypeTables') THEN
    DROP TABLE dbo."FgsJobType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260526140737_AddFgsJobTypeTables') THEN
    DROP TABLE dbo."FgsJobTypeCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260526140737_AddFgsJobTypeTables') THEN
    DROP TABLE dbo."FgsJobTypeSubCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260526140737_AddFgsJobTypeTables') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260526140737_AddFgsJobTypeTables';
    END IF;
END $EF$;
COMMIT;

