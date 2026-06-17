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

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory') THEN
    CREATE TABLE notification."FgsEmailHistory" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "EntityType" character varying(50) NOT NULL,
        "EntityId" bigint NOT NULL,
        "EmailTemplateId" bigint,
        "Subject" character varying(500) NOT NULL,
        "FromEmailAddress" character varying(500) NOT NULL,
        "FromDisplayName" character varying(255),
        "ToEmailAddresses" jsonb NOT NULL,
        "CcEmailAddresses" jsonb,
        "BccEmailAddresses" jsonb,
        "BodyHtml" text,
        "BodyText" text,
        "HasAttachments" boolean NOT NULL DEFAULT FALSE,
        "Status" character varying(50) NOT NULL,
        "SentOn" timestamptz,
        "FailureReason" text,
        "ProviderMessageId" character varying(500),
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" text,
        CONSTRAINT "PK_FgsEmailHistory" PRIMARY KEY ("Id")
    );
    COMMENT ON TABLE notification."FgsEmailHistory" IS 'Stores outbound email history for business entities and provides a permanent audit trail of email communications.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."Id" IS 'Primary key.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."EntityType" IS 'Entity associated with the email such as Estimate, Invoice, WorkOrder, Opportunity, or Customer.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."EntityId" IS 'Identifier of the associated business entity.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."EmailTemplateId" IS 'Email template used to generate the email.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."Subject" IS 'Email subject line.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."FromEmailAddress" IS 'Sender email address.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."FromDisplayName" IS 'Sender display name.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."ToEmailAddresses" IS 'JSON array containing recipient email addresses.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."CcEmailAddresses" IS 'JSON array containing carbon copy recipient email addresses.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."BccEmailAddresses" IS 'JSON array containing blind carbon copy recipient email addresses.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."BodyHtml" IS 'Email body in HTML format.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."BodyText" IS 'Email body in plain text format.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."HasAttachments" IS 'Indicates whether one or more attachments were included in the email.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."Status" IS 'Email delivery status such as Queued, Sent, Delivered, Opened, Failed, or Bounced.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."SentOn" IS 'Date and time the email was sent.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."FailureReason" IS 'Failure reason returned by the email provider when send fails.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."ProviderMessageId" IS 'Provider-specific message identifier used for troubleshooting and webhook tracking.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN notification."FgsEmailHistory"."CreatedBy" IS 'User or process that created the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory') THEN
    CREATE INDEX "IX_FgsEmailHistory_ProviderMessageId" ON notification."FgsEmailHistory" ("ProviderMessageId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory') THEN
    CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId" ON notification."FgsEmailHistory" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory') THEN
    CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId_EntityType_EntityId" ON notification."FgsEmailHistory" ("TenantId", "CompanyId", "EntityType", "EntityId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory') THEN
    CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId_SentOn" ON notification."FgsEmailHistory" ("TenantId", "CompanyId", "SentOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory') THEN
    CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId_Status" ON notification."FgsEmailHistory" ("TenantId", "CompanyId", "Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260617163955_AddFgsEmailHistory') THEN
    INSERT INTO notification."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260617163955_AddFgsEmailHistory', '10.0.8');
    END IF;
END $EF$;
COMMIT;

