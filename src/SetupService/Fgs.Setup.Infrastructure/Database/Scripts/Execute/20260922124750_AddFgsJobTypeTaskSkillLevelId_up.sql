START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    ALTER TABLE setup."FgsJobTypeTask" ADD "SkillLevelId" bigint;
    COMMENT ON COLUMN setup."FgsJobTypeTask"."SkillLevelId"
        IS 'Optional skill level required to perform this task.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    CREATE INDEX "IX_FgsJobTypeTask_SkillLevelId" ON setup."FgsJobTypeTask" ("SkillLevelId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    ALTER TABLE setup."FgsJobTypeTask" ADD CONSTRAINT "FK_FgsJobTypeTask_FgsSetupTechSkillLevel"
        FOREIGN KEY ("SkillLevelId") REFERENCES setup."FgsSetupTechSkillLevel" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260922124750_AddFgsJobTypeTaskSkillLevelId') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260922124750_AddFgsJobTypeTaskSkillLevelId', '10.0.8');
    END IF;
END $EF$;
COMMIT;
