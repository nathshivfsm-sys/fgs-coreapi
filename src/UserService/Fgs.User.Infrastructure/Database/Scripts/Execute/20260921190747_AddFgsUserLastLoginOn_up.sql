START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260921190747_AddFgsUserLastLoginOn') THEN
    ALTER TABLE identity."FgsUser" ADD "LastLoginOn" timestamp with time zone;
    COMMENT ON COLUMN identity."FgsUser"."LastLoginOn" IS 'UTC timestamp of the user''s most recent successful login.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260921190747_AddFgsUserLastLoginOn') THEN
    INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260921190747_AddFgsUserLastLoginOn', '10.0.8');
    END IF;
END $EF$;
COMMIT;
