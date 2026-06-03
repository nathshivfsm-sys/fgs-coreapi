-- =============================================================================
-- Migration: 20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption
-- Script:   20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption_up.sql
-- Path:     Persistence/Database/Scripts/Execute
-- Database: PostgreSQL
--
-- Replaces legacy credential tables; keeps audit.FgsCredentialAudit repointed to
-- setup.FgsCredential. Seed: Initial_Migration_Seed.sql separately.
-- Idempotent (dotnet ef migrations script --idempotent).
-- =============================================================================

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    ALTER TABLE audit."FgsCredentialAudit" DROP CONSTRAINT "FK_FgsCredentialAudit_CredentialSecret";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    DROP TABLE setup."FgsCredentialProviderConfiguration";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    DROP TABLE setup."FgsCredentialSecret";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    DROP TABLE glo."GloCredentialCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    DROP TABLE setup."FgsCredentialProvider";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    DROP TABLE glo."GloCredentialProviderType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    ALTER TABLE audit."FgsCredentialAudit" RENAME COLUMN "CredentialSecretId" TO "CredentialId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    ALTER INDEX audit."IX_FgsCredentialAudit_CredentialSecretId" RENAME TO "IX_FgsCredentialAudit_CredentialId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE TABLE glo."GloCredentialProviderType" (
        "Id" integer GENERATED ALWAYS AS IDENTITY,
        "ProviderCode" character varying(50) NOT NULL,
        "ProviderName" character varying(200) NOT NULL,
        "ConfigurationSchema" jsonb NOT NULL,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_GloCredentialProviderType" PRIMARY KEY ("Id")
    );
    COMMENT ON TABLE glo."GloCredentialProviderType" IS 'Master list of supported credential providers and integrations available within the FSM platform.';
    COMMENT ON COLUMN glo."GloCredentialProviderType"."ProviderCode" IS 'System unique code used by application logic and integration services.';
    COMMENT ON COLUMN glo."GloCredentialProviderType"."ProviderName" IS 'User friendly provider name displayed in setup screens.';
    COMMENT ON COLUMN glo."GloCredentialProviderType"."ConfigurationSchema" IS 'JSON schema used by the UI to dynamically render provider configuration fields and perform validation.';
    COMMENT ON COLUMN glo."GloCredentialProviderType"."IsActive" IS 'Indicates whether the provider can be selected for new credential configurations.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE TABLE setup."FgsCredential" (
        "Id" uuid NOT NULL,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "CredentialProviderTypeId" integer NOT NULL,
        "CredentialName" character varying(200) NOT NULL,
        "Description" character varying(500),
        "CredentialData" bytea NOT NULL,
        "EncryptedDataKey" bytea NOT NULL,
        "KeyIdentifier" character varying(200),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_FgsCredential" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsCredential_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES tenant."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsCredential_GloCredentialProviderType" FOREIGN KEY ("CredentialProviderTypeId") REFERENCES glo."GloCredentialProviderType" ("Id") ON DELETE RESTRICT
    );
    COMMENT ON TABLE setup."FgsCredential" IS 'Stores tenant-owned credentials encrypted using AWS KMS envelope encryption.';
    COMMENT ON COLUMN setup."FgsCredential"."TenantId" IS 'Tenant that owns the credential.';
    COMMENT ON COLUMN setup."FgsCredential"."CompanyId" IS 'Company that owns the credential.';
    COMMENT ON COLUMN setup."FgsCredential"."CredentialProviderTypeId" IS 'Credential provider associated with this credential.';
    COMMENT ON COLUMN setup."FgsCredential"."CredentialName" IS 'User friendly name displayed in tenant administration screens.';
    COMMENT ON COLUMN setup."FgsCredential"."Description" IS 'Optional description of the credential usage.';
    COMMENT ON COLUMN setup."FgsCredential"."CredentialData" IS 'Provider credential JSON encrypted using a Data Encryption Key (DEK).';
    COMMENT ON COLUMN setup."FgsCredential"."EncryptedDataKey" IS 'Data Encryption Key encrypted using AWS KMS.';
    COMMENT ON COLUMN setup."FgsCredential"."KeyIdentifier" IS 'AWS KMS key ARN or alias used to encrypt the Data Encryption Key.';
    COMMENT ON COLUMN setup."FgsCredential"."IsActive" IS 'Indicates whether the credential is active and available for use.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE TABLE glo."GloCredential" (
        "Id" integer GENERATED ALWAYS AS IDENTITY,
        "CredentialProviderTypeId" integer NOT NULL,
        "CredentialName" character varying(200) NOT NULL,
        "Description" character varying(500),
        "CredentialData" bytea NOT NULL,
        "EncryptedDataKey" bytea NOT NULL,
        "KeyIdentifier" character varying(200),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_GloCredential" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_GloCredential_ProviderType" FOREIGN KEY ("CredentialProviderTypeId") REFERENCES glo."GloCredentialProviderType" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE UNIQUE INDEX "UQ_GloCredentialProviderType_ProviderCode" ON glo."GloCredentialProviderType" ("ProviderCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE INDEX "IX_FgsCredential_CredentialProviderTypeId" ON setup."FgsCredential" ("CredentialProviderTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE INDEX "IX_FgsCredential_Tenant_Company" ON setup."FgsCredential" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE INDEX "IX_FgsCredential_Tenant_Company_ProviderType" ON setup."FgsCredential" ("TenantId", "CompanyId", "CredentialProviderTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE UNIQUE INDEX "UQ_FgsCredential_Tenant_Company_ProviderType" ON setup."FgsCredential" ("TenantId", "CompanyId", "CredentialProviderTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    CREATE INDEX "IX_GloCredential_CredentialProviderTypeId" ON glo."GloCredential" ("CredentialProviderTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    DELETE FROM audit."FgsCredentialAudit";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    ALTER TABLE audit."FgsCredentialAudit" ADD CONSTRAINT "FK_FgsCredentialAudit_Credential" FOREIGN KEY ("CredentialId") REFERENCES setup."FgsCredential" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM shared."__EFMigrationsHistory" WHERE "MigrationId" = '20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption') THEN
    INSERT INTO shared."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260601192509_ReplaceCredentialModelWithKmsEnvelopeEncryption', '10.0.8');
    END IF;
END $EF$;
COMMIT;

