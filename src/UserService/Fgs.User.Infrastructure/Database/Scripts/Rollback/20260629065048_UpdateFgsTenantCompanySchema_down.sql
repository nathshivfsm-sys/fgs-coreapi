-- Rollback for 20260629065048_UpdateFgsTenantCompanySchema
START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
        ALTER TABLE tenant."FgsTenantCompany" DROP COLUMN IF EXISTS "TimeZone";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
        ALTER TABLE tenant."FgsTenantCompany"
        ALTER COLUMN "FullLogoUrl" TYPE character varying(1000)
        USING "FullLogoUrl"::text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
        ALTER TABLE tenant."FgsTenantCompany"
        ALTER COLUMN "CompactLogoUrl" TYPE character varying(1000)
        USING "CompactLogoUrl"::text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
        ALTER TABLE tenant."FgsTenantCompany"
        ADD COLUMN IF NOT EXISTS "BusinessTypeId" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
        ALTER TABLE tenant."FgsTenantCompany"
        ADD COLUMN IF NOT EXISTS "FaviconUrl" character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
        ALTER TABLE tenant."FgsTenantCompany"
        ADD COLUMN IF NOT EXISTS "IconLogoUrl" character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
        DELETE FROM identity."__EFMigrationsHistory"
        WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema';
    END IF;
END $EF$;

COMMIT;
