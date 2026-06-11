START TRANSACTION;
DROP TABLE setup."FgsLeadDisqualificationReason";

DROP TABLE setup."FgsLeadStatus";

DROP TABLE glo."GloLeadDisqualificationReason";

DROP TABLE glo."GloLeadStatus";


DROP TABLE IF EXISTS setup."FgsSetupPricingMatrixLaborTier" CASCADE;
DROP TABLE IF EXISTS setup."FgsSetupPricingMatrixLabor" CASCADE;
DROP TABLE IF EXISTS setup."FgsSetupPricingMatrixMaterialTier" CASCADE;
DROP TABLE IF EXISTS setup."FgsSetupPricingMatrixOther" CASCADE;
DROP TABLE IF EXISTS setup."FgsSetupPricingMatrix" CASCADE;

ALTER TABLE setup."FgsTagEntityType" ADD CONSTRAINT "FK_FgsTagEntityType_GloMasterEntityType_MasterEntityTypeId" FOREIGN KEY ("MasterEntityTypeId") REFERENCES glo."GloMasterEntityType" ("Id") ON DELETE RESTRICT;

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260611122620_RefactorPricingMatrixAndAddLeadEntities';

COMMIT;

