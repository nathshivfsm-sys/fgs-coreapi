-- =============================================================================
-- Migration: 20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage
-- Script:   20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage_down.sql
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage') THEN
    DROP TABLE IF EXISTS dbo."GloOutboxMessage";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage') THEN
    CREATE TABLE dbo."FgsOutboxMessage" (
        "Id" uuid NOT NULL,
        "CorrelationId" character varying(100),
        "CreatedOn" timestamptz NOT NULL,
        "EventType" character varying(200) NOT NULL,
        "IdempotencyKey" character varying(200) NOT NULL,
        "IsDeleted" boolean NOT NULL,
        "LastError" character varying(2000),
        "Payload" jsonb NOT NULL,
        "ProcessedOn" timestamptz,
        "RetryCount" integer NOT NULL,
        "Status" character varying(50) NOT NULL,
        CONSTRAINT "PK_FgsOutboxMessage" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage') THEN
    CREATE UNIQUE INDEX "IX_FgsOutboxMessage_IdempotencyKey" ON dbo."FgsOutboxMessage" ("IdempotencyKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage') THEN
    CREATE INDEX "IX_FgsOutboxMessage_Status_CreatedOn" ON dbo."FgsOutboxMessage" ("Status", "CreatedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260523075504_ReplaceFgsOutboxMessageWithGloOutboxMessage';
    END IF;
END $EF$;

COMMIT;
