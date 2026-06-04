DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'job') THEN
        CREATE SCHEMA job;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS job."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM job."__EFMigrationsHistory" WHERE "MigrationId" = '20260603214323_InitialSchema') THEN
    INSERT INTO job."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603214323_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

