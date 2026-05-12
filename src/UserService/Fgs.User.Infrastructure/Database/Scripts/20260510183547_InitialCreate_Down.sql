-- =============================================================================
-- Migration: 20260510183547_InitialCreate
-- Script:   20260510183547_InitialCreate_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback: drops objects created by InitialCreate and removes the history row
--            for MigrationId '20260510183547_InitialCreate'.
-- =============================================================================

START TRANSACTION;
DROP TABLE dbo."FgsCredentialAudit";

DROP TABLE dbo."FgsCredentialProvider";

DROP TABLE dbo."FgsCredentialProviderConfiguration";

DROP TABLE dbo."FgsCredentialSecret";

DROP TABLE dbo."FgsLocation";

DROP TABLE dbo."FgsSetupCommunicationTemplate";

DROP TABLE dbo."FgsSetupCommunicationToken";

DROP TABLE dbo."FgsSetupDescription";

DROP TABLE dbo."FgsSetupGLBreak";

DROP TABLE dbo."FgsSetupPaymentMethod";

DROP TABLE dbo."FgsSetupPaymentTerm";

DROP TABLE dbo."FgsSetupPostalCode";

DROP TABLE dbo."FgsSetupPriceSheet";

DROP TABLE dbo."FgsSetupPriceSheetLabor";

DROP TABLE dbo."FgsSetupPriceSheetLaborTier";

DROP TABLE dbo."FgsSetupPriceSheetMaterial";

DROP TABLE dbo."FgsSetupPriceSheetMaterialRange";

DROP TABLE dbo."FgsSetupPriceSheetOther";

DROP TABLE dbo."FgsSetupServiceAssetManufacturer";

DROP TABLE dbo."FgsSetupServiceAssetMedia";

DROP TABLE dbo."FgsSetupServiceAssetModelSerialDescription";

DROP TABLE dbo."FgsSetupServiceAssetType";

DROP TABLE dbo."FgsSetupTax";

DROP TABLE dbo."FgsSetupTaxAuthority";

DROP TABLE dbo."FgsSetupTaxDetail";

DROP TABLE dbo."FgsSetupTechSkillLevel";

DROP TABLE dbo."FgsSetupTechTrade";

DROP TABLE dbo."FgsSetupTimeSlot";

DROP TABLE dbo."FgsSetupTitleOfCourtesy";

DROP TABLE dbo."FgsSetupZone";

DROP TABLE dbo."FgsTenant";

DROP TABLE dbo."FgsTenantCompany";

DROP TABLE dbo."FgsTenantCompanyConfiguration";

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260510183547_InitialCreate';

COMMIT;

