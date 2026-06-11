START TRANSACTION;
DROP TABLE setup."FgsSalesActivityOutcome";

DROP TABLE setup."FgsSalesActivityType";

DROP TABLE setup."FgsSalesDispositionReason";

DROP TABLE glo."GloSalesActivityOutcome";

DROP TABLE glo."GloSalesActivityType";

DROP TABLE glo."GloSalesDispositionReason";

DROP TABLE glo."GloSalesPipelineStatus";

DROP TABLE setup."FgsSalesPipelineStatus";

CREATE INDEX "IX_FgsVehicleMaintenance_VehicleMaintenanceTypeId" ON setup."FgsVehicleMaintenance" ("VehicleMaintenanceTypeId");

CREATE INDEX "IX_FgsSetupPricingMatrixLabor_LaborRateTypeId" ON setup."FgsSetupPricingMatrixLabor" ("LaborRateTypeId");

CREATE INDEX "IX_FgsEntityTag_MasterEntityTypeId1" ON setup."FgsEntityTag" ("MasterEntityTypeId");

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260611130512_AddSalesPipelineEntities';

COMMIT;

