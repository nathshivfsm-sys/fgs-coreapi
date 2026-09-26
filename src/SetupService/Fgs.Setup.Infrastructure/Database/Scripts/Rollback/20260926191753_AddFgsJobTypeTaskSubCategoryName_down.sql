-- Rollback for 20260926191753_AddFgsJobTypeTaskSubCategoryName
START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    DROP INDEX IF EXISTS setup."UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    ALTER TABLE setup."FgsJobTypeTask" DROP COLUMN IF EXISTS "Name";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    ALTER TABLE setup."FgsJobTypeTask"
        ALTER COLUMN "TaskName" TYPE character varying(200);
    COMMENT ON COLUMN setup."FgsJobTypeTask"."TaskName"
        IS 'Name of the task to be performed.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName';
    END IF;
END $EF$;
COMMIT;
