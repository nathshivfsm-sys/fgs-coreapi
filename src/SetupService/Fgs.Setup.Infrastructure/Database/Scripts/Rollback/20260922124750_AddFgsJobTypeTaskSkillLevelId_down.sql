-- Rollback for 20260922124750_AddFgsJobTypeTaskSkillLevelId
START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    ALTER TABLE setup."FgsJobTypeTask" DROP CONSTRAINT IF EXISTS "FK_FgsJobTypeTask_FgsSetupTechSkillLevel";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    DROP INDEX IF EXISTS setup."IX_FgsJobTypeTask_SkillLevelId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    ALTER TABLE setup."FgsJobTypeTask" DROP COLUMN IF EXISTS "SkillLevelId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId';
    END IF;
END $EF$;
COMMIT;
