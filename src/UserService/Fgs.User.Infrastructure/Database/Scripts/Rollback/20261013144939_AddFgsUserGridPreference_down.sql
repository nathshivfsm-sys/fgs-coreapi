START TRANSACTION;

DROP TABLE IF EXISTS identity."FgsUserGridPreference";

ALTER TABLE identity."FgsUser" DROP CONSTRAINT IF EXISTS "AK_FgsUser_Id_TenantId_CompanyId";

DELETE FROM identity."__EFMigrationsHistory"
WHERE "MigrationId" = '20261013144939_AddFgsUserGridPreference';

COMMIT;
