START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260724175100_AddFgsUserAuthenticationMethod') THEN
        ALTER TABLE identity."FgsUser"
            ADD COLUMN IF NOT EXISTS "AuthenticationMethod" smallint NOT NULL DEFAULT 3;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260724175100_AddFgsUserAuthenticationMethod') THEN
        ALTER TABLE identity."FgsUser"
            DROP CONSTRAINT IF EXISTS "CK_FgsUser_AuthenticationMethod";

        ALTER TABLE identity."FgsUser"
            ADD CONSTRAINT "CK_FgsUser_AuthenticationMethod"
            CHECK ("AuthenticationMethod" IN (1, 2, 3, 4, 5));
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260724175100_AddFgsUserAuthenticationMethod') THEN
        INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20260724175100_AddFgsUserAuthenticationMethod', '10.0.8');
    END IF;
END $EF$;

COMMIT;
