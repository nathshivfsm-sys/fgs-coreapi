-- Rollback for 20260927162234_RetargetJobTypeTaskToJobCategory
START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER TABLE setup."FgsJobTypeTask" DROP CONSTRAINT IF EXISTS "FK_FgsJobTypeTask_FgsJobCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    DELETE FROM setup."FgsJobTypeTask" t
    WHERE NOT EXISTS (
        SELECT 1
        FROM setup."FgsJobTypeCategory" jtc
        WHERE jtc."Id" = t."JobCategoryId"
          AND jtc."TenantId" = t."TenantId"
          AND jtc."CompanyId" = t."CompanyId"
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER TABLE setup."FgsJobTypeTask" RENAME COLUMN "JobCategoryId" TO "JobTypeCategoryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER INDEX setup."IX_FgsJobTypeTask_JobCategoryId" RENAME TO "IX_FgsJobTypeTask_JobTypeCategoryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER INDEX setup."IX_FgsJobTypeTask_Tenant_Company_JobCategory" RENAME TO "IX_FgsJobTypeTask_Tenant_Company_JobTypeCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER INDEX setup."UX_FgsJobTypeTask_Tenant_Company_JobCategory_Name" RENAME TO "UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    COMMENT ON TABLE setup."FgsJobTypeTask" IS 'Stores the tasks that belong to a Job Type Category. Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours.';
    COMMENT ON COLUMN setup."FgsJobTypeTask"."JobTypeCategoryId" IS 'Identifier of the Job Type Category that owns this task.';
    COMMENT ON COLUMN setup."FgsJobTypeTask"."Name" IS 'Sub-category name unique within the parent Job Type Category.';
    COMMENT ON COLUMN setup."FgsJobTypeTask"."DisplayOrder" IS 'Controls the display sequence of tasks within the Job Type Category.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER TABLE setup."FgsJobTypeTask" ADD CONSTRAINT "FK_FgsJobTypeTask_FgsJobTypeCategory" FOREIGN KEY ("JobTypeCategoryId") REFERENCES setup."FgsJobTypeCategory" ("Id") ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory';
    END IF;
END $EF$;
COMMIT;
