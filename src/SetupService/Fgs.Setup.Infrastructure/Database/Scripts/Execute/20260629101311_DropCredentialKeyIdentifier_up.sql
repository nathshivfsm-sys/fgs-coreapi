START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260629101311_DropCredentialKeyIdentifier') THEN
        ALTER TABLE glo."GloCredential" DROP COLUMN IF EXISTS "KeyIdentifier";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260629101311_DropCredentialKeyIdentifier') THEN
        ALTER TABLE setup."FgsCredential" DROP COLUMN IF EXISTS "KeyIdentifier";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260629101311_DropCredentialKeyIdentifier') THEN
        INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20260629101311_DropCredentialKeyIdentifier', '10.0.8');
    END IF;
END $EF$;

COMMIT;
