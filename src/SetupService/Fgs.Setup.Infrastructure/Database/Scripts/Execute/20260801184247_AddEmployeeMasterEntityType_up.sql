DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'glo') THEN
        CREATE SCHEMA glo;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS setup."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260801184247_AddEmployeeMasterEntityType') THEN
    INSERT INTO glo."GloMasterEntityType"
    (
        "Code",
        "IsDocumentAllowed",
        "IsActive",
        "SortOrder",
        "CreatedOn",
        "CreatedBy"
    )
    SELECT
        'EMPLOYEE',
        TRUE,
        TRUE,
        15,
        timezone('utc', now()),
        'System'
    WHERE NOT EXISTS (
        SELECT 1
        FROM glo."GloMasterEntityType" t
        WHERE t."Code" = 'EMPLOYEE'
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260801184247_AddEmployeeMasterEntityType') THEN
    INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260801184247_AddEmployeeMasterEntityType', '10.0.8');
    END IF;
END $EF$;
COMMIT;
