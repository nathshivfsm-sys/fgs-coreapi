-- Rollback for 20260720184210_ReshapeEmailHistoryAddSmsHistoryDropNotificationHistory

START TRANSACTION;

DROP TABLE IF EXISTS notification."FgsSmsHistory";
DROP TABLE IF EXISTS notification."FgsEmailHistory";

DROP TYPE IF EXISTS notification.source_application;
DROP TYPE IF EXISTS notification.notification_status;

CREATE TABLE notification."FgsEmailHistory" (
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
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
    "HasAttachments" boolean NOT NULL DEFAULT false,
    "Status" character varying(50) NOT NULL,
    "SentOn" timestamptz,
    "FailureReason" text,
    "ProviderMessageId" character varying(500),
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" text,
    CONSTRAINT "PK_FgsEmailHistory" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_FgsEmailHistory_ProviderMessageId" ON notification."FgsEmailHistory" ("ProviderMessageId");
CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId" ON notification."FgsEmailHistory" ("TenantId", "CompanyId");
CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId_EntityType_EntityId" ON notification."FgsEmailHistory" ("TenantId", "CompanyId", "EntityType", "EntityId");
CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId_Status" ON notification."FgsEmailHistory" ("TenantId", "CompanyId", "Status");
CREATE INDEX "IX_FgsEmailHistory_TenantId_CompanyId_SentOn" ON notification."FgsEmailHistory" ("TenantId", "CompanyId", "SentOn");

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

CREATE INDEX "IX_FgsNotificationHistory_TenantId_CreatedOn" ON notification."FgsNotificationHistory" ("TenantId", "CreatedOn");

DELETE FROM notification."__EFMigrationsHistory"
WHERE "MigrationId" = '20260720184210_ReshapeEmailHistoryAddSmsHistoryDropNotificationHistory';

COMMIT;
