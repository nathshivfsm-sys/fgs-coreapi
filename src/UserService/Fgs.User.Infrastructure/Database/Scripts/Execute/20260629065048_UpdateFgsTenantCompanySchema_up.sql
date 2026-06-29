START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
    ALTER TABLE tenant."FgsTenantCompany" DROP COLUMN "BusinessTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
    ALTER TABLE tenant."FgsTenantCompany" DROP COLUMN "FaviconUrl";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
    ALTER TABLE tenant."FgsTenantCompany" DROP COLUMN "IconLogoUrl";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
    ALTER TABLE tenant."FgsTenantCompany"
    ALTER COLUMN "FullLogoUrl" TYPE bigint
    USING "FullLogoUrl"::bigint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
    ALTER TABLE tenant."FgsTenantCompany"
    ALTER COLUMN "CompactLogoUrl" TYPE bigint
    USING "CompactLogoUrl"::bigint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
    ALTER TABLE tenant."FgsTenantCompany" ADD "TimeZone" character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260629065048_UpdateFgsTenantCompanySchema') THEN
    INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260629065048_UpdateFgsTenantCompanySchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

