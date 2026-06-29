-- Rollback for 20260629101311_DropCredentialKeyIdentifier
START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260629101311_DropCredentialKeyIdentifier') THEN
        ALTER TABLE glo."GloCredential"
        ADD COLUMN IF NOT EXISTS "KeyIdentifier" character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260629101311_DropCredentialKeyIdentifier') THEN
        ALTER TABLE setup."FgsCredential"
        ADD COLUMN IF NOT EXISTS "KeyIdentifier" character varying(200);
        COMMENT ON COLUMN setup."FgsCredential"."KeyIdentifier" IS 'AWS KMS key ARN or alias used to encrypt the Data Encryption Key.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260629101311_DropCredentialKeyIdentifier') THEN
        DELETE FROM setup."__EFMigrationsHistory"
        WHERE "MigrationId" = '20260629101311_DropCredentialKeyIdentifier';
    END IF;
END $EF$;

COMMIT;
