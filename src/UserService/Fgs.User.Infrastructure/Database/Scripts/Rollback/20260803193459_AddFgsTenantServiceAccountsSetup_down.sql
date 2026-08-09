START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup') THEN
    DROP TABLE IF EXISTS tenant."FgsTenantServiceAccountsSetup";

    DROP INDEX IF EXISTS identity."IX_FgsUser_TenantId_CompanyId_Email";

    CREATE INDEX IF NOT EXISTS "IX_FgsUser_TenantId_CompanyId"
    ON identity."FgsUser" ("TenantId", "CompanyId");

    CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsUser_TenantId_Email"
    ON identity."FgsUser" ("TenantId", "Email")
    WHERE "IsDeleted" = false;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup') THEN
    DELETE FROM identity."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup';
    END IF;
END $EF$;
COMMIT;
