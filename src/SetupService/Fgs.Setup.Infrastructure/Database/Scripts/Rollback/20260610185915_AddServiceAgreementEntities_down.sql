START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260610185915_AddServiceAgreementEntities') THEN
    DROP TABLE setup."FgsSetupServiceAgreementPricingComponent";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260610185915_AddServiceAgreementEntities') THEN
    DROP TABLE setup."FgsSetupServiceAgreementTemplateCoverage";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260610185915_AddServiceAgreementEntities') THEN
    DROP TABLE setup."FgsSetupServiceAgreementTemplatePricingComponent";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260610185915_AddServiceAgreementEntities') THEN
    DROP TABLE setup."FgsSetupServiceAgreementTemplate";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM setup."__EFMigrationsHistory" WHERE "MigrationId" = '20260610185915_AddServiceAgreementEntities') THEN
    DELETE FROM setup."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260610185915_AddServiceAgreementEntities';
    END IF;
END $EF$;
COMMIT;

