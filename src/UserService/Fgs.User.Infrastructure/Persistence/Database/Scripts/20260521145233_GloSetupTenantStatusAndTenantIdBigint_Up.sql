-- =============================================================================
-- Migration: 20260521145233_GloSetupTenantStatusAndTenantIdBigint
-- - GloSetupTenantStatus catalog + UX_GloSetupTenantStatus_Name
-- - FgsTenant.FgsTenantStatusId (default 1) + FK_FgsTenant_GloSetupTenantStatus
-- - TenantId / FgsTenant.Id: uuid -> bigint (identity on FgsTenant.Id)
-- Pair with: Database/Migrations/20260521145233_GloSetupTenantStatusAndTenantIdBigint.cs
--
-- WARNING: TenantId type change drops/recreates dependent FKs. Use on dev DBs or
-- after backup. Existing uuid tenant keys are not preserved.
-- =============================================================================

START TRANSACTION;

CREATE TABLE IF NOT EXISTS dbo."GloSetupTenantStatus"
(
    "Id" smallint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500),
    "IsActive" boolean NOT NULL DEFAULT true,
    "CreatedOn" timestamptz NOT NULL DEFAULT now(),
    "CreatedBy" bigint,
    "UpdatedOn" timestamptz,
    "UpdatedBy" bigint,
    CONSTRAINT "PK_GloSetupTenantStatus" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "UX_GloSetupTenantStatus_Name"
    ON dbo."GloSetupTenantStatus" ("Name");

INSERT INTO dbo."GloSetupTenantStatus" ("Id", "Name", "Description", "IsActive", "CreatedOn")
OVERRIDING SYSTEM VALUE
VALUES
    (1, 'Active', 'Tenant is active and operational.', true, timezone('utc', now())),
    (2, 'Prospect', 'Tenant is in signup or onboarding.', true, timezone('utc', now())),
    (3, 'Suspended', 'Tenant access is temporarily suspended.', true, timezone('utc', now())),
    (4, 'Cancelled', 'Tenant subscription is cancelled.', true, timezone('utc', now()))
ON CONFLICT ("Id") DO NOTHING;

SELECT setval(
    pg_get_serial_sequence('dbo."GloSetupTenantStatus"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSetupTenantStatus"), 1),
    true);

ALTER TABLE IF EXISTS dbo."FgsTenant"
    ADD COLUMN IF NOT EXISTS "FgsTenantStatusId" smallint NOT NULL DEFAULT 1;

DO $EF$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'FK_FgsTenant_GloSetupTenantStatus'
          AND conrelid = 'dbo."FgsTenant"'::regclass
    ) THEN
        ALTER TABLE dbo."FgsTenant"
            ADD CONSTRAINT "FK_FgsTenant_GloSetupTenantStatus"
            FOREIGN KEY ("FgsTenantStatusId")
            REFERENCES dbo."GloSetupTenantStatus" ("Id");
    END IF;
END $EF$;

CREATE INDEX IF NOT EXISTS "IX_FgsTenant_FgsTenantStatusId"
    ON dbo."FgsTenant" ("FgsTenantStatusId");

INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260521145233_GloSetupTenantStatusAndTenantIdBigint', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
