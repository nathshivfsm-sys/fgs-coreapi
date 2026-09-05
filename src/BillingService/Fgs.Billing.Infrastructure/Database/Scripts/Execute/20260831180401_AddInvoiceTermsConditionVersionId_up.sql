START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260831180401_AddInvoiceTermsConditionVersionId') THEN
    ALTER TABLE billing."FgsInvoice" ADD "TermsConditionVersionId" bigint;
    COMMENT ON COLUMN billing."FgsInvoice"."TermsConditionVersionId" IS 'Reference to the specific terms and conditions version used by the invoice.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260831180401_AddInvoiceTermsConditionVersionId') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260831180401_AddInvoiceTermsConditionVersionId', '10.0.8');
    END IF;
END $EF$;
COMMIT;

