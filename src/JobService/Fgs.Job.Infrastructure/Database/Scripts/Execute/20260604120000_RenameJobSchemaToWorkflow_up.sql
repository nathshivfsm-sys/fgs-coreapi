DO $rename$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'job')
       AND NOT EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'workflow') THEN
        ALTER SCHEMA job RENAME TO workflow;
    ELSIF EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'job')
       AND EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'workflow') THEN
        DROP SCHEMA job CASCADE;
    END IF;
END
$rename$;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM workflow."__EFMigrationsHistory" WHERE "MigrationId" = '20260604120000_RenameJobSchemaToWorkflow') THEN
    INSERT INTO workflow."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260604120000_RenameJobSchemaToWorkflow', '10.0.8');
    END IF;
END $EF$;
COMMIT;
