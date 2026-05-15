-- =============================================================================
-- Migration: 20260515090709_UserIdentityInvitationOutbox
-- Script:   20260515090709_UserIdentityInvitationOutbox_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Creates FgsUser, FgsInvitation, and FgsOutboxMessage tables.
--   2. Records MigrationId in "__EFMigrationsHistory" when not yet present.
--
-- Notes:
--   - Idempotent DO blocks (matches: dotnet ef migrations script --idempotent).
--   - Matches: 20260515090709_UserIdentityInvitationOutbox.cs
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE TABLE dbo."FgsOutboxMessage" (
        "Id" uuid NOT NULL,
        "EventType" character varying(200) NOT NULL,
        "Payload" jsonb NOT NULL,
        "IdempotencyKey" character varying(200) NOT NULL,
        "CorrelationId" character varying(100),
        "Status" character varying(50) NOT NULL,
        "RetryCount" integer NOT NULL,
        "LastError" character varying(2000),
        "CreatedOn" timestamptz NOT NULL,
        "ProcessedOn" timestamptz,
        "IsDeleted" boolean NOT NULL,
        CONSTRAINT "PK_FgsOutboxMessage" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE TABLE dbo."FgsUser" (
        "Id" uuid NOT NULL,
        "TenantId" uuid NOT NULL,
        "CompanyId" uuid NOT NULL,
        "Email" character varying(300) NOT NULL,
        "NormalizedEmail" character varying(300) NOT NULL,
        "DisplayName" character varying(200) NOT NULL,
        "PasswordHash" character varying(500),
        "EntraObjectId" character varying(100),
        "Role" character varying(50) NOT NULL,
        "IsActive" boolean NOT NULL,
        "IsDeleted" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsUser" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsUser_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyGuid") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsUser_FgsTenant_TenantId" FOREIGN KEY ("TenantId") REFERENCES dbo."FgsTenant" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE TABLE dbo."FgsInvitation" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "TenantId" uuid NOT NULL,
        "Email" character varying(300) NOT NULL,
        "TokenHash" character varying(128) NOT NULL,
        "Status" character varying(50) NOT NULL,
        "ExpiresAtUtc" timestamptz NOT NULL,
        "AcceptedAtUtc" timestamptz,
        "IsDeleted" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsInvitation" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsInvitation_FgsUser_UserId" FOREIGN KEY ("UserId") REFERENCES dbo."FgsUser" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE INDEX "IX_FgsInvitation_TenantId_Email_Status" ON dbo."FgsInvitation" ("TenantId", "Email", "Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE INDEX "IX_FgsInvitation_TokenHash" ON dbo."FgsInvitation" ("TokenHash");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE INDEX "IX_FgsInvitation_UserId" ON dbo."FgsInvitation" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE UNIQUE INDEX "IX_FgsOutboxMessage_IdempotencyKey" ON dbo."FgsOutboxMessage" ("IdempotencyKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE INDEX "IX_FgsOutboxMessage_Status_CreatedOn" ON dbo."FgsOutboxMessage" ("Status", "CreatedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE INDEX "IX_FgsUser_TenantId_CompanyId" ON dbo."FgsUser" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    CREATE UNIQUE INDEX "IX_FgsUser_TenantId_NormalizedEmail" ON dbo."FgsUser" ("TenantId", "NormalizedEmail") WHERE "IsDeleted" = false;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260515090709_UserIdentityInvitationOutbox') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260515090709_UserIdentityInvitationOutbox', '10.0.0');
    END IF;
END $EF$;

COMMIT;
