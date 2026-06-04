DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'billing') THEN
        CREATE SCHEMA billing;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS billing."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260603212540_InitialSchema') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603212540_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'billing') THEN
            CREATE SCHEMA billing;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE TABLE billing."FgsTenantCompanyCache" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "CompanyGuid" uuid NOT NULL,
        "CompanyCode" character varying(100) NOT NULL,
        "CompanyName" character varying(200) NOT NULL,
        "IsActive" boolean NOT NULL,
        "UpdatedOn" timestamptz,
        CONSTRAINT "PK_FgsTenantCompanyCache" PRIMARY KEY ("TenantId", "CompanyId")
    );
    COMMENT ON TABLE billing."FgsTenantCompanyCache" IS 'Local cache of tenant company information used by the billing schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyId" IS 'Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyGuid" IS 'Globally unique company identifier used by integrations and external systems.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyCode" IS 'Unique company code within a tenant.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyName" IS 'Display name of the company.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."IsActive" IS 'Indicates whether the company is active.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."UpdatedOn" IS 'Timestamp of the most recent synchronization from tenant.FgsTenantCompany.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_CompanyName" ON billing."FgsTenantCompanyCache" ("CompanyName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_IsActive" ON billing."FgsTenantCompanyCache" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE UNIQUE INDEX "UQ_FgsTenantCompanyCache_CompanyGuid" ON billing."FgsTenantCompanyCache" ("CompanyGuid");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260604131918_AddFgsTenantCompanyCache', '10.0.8');
    END IF;
END $EF$;
COMMIT;

