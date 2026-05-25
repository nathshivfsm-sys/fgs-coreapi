-- =============================================================================
-- Migration: 20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole
-- Script:   20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole_up.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    ALTER TABLE dbo."GloSubCategory" RENAME TO "GloJobTypeSubCategory";
    ALTER TABLE dbo."GloCategory" RENAME TO "GloJobTypeCategory";
    ALTER TABLE dbo."GloCategorySubCategory" RENAME TO "GloJobTypeCategorySubCategory";

    ALTER TABLE dbo."GloJobTypeSubCategory" RENAME CONSTRAINT "PK_GloSubCategory" TO "PK_GloJobTypeSubCategory";
    ALTER INDEX dbo."UQ_GloSubCategory_Code" RENAME TO "UQ_GloJobTypeSubCategory_Code";
    ALTER TABLE dbo."GloJobTypeSubCategory" RENAME CONSTRAINT "CK_GloSubCategory_Code_Upper" TO "CK_GloJobTypeSubCategory_Code_Upper";

    ALTER TABLE dbo."GloJobTypeCategory" RENAME CONSTRAINT "PK_GloCategory" TO "PK_GloJobTypeCategory";
    ALTER INDEX dbo."UQ_GloCategory_BusinessTypeId_Code" RENAME TO "UQ_GloJobTypeCategory_BusinessTypeId_Code";
    ALTER TABLE dbo."GloJobTypeCategory" RENAME CONSTRAINT "CK_GloCategory_Code_Upper" TO "CK_GloJobTypeCategory_Code_Upper";
    ALTER TABLE dbo."GloJobTypeCategory" RENAME CONSTRAINT "FK_GloCategory_GloBusinessType_BusinessTypeId" TO "FK_GloJobTypeCategory_GloBusinessType_BusinessTypeId";

    ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "PK_GloCategorySubCategory" TO "PK_GloJobTypeCategorySubCategory";
    ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "FK_GloCategorySubCategory_GloBusinessType_BusinessTypeId" TO "FK_GloJobTypeCategorySubCategory_GloBusinessType_BusinessTypeId";
    ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "FK_GloCategorySubCategory_GloCategory_CategoryId" TO "FK_GloJobTypeCategorySubCategory_GloJobTypeCategory_CategoryId";
    ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "FK_GloCategorySubCategory_GloSubCategory_SubCategoryId" TO "FK_GloJobTypeCategorySubCategory_GloJobTypeSubCategory_SubCategoryId";
    ALTER INDEX dbo."IX_GloCategorySubCategory_CategoryId" RENAME TO "IX_GloJobTypeCategorySubCategory_CategoryId";
    ALTER INDEX dbo."IX_GloCategorySubCategory_SubCategoryId" RENAME TO "IX_GloJobTypeCategorySubCategory_SubCategoryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    INSERT INTO dbo."FgsUserRole" ("UserId", "TenantId", "CompanyId", "GloRoleId", "CreatedOn")
    SELECT u."Id", u."TenantId", u."CompanyId", r."Id", NOW()
    FROM dbo."FgsUser" u
    INNER JOIN dbo."GloRole" r ON r."RoleCode" = 'TENANT_ADMIN'
    WHERE u."Role" = 'Admin'
      AND NOT EXISTS (
          SELECT 1
          FROM dbo."FgsUserRole" ur
          WHERE ur."UserId" = u."Id"
            AND ur."GloRoleId" = r."Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    ALTER TABLE dbo."FgsUser" DROP COLUMN "Role";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE TABLE dbo."FgsEntityTag" (
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "TagId" bigint NOT NULL,
        "MasterEntityTypeId" integer NOT NULL,
        "EntityId" bigint NOT NULL,
        "Notes" character varying(500),
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint,
        CONSTRAINT "PK_FgsEntityTag" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsEntityTag_FgsTag_TagId" FOREIGN KEY ("TagId") REFERENCES dbo."FgsTag" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsEntityTag_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsEntityTag_GloMasterEntityType_MasterEntityTypeId" FOREIGN KEY ("MasterEntityTypeId") REFERENCES dbo."GloMasterEntityType" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE TABLE dbo."FgsTagEntityType" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "TagId" bigint NOT NULL,
        "MasterEntityTypeId" integer NOT NULL,
        "IsDefault" boolean NOT NULL DEFAULT FALSE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint,
        CONSTRAINT "PK_FgsTagEntityType" PRIMARY KEY ("TenantId", "CompanyId", "TagId", "MasterEntityTypeId"),
        CONSTRAINT "FK_FgsTagEntityType_FgsTag_TagId" FOREIGN KEY ("TagId") REFERENCES dbo."FgsTag" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsTagEntityType_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsTagEntityType_GloMasterEntityType_MasterEntityTypeId" FOREIGN KEY ("MasterEntityTypeId") REFERENCES dbo."GloMasterEntityType" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE INDEX "IX_FgsEntityTag_CreatedOn" ON dbo."FgsEntityTag" ("CreatedOn" DESC);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE INDEX "IX_FgsEntityTag_Entity" ON dbo."FgsEntityTag" ("TenantId", "CompanyId", "MasterEntityTypeId", "EntityId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE INDEX "IX_FgsEntityTag_MasterEntityTypeId" ON dbo."FgsEntityTag" ("TenantId", "CompanyId", "MasterEntityTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE INDEX "IX_FgsEntityTag_TagId" ON dbo."FgsEntityTag" ("TenantId", "CompanyId", "TagId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE UNIQUE INDEX "UX_FgsEntityTag_TenantId_CompanyId_TagId_MasterEntityTypeId_EntityId" ON dbo."FgsEntityTag" ("TenantId", "CompanyId", "TagId", "MasterEntityTypeId", "EntityId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE INDEX "IX_FgsTagEntityType_IsDefault" ON dbo."FgsTagEntityType" ("TenantId", "CompanyId", "MasterEntityTypeId", "IsDefault");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE INDEX "IX_FgsTagEntityType_MasterEntityTypeId" ON dbo."FgsTagEntityType" ("TenantId", "CompanyId", "MasterEntityTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    CREATE INDEX "IX_FgsTagEntityType_TagId" ON dbo."FgsTagEntityType" ("TenantId", "CompanyId", "TagId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole', '10.0.8');
    END IF;
END $EF$;
COMMIT;

