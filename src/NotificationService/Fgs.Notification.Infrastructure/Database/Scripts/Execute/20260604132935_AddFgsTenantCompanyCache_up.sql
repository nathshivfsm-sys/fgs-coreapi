DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'notification') THEN
        CREATE SCHEMA notification;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS notification."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260603222551_InitialSchema') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'notification') THEN
            CREATE SCHEMA notification;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260603222551_InitialSchema') THEN
    CREATE TABLE notification."FgsNotificationHistory" (
        "Id" uuid NOT NULL,
        "TenantId" bigint NOT NULL,
        "Channel" integer NOT NULL,
        "TemplateName" character varying(128) NOT NULL,
        "Recipient" character varying(512),
        "Status" integer NOT NULL,
        "CorrelationId" character varying(64),
        "ProviderMessageId" character varying(256),
        "Error" character varying(2000),
        "CreatedOn" timestamptz NOT NULL,
        "SentOn" timestamptz,
        CONSTRAINT "PK_FgsNotificationHistory" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260603222551_InitialSchema') THEN
    CREATE TABLE notification."FgsProcessedIntegrationEvent" (
        "Id" uuid NOT NULL,
        "MessageId" character varying(128) NOT NULL,
        "EventType" character varying(128) NOT NULL,
        "ProcessedOn" timestamptz NOT NULL,
        CONSTRAINT "PK_FgsProcessedIntegrationEvent" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260603222551_InitialSchema') THEN
    CREATE INDEX "IX_FgsNotificationHistory_TenantId_CreatedOn" ON notification."FgsNotificationHistory" ("TenantId", "CreatedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260603222551_InitialSchema') THEN
    CREATE UNIQUE INDEX "IX_FgsProcessedIntegrationEvent_MessageId" ON notification."FgsProcessedIntegrationEvent" ("MessageId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260603222551_InitialSchema') THEN
    INSERT INTO notification."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603222551_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132935_AddFgsTenantCompanyCache') THEN
    CREATE TABLE notification."FgsTenantCompanyCache" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "CompanyGuid" uuid NOT NULL,
        "CompanyCode" character varying(100) NOT NULL,
        "CompanyName" character varying(200) NOT NULL,
        "IsActive" boolean NOT NULL,
        "UpdatedOn" timestamptz,
        CONSTRAINT "PK_FgsTenantCompanyCache" PRIMARY KEY ("TenantId", "CompanyId")
    );
    COMMENT ON TABLE notification."FgsTenantCompanyCache" IS 'Local cache of tenant company information used by the notification schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.';
    COMMENT ON COLUMN notification."FgsTenantCompanyCache"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN notification."FgsTenantCompanyCache"."CompanyId" IS 'Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber.';
    COMMENT ON COLUMN notification."FgsTenantCompanyCache"."CompanyGuid" IS 'Globally unique company identifier used by integrations and external systems.';
    COMMENT ON COLUMN notification."FgsTenantCompanyCache"."CompanyCode" IS 'Unique company code within a tenant.';
    COMMENT ON COLUMN notification."FgsTenantCompanyCache"."CompanyName" IS 'Display name of the company.';
    COMMENT ON COLUMN notification."FgsTenantCompanyCache"."IsActive" IS 'Indicates whether the company is active.';
    COMMENT ON COLUMN notification."FgsTenantCompanyCache"."UpdatedOn" IS 'Timestamp of the most recent synchronization from tenant.FgsTenantCompany.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132935_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_CompanyName" ON notification."FgsTenantCompanyCache" ("CompanyName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132935_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_IsActive" ON notification."FgsTenantCompanyCache" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132935_AddFgsTenantCompanyCache') THEN
    CREATE UNIQUE INDEX "UQ_FgsTenantCompanyCache_CompanyGuid" ON notification."FgsTenantCompanyCache" ("CompanyGuid");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132935_AddFgsTenantCompanyCache') THEN
    INSERT INTO notification."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260604132935_AddFgsTenantCompanyCache', '10.0.8');
    END IF;
END $EF$;
COMMIT;

