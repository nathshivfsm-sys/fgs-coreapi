START TRANSACTION;
ALTER TABLE setup."FgsBillingCategory" DROP CONSTRAINT "FK_FgsBillingCategory_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsBusinessType" DROP CONSTRAINT "FK_FgsBusinessType_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsCredential" DROP CONSTRAINT "FK_FgsCredential_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsCredential" DROP CONSTRAINT "FK_FgsCredential_GloCredentialProviderTypeCache_ProviderTypeId";

ALTER TABLE setup."FgsEntityTag" DROP CONSTRAINT "FK_FgsEntityTag_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsInventoryCategory" DROP CONSTRAINT "FK_FgsInventoryCategory_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsInventoryItem" DROP CONSTRAINT "FK_FgsInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsInventoryItemAlternate" DROP CONSTRAINT "FK_FgsInventoryItemAlternate_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsInventoryItemDependency" DROP CONSTRAINT "FK_FgsInventoryItemDependency_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsInventoryItemType" DROP CONSTRAINT "FK_FgsInventoryItemType_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsInventoryStock" DROP CONSTRAINT "FK_FgsInventoryStock_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsInventorySubCategory" DROP CONSTRAINT "FK_FgsInventorySubCategory_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsJobType" DROP CONSTRAINT "FK_FgsJobType_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsJobTypeCategory" DROP CONSTRAINT "FK_FgsJobTypeCategory_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsJobTypeSubCategory" DROP CONSTRAINT "FK_FgsJobTypeSubCategory_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsLeadSource" DROP CONSTRAINT "FK_FgsLeadSource_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsResolutionCode" DROP CONSTRAINT "FK_FgsResolutionCode_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsResolutionCode" DROP CONSTRAINT "FK_FgsResolutionCode_GloResolutionTypeCache_ResolutionTypeId";

ALTER TABLE setup."FgsSetupDescription" DROP CONSTRAINT "FK_FgsSetupDescription_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupGLBreak" DROP CONSTRAINT "FK_FgsSetupGLBreak_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupGLBreakTrade" DROP CONSTRAINT "FK_FgsSetupGLBreakTrade_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupLaborRateType" DROP CONSTRAINT "FK_FgsSetupLaborRateType_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPaymentMethod" DROP CONSTRAINT "FK_FgsSetupPaymentMethod_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPaymentTerm" DROP CONSTRAINT "FK_FgsSetupPaymentTerm_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPostalCode" DROP CONSTRAINT "FK_FgsSetupPostalCode_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPricingMatrix" DROP CONSTRAINT "FK_FgsSetupPricingMatrix_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPricingMatrixLabor" DROP CONSTRAINT "FK_FgsSetupPricingMatrixLabor_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPricingMatrixLaborTier" DROP CONSTRAINT "FK_FgsSetupPricingMatrixLaborTier_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPricingMatrixMaterialTier" DROP CONSTRAINT "FK_FgsSetupPricingMatrixMaterialTier_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupPricingMatrixOther" DROP CONSTRAINT "FK_FgsSetupPricingMatrixOther_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupServiceAssetManufacturer" DROP CONSTRAINT "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupServiceAssetModelReference" DROP CONSTRAINT "FK_FgsSetupServiceAssetModelReference_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupServiceAssetType" DROP CONSTRAINT "FK_FgsSetupServiceAssetType_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupTax" DROP CONSTRAINT "FK_FgsSetupTax_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupTaxAuthority" DROP CONSTRAINT "FK_FgsSetupTaxAuthority_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupTaxDetail" DROP CONSTRAINT "FK_FgsSetupTaxDetail_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupTechSkillLevel" DROP CONSTRAINT "FK_FgsSetupTechSkillLevel_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupTechTrade" DROP CONSTRAINT "FK_FgsSetupTechTrade_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupTimeSlot" DROP CONSTRAINT "FK_FgsSetupTimeSlot_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupTitleOfCourtesy" DROP CONSTRAINT "FK_FgsSetupTitleOfCourtesy_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsSetupZone" DROP CONSTRAINT "FK_FgsSetupZone_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsTag" DROP CONSTRAINT "FK_FgsTag_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsTagEntityType" DROP CONSTRAINT "FK_FgsTagEntityType_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsVehicle" DROP CONSTRAINT "FK_FgsVehicle_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsVehicleMaintenance" DROP CONSTRAINT "FK_FgsVehicleMaintenance_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsVendor" DROP CONSTRAINT "FK_FgsVendor_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsVendorInventoryItem" DROP CONSTRAINT "FK_FgsVendorInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId";

ALTER TABLE setup."FgsWarehouse" DROP CONSTRAINT "FK_FgsWarehouse_FgsTenantCompanyCache_TenantId_CompanyId";

DROP TABLE setup."FgsTenantCompanyCache";

DROP TABLE setup."GloCredentialProviderTypeCache";

DROP TABLE setup."GloResolutionTypeCache";

ALTER TABLE setup."FgsWarehouse" DROP COLUMN "AddressId";

ALTER TABLE setup."FgsWarehouse" ADD "LocationId" uuid;
COMMENT ON COLUMN setup."FgsWarehouse"."LocationId" IS 'Optional reference to the physical address or geo location in FgsLocation.';

ALTER TABLE setup."FgsCredential" ADD CONSTRAINT "FK_FgsCredential_GloCredentialProviderType" FOREIGN KEY ("CredentialProviderTypeId") REFERENCES glo."GloCredentialProviderType" ("Id") ON DELETE RESTRICT;

ALTER TABLE setup."FgsResolutionCode" ADD CONSTRAINT "FK_FgsResolutionCode_GloResType" FOREIGN KEY ("GloResolutionTypeId") REFERENCES glo."GloResolutionType" ("Id") ON DELETE RESTRICT;

DELETE FROM setup."__EFMigrationsHistory"
WHERE "MigrationId" = '20260604133506_AddCacheTablesAndRewireFks';

COMMIT;

