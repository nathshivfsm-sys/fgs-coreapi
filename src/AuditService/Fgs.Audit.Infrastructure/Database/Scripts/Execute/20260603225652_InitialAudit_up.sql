DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'audit') THEN
        CREATE SCHEMA audit;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS audit."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'audit') THEN
            CREATE SCHEMA audit;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit') THEN
    CREATE TABLE audit."FgsCredentialAudit" (
        "Id" uuid NOT NULL,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "CredentialId" uuid NOT NULL,
        "ActionType" character varying(100) NOT NULL,
        "Remarks" character varying(1000),
        "OldVersionNo" integer,
        "NewVersionNo" integer,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" text,
        CONSTRAINT "PK_FgsCredentialAudit" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit') THEN
    CREATE INDEX "IX_FgsCredentialAudit_CredentialId" ON audit."FgsCredentialAudit" ("CredentialId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit') THEN
    CREATE INDEX "IX_FgsCredentialAudit_Tenant_Company" ON audit."FgsCredentialAudit" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit') THEN
    CREATE INDEX "IX_FgsCredentialAudit_Tenant_Company_Cred" ON audit."FgsCredentialAudit" ("TenantId", "CompanyId", "CredentialId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit') THEN
    CREATE UNIQUE INDEX "UQ_FgsCredentialAudit" ON audit."FgsCredentialAudit" ("TenantId", "CompanyId", "CredentialId", "ActionType", "NewVersionNo");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM audit."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225652_InitialAudit') THEN
    INSERT INTO audit."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603225652_InitialAudit', '10.0.8');
    END IF;
END $EF$;
COMMIT;

