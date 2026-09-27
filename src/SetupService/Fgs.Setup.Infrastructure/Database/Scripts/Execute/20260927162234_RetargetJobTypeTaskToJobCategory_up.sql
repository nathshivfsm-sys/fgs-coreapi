START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER TABLE setup."FgsJobTypeTask" DROP CONSTRAINT "FK_FgsJobTypeTask_FgsJobTypeCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    DELETE FROM setup."FgsJobTypeTask" t
    WHERE NOT EXISTS (
        SELECT 1
        FROM setup."FgsJobCategory" jc
        WHERE jc."Id" = t."JobTypeCategoryId"
          AND jc."TenantId" = t."TenantId"
          AND jc."CompanyId" = t."CompanyId"
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER TABLE setup."FgsJobTypeTask" RENAME COLUMN "JobTypeCategoryId" TO "JobCategoryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER INDEX setup."IX_FgsJobTypeTask_JobTypeCategoryId" RENAME TO "IX_FgsJobTypeTask_JobCategoryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER INDEX setup."IX_FgsJobTypeTask_Tenant_Company_JobTypeCategory" RENAME TO "IX_FgsJobTypeTask_Tenant_Company_JobCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER INDEX setup."UX_FgsJobTypeTask_Tenant_Company_JobTypeCategory_Name" RENAME TO "UX_FgsJobTypeTask_Tenant_Company_JobCategory_Name";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    COMMENT ON TABLE setup."FgsJobTypeTask" IS 'Stores the tasks that belong to a Job Category (master catalog). Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    COMMENT ON COLUMN setup."FgsJobTypeTask"."JobCategoryId" IS 'Identifier of the Job Category (master catalog) that owns this task.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    COMMENT ON COLUMN setup."FgsJobTypeTask"."Name" IS 'Sub-category name unique within the parent Job Category.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    COMMENT ON COLUMN setup."FgsJobTypeTask"."DisplayOrder" IS 'Controls the display sequence of tasks within the Job Category.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    ALTER TABLE setup."FgsJobTypeTask" ADD CONSTRAINT "FK_FgsJobTypeTask_FgsJobCategory" FOREIGN KEY ("JobCategoryId") REFERENCES setup."FgsJobCategory" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260927162234_RetargetJobTypeTaskToJobCategory') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260927162234_RetargetJobTypeTaskToJobCategory', '10.0.8');
    END IF;
END $EF$;
COMMIT;

