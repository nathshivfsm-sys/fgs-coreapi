START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    ALTER TABLE setup."FgsJobTypeTask"
        ALTER COLUMN "TaskName" TYPE character varying(350);
    COMMENT ON COLUMN setup."FgsJobTypeTask"."TaskName"
        IS 'Name of the task to be performed. Defaults to category name plus a space plus the sub-category name.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    ALTER TABLE setup."FgsJobTypeTask" ADD "Name" character varying(150) NOT NULL DEFAULT '';
    COMMENT ON COLUMN setup."FgsJobTypeTask"."Name"
        IS 'Sub-category name unique within the parent Job Type Category.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    CREATE UNIQUE INDEX "UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name"
        ON setup."FgsJobTypeTask" ("TenantId", "CompanyId", "JobTypeCategoryId", "Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260926191753_AddFgsJobTypeTaskSubCategoryName') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260926191753_AddFgsJobTypeTaskSubCategoryName', '10.0.8');
    END IF;
END $EF$;
COMMIT;
