-- =============================================================================
-- Migration: 20260519120000_GloCatalogConstraints
-- Adds missing unique constraints, check constraints, and column defaults
-- for GloRole, GloSetupDescriptionType, GloSetupLaborRateType, GloStateProvince.
-- Script:   Database/Scripts/20260519120000_GloCatalogConstraints_Up.sql
-- Pair with: Database/Migrations/20260519120000_GloCatalogConstraints.cs
-- Run after Initial_Migration when tables were created without inline constraints.
-- =============================================================================

START TRANSACTION;

-- ---------------------------------------------------------------------------
-- GloRole
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."IX_GloRole_RoleCode";

ALTER TABLE dbo."GloRole"
    ALTER COLUMN "IsAssignable" SET DEFAULT TRUE,
    ALTER COLUMN "IsSystemRole" SET DEFAULT FALSE,
    ALTER COLUMN "SortOrder" SET DEFAULT 0,
    ALTER COLUMN "IsActive" SET DEFAULT TRUE,
    ALTER COLUMN "CreatedOn" SET DEFAULT (timezone('utc', now()));

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloRole'
          AND c.conname = 'UX_GloRole_RoleCode'
    ) THEN
        ALTER TABLE dbo."GloRole"
            ADD CONSTRAINT "UX_GloRole_RoleCode" UNIQUE ("RoleCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloRole'
          AND c.conname = 'CK_GloRole_RoleCode_NotEmpty'
    ) THEN
        ALTER TABLE dbo."GloRole"
            ADD CONSTRAINT "CK_GloRole_RoleCode_NotEmpty"
            CHECK (length(trim("RoleCode")) > 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloRole'
          AND c.conname = 'CK_GloRole_Name_NotEmpty'
    ) THEN
        ALTER TABLE dbo."GloRole"
            ADD CONSTRAINT "CK_GloRole_Name_NotEmpty"
            CHECK (length(trim("Name")) > 0);
    END IF;
END $EF$;

-- ---------------------------------------------------------------------------
-- GloSetupDescriptionType
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."IX_GloSetupDescriptionType_Code";

ALTER TABLE dbo."GloSetupDescriptionType"
    ALTER COLUMN "Id" SET DEFAULT gen_random_uuid(),
    ALTER COLUMN "IsActive" SET DEFAULT TRUE,
    ALTER COLUMN "CreatedOn" SET DEFAULT (timezone('utc', now()));

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloSetupDescriptionType'
          AND c.conname = 'UQ_GloSetupDescriptionType_Code'
    ) THEN
        ALTER TABLE dbo."GloSetupDescriptionType"
            ADD CONSTRAINT "UQ_GloSetupDescriptionType_Code" UNIQUE ("Code");
    END IF;
END $EF$;

-- ---------------------------------------------------------------------------
-- GloSetupLaborRateType
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."IX_GloSetupLaborRateType_Name";

ALTER TABLE dbo."GloSetupLaborRateType"
    ALTER COLUMN "SortOrder" SET DEFAULT 0,
    ALTER COLUMN "IsSystem" SET DEFAULT TRUE,
    ALTER COLUMN "IsActive" SET DEFAULT TRUE,
    ALTER COLUMN "CreatedOn" SET DEFAULT (timezone('utc', now()));

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloSetupLaborRateType'
          AND c.conname = 'UQ_GloSetupLaborRateType_Name'
    ) THEN
        ALTER TABLE dbo."GloSetupLaborRateType"
            ADD CONSTRAINT "UQ_GloSetupLaborRateType_Name" UNIQUE ("Name");
    END IF;
END $EF$;

-- ---------------------------------------------------------------------------
-- GloStateProvince (unique on country + code; FK already on initial migration)
-- ---------------------------------------------------------------------------
DROP INDEX IF EXISTS dbo."UQ_GloStateProvince";

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        INNER JOIN pg_class t ON t.oid = c.conrelid
        INNER JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'dbo'
          AND t.relname = 'GloStateProvince'
          AND c.conname = 'UQ_GloStateProvince'
    ) THEN
        ALTER TABLE dbo."GloStateProvince"
            ADD CONSTRAINT "UQ_GloStateProvince"
            UNIQUE ("CountryCode", "StateProvinceCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM dbo."__EFMigrationsHistory"
        WHERE "MigrationId" = '20260519120000_GloCatalogConstraints'
    ) THEN
        INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20260519120000_GloCatalogConstraints', '10.0.8');
    END IF;
END $EF$;

COMMIT;
