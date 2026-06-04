-- =============================================================================
-- Migration: 20260603222551_InitialSchema
-- Script:   InitialSchema_up.sql
-- Schema:   notification
-- =============================================================================

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
    CREATE INDEX "IX_FgsNotificationHistory_TenantId_CreatedOn"
        ON notification."FgsNotificationHistory" ("TenantId", "CreatedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notification."__EFMigrationsHistory" WHERE "MigrationId" = '20260603222551_InitialSchema') THEN
    CREATE UNIQUE INDEX "IX_FgsProcessedIntegrationEvent_MessageId"
        ON notification."FgsProcessedIntegrationEvent" ("MessageId");
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
