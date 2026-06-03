-- =============================================================================
-- Migration: 20260516180000_InitialPlatform
-- Script:   20260516180000_InitialPlatform_Up.sql
-- Database: fgs_platform (PostgreSQL, schema: dbo)
--
-- Behavior:
--   1. Ensures schema "dbo" exists.
--   2. Ensures "__EFMigrationsHistory" exists (IF NOT EXISTS).
--   3. Applies InitialPlatform DDL only when MigrationId is not yet recorded.
--   4. Inserts MigrationId '20260516180000_InitialPlatform' and ProductVersion
--      into "__EFMigrationsHistory" after successful DDL.
--
-- Notes:
--   - Idempotent DO blocks (matches: dotnet ef migrations script --idempotent).
--   - Matches: 20260516180000_InitialPlatform.cs
-- =============================================================================

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dbo') THEN
        CREATE SCHEMA dbo;
    END IF;
END $EF$;

CREATE TABLE IF NOT EXISTS dbo."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dbo') THEN
            CREATE SCHEMA dbo;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    CREATE TABLE dbo."FgsNotificationHistory" (
        "Id" uuid NOT NULL,
        "TenantId" uuid NOT NULL,
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
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    CREATE TABLE dbo."FgsProcessedIntegrationEvent" (
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
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    CREATE INDEX "IX_FgsNotificationHistory_TenantId_CreatedOn" ON dbo."FgsNotificationHistory" ("TenantId", "CreatedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    CREATE UNIQUE INDEX "IX_FgsProcessedIntegrationEvent_MessageId" ON dbo."FgsProcessedIntegrationEvent" ("MessageId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260516180000_InitialPlatform') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260516180000_InitialPlatform', '10.0.8');
    END IF;
END $EF$;

COMMIT;
