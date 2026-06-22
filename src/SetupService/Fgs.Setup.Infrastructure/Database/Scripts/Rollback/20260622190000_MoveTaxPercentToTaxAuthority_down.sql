-- Rollback for 20260622190000_MoveTaxPercentToTaxAuthority
START TRANSACTION;

ALTER TABLE setup."FgsSetupTaxDetail" ADD COLUMN IF NOT EXISTS "TaxPercent" numeric;

UPDATE setup."FgsSetupTaxDetail" td
SET "TaxPercent" = ta."TaxPercent"
FROM setup."FgsSetupTaxAuthority" ta
WHERE td."FgsSetupTaxAuthorityId" = ta."Id";

UPDATE setup."FgsSetupTaxDetail"
SET "TaxPercent" = 0
WHERE "TaxPercent" IS NULL;

ALTER TABLE setup."FgsSetupTaxDetail" ALTER COLUMN "TaxPercent" SET NOT NULL;

ALTER TABLE setup."FgsSetupTaxDetail" ADD CONSTRAINT "CK_FgsSetupTaxDetail_TaxPercent"
    CHECK ("TaxPercent" >= 0 AND "TaxPercent" <= 100);

ALTER TABLE setup."FgsSetupTaxAuthority" DROP CONSTRAINT IF EXISTS "CK_FgsSetupTaxAuthority_TaxPercent";
ALTER TABLE setup."FgsSetupTaxAuthority" DROP COLUMN IF EXISTS "TaxPercent";

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260622190000_MoveTaxPercentToTaxAuthority';

COMMIT;
