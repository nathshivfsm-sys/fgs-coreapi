-- Rollback for 20260713180610_AddIdentityAuthorizationAndApiEntities
START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsUserRole" DROP CONSTRAINT IF EXISTS "FK_FgsUserRole_FgsTenantCompanyCache";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsUser" DROP CONSTRAINT IF EXISTS "FK_FgsUser_FgsTenantCompanyCache";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsRole" DROP CONSTRAINT IF EXISTS "FK_FgsRole_ParentRole";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsRole" DROP CONSTRAINT IF EXISTS "FK_FgsRole_FgsTenantCompanyCache";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsApiRequestLog";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsApiSecret";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsApiWebhookSubscription";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsDataAccessScope";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsRoleDataAccess";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsRolePermission";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsApiClient";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsApiEvent";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsApiWebhook";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsDataAccess";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP TABLE IF EXISTS identity."FgsPermission";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP INDEX IF EXISTS identity."IX_FgsUserRole_TenantId_CompanyId_UserId_FgsRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP INDEX IF EXISTS identity."IX_FgsRole_ParentRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP INDEX IF EXISTS identity."IX_FgsRole_TenantId_CompanyId_IsBuiltIn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DROP INDEX IF EXISTS identity."IX_FgsRole_TenantId_CompanyId_Name";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsUserRole" DROP COLUMN IF EXISTS "CreatedBy";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsRole" DROP COLUMN IF EXISTS "DisplayOrder";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsRole" DROP COLUMN IF EXISTS "IsBuiltIn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsRole" DROP COLUMN IF EXISTS "ParentRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        COMMENT ON TABLE identity."FgsUserRole" IS NULL;
        COMMENT ON COLUMN identity."FgsUserRole"."CreatedOn" IS NULL;
        COMMENT ON COLUMN identity."FgsUserRole"."UserId" IS NULL;
        COMMENT ON COLUMN identity."FgsUserRole"."FgsRoleId" IS NULL;
        COMMENT ON COLUMN identity."FgsUserRole"."Id" IS NULL;
        COMMENT ON COLUMN identity."FgsUserRole"."TenantId" IS NULL;
        COMMENT ON COLUMN identity."FgsUserRole"."CompanyId" IS NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        COMMENT ON TABLE identity."FgsRole" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."Id" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."TenantId" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."CompanyId" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."RoleCode" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."Name" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."Description" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."IsActive" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."CreatedOn" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."CreatedBy" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."UpdatedOn" IS NULL;
        COMMENT ON COLUMN identity."FgsRole"."UpdatedBy" IS NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsUserRole" ALTER COLUMN "FgsRoleId" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsUserRole" ADD COLUMN IF NOT EXISTS "GloRoleId" smallint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsRole"
            ALTER COLUMN "IsActive" DROP DEFAULT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsRole" ADD COLUMN IF NOT EXISTS "GloRoleId" smallint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        CREATE INDEX IF NOT EXISTS "IX_FgsUserRole_GloRoleId"
            ON identity."FgsUserRole" ("GloRoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsUserRole_UserId_FgsRoleId"
            ON identity."FgsUserRole" ("UserId", "FgsRoleId")
            WHERE "FgsRoleId" IS NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsUserRole_UserId_GloRoleId"
            ON identity."FgsUserRole" ("UserId", "GloRoleId")
            WHERE "GloRoleId" IS NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        ALTER TABLE identity."FgsUserRole" DROP CONSTRAINT IF EXISTS "CK_FgsUserRole_OnlyOneRole";
        ALTER TABLE identity."FgsUserRole"
            ADD CONSTRAINT "CK_FgsUserRole_OnlyOneRole"
            CHECK (("GloRoleId" IS NOT NULL AND "FgsRoleId" IS NULL) OR ("GloRoleId" IS NULL AND "FgsRoleId" IS NOT NULL));
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        -- Preserve existing FgsUserRole indexes from initial migration
        CREATE INDEX IF NOT EXISTS "IX_FgsUserRole_UserId"
            ON identity."FgsUserRole" ("UserId");
        CREATE INDEX IF NOT EXISTS "IX_FgsUserRole_FgsRoleId"
            ON identity."FgsUserRole" ("FgsRoleId");
        CREATE INDEX IF NOT EXISTS "IX_FgsUserRole_TenantId_CompanyId"
            ON identity."FgsUserRole" ("TenantId", "CompanyId");
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_FgsRole_TenantId_CompanyId_RoleCode"
            ON identity."FgsRole" ("TenantId", "CompanyId", "RoleCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
        DELETE FROM identity."__EFMigrationsHistory"
        WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities';
    END IF;
END $EF$;

COMMIT;
