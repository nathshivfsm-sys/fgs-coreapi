-- =============================================================================
-- Revert: 20260519120000_GloCatalogConstraints
-- =============================================================================

START TRANSACTION;

ALTER TABLE dbo."GloStateProvince"
    DROP CONSTRAINT IF EXISTS "UQ_GloStateProvince";

CREATE UNIQUE INDEX IF NOT EXISTS "UQ_GloStateProvince"
    ON dbo."GloStateProvince" ("CountryCode", "StateProvinceCode");

ALTER TABLE dbo."GloSetupLaborRateType"
    DROP CONSTRAINT IF EXISTS "UQ_GloSetupLaborRateType_Name";

ALTER TABLE dbo."GloSetupLaborRateType"
    ALTER COLUMN "SortOrder" DROP DEFAULT,
    ALTER COLUMN "IsSystem" DROP DEFAULT,
    ALTER COLUMN "IsActive" DROP DEFAULT,
    ALTER COLUMN "CreatedOn" DROP DEFAULT;

CREATE UNIQUE INDEX IF NOT EXISTS "IX_GloSetupLaborRateType_Name"
    ON dbo."GloSetupLaborRateType" ("Name");

ALTER TABLE dbo."GloSetupDescriptionType"
    DROP CONSTRAINT IF EXISTS "UQ_GloSetupDescriptionType_Code";

ALTER TABLE dbo."GloSetupDescriptionType"
    ALTER COLUMN "Id" DROP DEFAULT,
    ALTER COLUMN "IsActive" DROP DEFAULT,
    ALTER COLUMN "CreatedOn" DROP DEFAULT;

CREATE UNIQUE INDEX IF NOT EXISTS "IX_GloSetupDescriptionType_Code"
    ON dbo."GloSetupDescriptionType" ("Code");

ALTER TABLE dbo."GloRole"
    DROP CONSTRAINT IF EXISTS "CK_GloRole_Name_NotEmpty",
    DROP CONSTRAINT IF EXISTS "CK_GloRole_RoleCode_NotEmpty",
    DROP CONSTRAINT IF EXISTS "UX_GloRole_RoleCode";

ALTER TABLE dbo."GloRole"
    ALTER COLUMN "IsAssignable" DROP DEFAULT,
    ALTER COLUMN "IsSystemRole" DROP DEFAULT,
    ALTER COLUMN "SortOrder" DROP DEFAULT,
    ALTER COLUMN "IsActive" DROP DEFAULT,
    ALTER COLUMN "CreatedOn" DROP DEFAULT;

CREATE UNIQUE INDEX IF NOT EXISTS "IX_GloRole_RoleCode"
    ON dbo."GloRole" ("RoleCode");

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260519120000_GloCatalogConstraints';

COMMIT;
