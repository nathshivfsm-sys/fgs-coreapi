DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'setup') THEN
        CREATE SCHEMA setup;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS setup."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260622190000_MoveTaxPercentToTaxAuthority') THEN
        ALTER TABLE setup."FgsSetupTaxAuthority" ADD COLUMN "TaxPercent" numeric;

        UPDATE setup."FgsSetupTaxAuthority" ta
        SET "TaxPercent" = sub."TaxPercent"
        FROM (
            SELECT DISTINCT ON ("FgsSetupTaxAuthorityId")
                "FgsSetupTaxAuthorityId",
                "TaxPercent"
            FROM setup."FgsSetupTaxDetail"
            ORDER BY "FgsSetupTaxAuthorityId", "EffectiveFromDate" DESC
        ) sub
        WHERE ta."Id" = sub."FgsSetupTaxAuthorityId";

        UPDATE setup."FgsSetupTaxAuthority"
        SET "TaxPercent" = 0
        WHERE "TaxPercent" IS NULL;

        ALTER TABLE setup."FgsSetupTaxAuthority" ALTER COLUMN "TaxPercent" SET NOT NULL;
        ALTER TABLE setup."FgsSetupTaxAuthority" ALTER COLUMN "TaxPercent" SET DEFAULT 0;

        ALTER TABLE setup."FgsSetupTaxDetail" DROP CONSTRAINT IF EXISTS "CK_FgsSetupTaxDetail_TaxPercent";
        ALTER TABLE setup."FgsSetupTaxDetail" DROP COLUMN IF EXISTS "TaxPercent";

        ALTER TABLE setup."FgsSetupTaxAuthority" ADD CONSTRAINT "CK_FgsSetupTaxAuthority_TaxPercent"
            CHECK ("TaxPercent" >= 0 AND "TaxPercent" <= 100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260622190000_MoveTaxPercentToTaxAuthority') THEN
        INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20260622190000_MoveTaxPercentToTaxAuthority', '10.0.8');
    END IF;
END $EF$;

COMMIT;
