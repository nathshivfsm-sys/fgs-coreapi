-- =============================================================================
-- Migration: 20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole
-- Script:   20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole_down.sql
-- =============================================================================

START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    DROP TABLE IF EXISTS dbo."FgsEntityTag";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    DROP TABLE IF EXISTS dbo."FgsTagEntityType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    ALTER TABLE dbo."FgsUser" ADD "Role" character varying(50) NOT NULL DEFAULT 'Admin';
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    ALTER TABLE dbo."GloJobTypeSubCategory" RENAME TO "GloSubCategory";
    ALTER TABLE dbo."GloJobTypeCategory" RENAME TO "GloCategory";
    ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME TO "GloCategorySubCategory";

    ALTER TABLE dbo."GloSubCategory" RENAME CONSTRAINT "PK_GloJobTypeSubCategory" TO "PK_GloSubCategory";
    ALTER INDEX dbo."UQ_GloJobTypeSubCategory_Code" RENAME TO "UQ_GloSubCategory_Code";
    ALTER TABLE dbo."GloSubCategory" RENAME CONSTRAINT "CK_GloJobTypeSubCategory_Code_Upper" TO "CK_GloSubCategory_Code_Upper";

    ALTER TABLE dbo."GloCategory" RENAME CONSTRAINT "PK_GloJobTypeCategory" TO "PK_GloCategory";
    ALTER INDEX dbo."UQ_GloJobTypeCategory_BusinessTypeId_Code" RENAME TO "UQ_GloCategory_BusinessTypeId_Code";
    ALTER TABLE dbo."GloCategory" RENAME CONSTRAINT "CK_GloJobTypeCategory_Code_Upper" TO "CK_GloCategory_Code_Upper";
    ALTER TABLE dbo."GloCategory" RENAME CONSTRAINT "FK_GloJobTypeCategory_GloBusinessType_BusinessTypeId" TO "FK_GloCategory_GloBusinessType_BusinessTypeId";

    ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "PK_GloJobTypeCategorySubCategory" TO "PK_GloCategorySubCategory";
    ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloBusinessType_BusinessTypeId" TO "FK_GloCategorySubCategory_GloBusinessType_BusinessTypeId";
    ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloJobTypeCategory_CategoryId" TO "FK_GloCategorySubCategory_GloCategory_CategoryId";
    ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloJobTypeSubCategory_SubCategoryId" TO "FK_GloCategorySubCategory_GloSubCategory_SubCategoryId";
    ALTER INDEX dbo."IX_GloJobTypeCategorySubCategory_CategoryId" RENAME TO "IX_GloCategorySubCategory_CategoryId";
    ALTER INDEX dbo."IX_GloJobTypeCategorySubCategory_SubCategoryId" RENAME TO "IX_GloCategorySubCategory_SubCategoryId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260524050947_RenameJobTypeCategoriesAddEntityTagRemoveUserRole';
    END IF;
END $EF$;
COMMIT;

