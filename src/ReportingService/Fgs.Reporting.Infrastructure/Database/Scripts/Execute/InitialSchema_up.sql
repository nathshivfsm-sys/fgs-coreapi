DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'reporting') THEN
        CREATE SCHEMA reporting;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS reporting."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM reporting."__EFMigrationsHistory" WHERE "MigrationId" = '20260603214016_InitialSchema') THEN
    INSERT INTO reporting."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603214016_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

