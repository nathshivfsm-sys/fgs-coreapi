-- Rollback for 20260724175100_AddFgsUserAuthenticationMethod
START TRANSACTION;

ALTER TABLE identity."FgsUser"
    DROP CONSTRAINT IF EXISTS "CK_FgsUser_AuthenticationMethod";

ALTER TABLE identity."FgsUser"
    DROP COLUMN IF EXISTS "AuthenticationMethod";

DELETE FROM identity."__EFMigrationsHistory"
WHERE "MigrationId" = '20260724175100_AddFgsUserAuthenticationMethod';

COMMIT;
