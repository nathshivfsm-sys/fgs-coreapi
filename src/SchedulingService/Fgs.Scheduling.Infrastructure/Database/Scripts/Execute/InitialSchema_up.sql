DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dispatch') THEN
        CREATE SCHEMA dispatch;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS dispatch."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260603213051_InitialSchema') THEN
    INSERT INTO dispatch."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603213051_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

