DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'integration') THEN
        CREATE SCHEMA integration;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS integration."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260604133449_AddFgsTenantCompanyCache') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'integration') THEN
            CREATE SCHEMA integration;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260604133449_AddFgsTenantCompanyCache') THEN
    CREATE TABLE integration."FgsTenantCompanyCache" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "CompanyGuid" uuid NOT NULL,
        "CompanyCode" character varying(100) NOT NULL,
        "CompanyName" character varying(200) NOT NULL,
        "IsActive" boolean NOT NULL,
        "UpdatedOn" timestamptz,
        CONSTRAINT "PK_FgsTenantCompanyCache" PRIMARY KEY ("TenantId", "CompanyId")
    );
    COMMENT ON TABLE integration."FgsTenantCompanyCache" IS 'Local cache of tenant company information used by the integration schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.';
    COMMENT ON COLUMN integration."FgsTenantCompanyCache"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN integration."FgsTenantCompanyCache"."CompanyId" IS 'Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber.';
    COMMENT ON COLUMN integration."FgsTenantCompanyCache"."CompanyGuid" IS 'Globally unique company identifier used by integrations and external systems.';
    COMMENT ON COLUMN integration."FgsTenantCompanyCache"."CompanyCode" IS 'Unique company code within a tenant.';
    COMMENT ON COLUMN integration."FgsTenantCompanyCache"."CompanyName" IS 'Display name of the company.';
    COMMENT ON COLUMN integration."FgsTenantCompanyCache"."IsActive" IS 'Indicates whether the company is active.';
    COMMENT ON COLUMN integration."FgsTenantCompanyCache"."UpdatedOn" IS 'Timestamp of the most recent synchronization from tenant.FgsTenantCompany.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260604133449_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_CompanyName" ON integration."FgsTenantCompanyCache" ("CompanyName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260604133449_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_IsActive" ON integration."FgsTenantCompanyCache" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260604133449_AddFgsTenantCompanyCache') THEN
    CREATE UNIQUE INDEX "UQ_FgsTenantCompanyCache_CompanyGuid" ON integration."FgsTenantCompanyCache" ("CompanyGuid");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260604133449_AddFgsTenantCompanyCache') THEN
    INSERT INTO integration."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260604133449_AddFgsTenantCompanyCache', '10.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260617162140_AddFgsPaymentTransactionPayload') THEN
    CREATE TABLE integration."FgsPaymentTransactionPayload" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "PaymentTransactionId" bigint NOT NULL,
        "RequestJson" jsonb,
        "ResponseJson" jsonb,
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        CONSTRAINT "PK_FgsPaymentTransactionPayload" PRIMARY KEY ("Id")
    );
    COMMENT ON TABLE integration."FgsPaymentTransactionPayload" IS 'Stores optional payment processor request and response payloads for troubleshooting, support, auditing, and integration diagnostics.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260617162140_AddFgsPaymentTransactionPayload') THEN
    CREATE INDEX "IX_FgsPaymentTransactionPayload_TenantCompany" ON integration."FgsPaymentTransactionPayload" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260617162140_AddFgsPaymentTransactionPayload') THEN
    CREATE UNIQUE INDEX "UX_FgsPaymentTransactionPayload_PaymentTransaction" ON integration."FgsPaymentTransactionPayload" ("PaymentTransactionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260617162140_AddFgsPaymentTransactionPayload') THEN
    ALTER TABLE integration."FgsPaymentTransactionPayload"
        ADD CONSTRAINT "FK_FgsPaymentTransactionPayload_TenantCompany"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId")
        ON DELETE RESTRICT;

    ALTER TABLE integration."FgsPaymentTransactionPayload"
        ADD CONSTRAINT "FK_FgsPaymentTransactionPayload_PaymentTransaction"
        FOREIGN KEY ("PaymentTransactionId")
        REFERENCES billing."FgsPaymentTransaction" ("Id")
        ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM integration."__EFMigrationsHistory" WHERE "MigrationId" = '20260617162140_AddFgsPaymentTransactionPayload') THEN
    INSERT INTO integration."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260617162140_AddFgsPaymentTransactionPayload', '10.0.8');
    END IF;
END $EF$;
COMMIT;

