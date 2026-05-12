-- =============================================================================
-- Migration: 20260512144449_SchemaCleanupAlignment
-- Script:   20260512144449_SchemaCleanupAlignment_Down.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Rollback: reverses the EF migration Down() for SchemaCleanupAlignment and removes
--            the history row for MigrationId '20260512144449_SchemaCleanupAlignment'.
--            Apply only to databases that have this migration applied.
-- =============================================================================

START TRANSACTION;
ALTER TABLE dbo."FgsCredentialAudit" DROP CONSTRAINT "FK_FgsCredentialAudit_FgsCredentialSecret_CredentialSecretId";

ALTER TABLE dbo."FgsCredentialAudit" DROP CONSTRAINT "FK_FgsCredentialAudit_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsCredentialProvider" DROP CONSTRAINT "FK_FgsCredentialProvider_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsCredentialProviderConfiguration" DROP CONSTRAINT "FK_FgsCredentialProviderConfiguration_FgsCredentialProvider_Cr~";

ALTER TABLE dbo."FgsCredentialProviderConfiguration" DROP CONSTRAINT "FK_FgsCredentialProviderConfiguration_FgsTenantCompany_TenantI~";

ALTER TABLE dbo."FgsCredentialSecret" DROP CONSTRAINT "FK_FgsCredentialSecret_FgsCredentialProvider_CredentialProvide~";

ALTER TABLE dbo."FgsCredentialSecret" DROP CONSTRAINT "FK_FgsCredentialSecret_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsLocation" DROP CONSTRAINT "FK_FgsLocation_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsLocation" DROP CONSTRAINT "FK_FgsLocation_GloMasterEntityType_MasterEntityTypeId";

ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP CONSTRAINT "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_Com~";

ALTER TABLE dbo."FgsSetupDescription" DROP CONSTRAINT "FK_FgsSetupDescription_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupGLBreak" DROP CONSTRAINT "FK_FgsSetupGLBreak_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupPaymentMethod" DROP CONSTRAINT "FK_FgsSetupPaymentMethod_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupPaymentMethod" DROP CONSTRAINT "FK_FgsSetupPaymentMethod_GloPaymentMethodType_GloPaymentMethod~";

ALTER TABLE dbo."FgsSetupPaymentTerm" DROP CONSTRAINT "FK_FgsSetupPaymentTerm_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupPostalCode" DROP CONSTRAINT "FK_FgsSetupPostalCode_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupPriceSheet" DROP CONSTRAINT "FK_FgsSetupPriceSheet_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupPriceSheetLabor" DROP CONSTRAINT "FK_FgsSetupPriceSheetLabor_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" DROP CONSTRAINT "FK_FgsSetupPriceSheetLaborTier_FgsTenantCompany_TenantId_Compa~";

ALTER TABLE dbo."FgsSetupPriceSheetMaterial" DROP CONSTRAINT "FK_FgsSetupPriceSheetMaterial_FgsTenantCompany_TenantId_Compan~";

ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" DROP CONSTRAINT "FK_FgsSetupPriceSheetMaterialRange_FgsTenantCompany_TenantId_C~";

ALTER TABLE dbo."FgsSetupPriceSheetOther" DROP CONSTRAINT "FK_FgsSetupPriceSheetOther_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" DROP CONSTRAINT "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompany_TenantId_~";

ALTER TABLE dbo."FgsSetupServiceAssetMedia" DROP CONSTRAINT "FK_FgsSetupServiceAssetMedia_FgsTenantCompany_TenantId_Company~";

ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" DROP CONSTRAINT "FK_FgsSetupServiceAssetModelSerialDescription_FgsTenantCompany~";

ALTER TABLE dbo."FgsSetupServiceAssetType" DROP CONSTRAINT "FK_FgsSetupServiceAssetType_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupTax" DROP CONSTRAINT "FK_FgsSetupTax_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupTaxAuthority" DROP CONSTRAINT "FK_FgsSetupTaxAuthority_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupTaxDetail" DROP CONSTRAINT "FK_FgsSetupTaxDetail_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupTechSkillLevel" DROP CONSTRAINT "FK_FgsSetupTechSkillLevel_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupTechTrade" DROP CONSTRAINT "FK_FgsSetupTechTrade_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupTimeSlot" DROP CONSTRAINT "FK_FgsSetupTimeSlot_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupTitleOfCourtesy" DROP CONSTRAINT "FK_FgsSetupTitleOfCourtesy_FgsTenantCompany_TenantId_CompanyId";

ALTER TABLE dbo."FgsSetupZone" DROP CONSTRAINT "FK_FgsSetupZone_FgsTenantCompany_TenantId_CompanyId";

DROP TABLE dbo."FgsTenantServiceSetup";

DROP TABLE dbo."GloCommunicationToken";

DROP TABLE dbo."GloMasterEntityType";

DROP TABLE dbo."GloPaymentMethodType";

DROP TABLE dbo."GloTimeCardOption";

ALTER TABLE dbo."FgsTenantCompany" DROP CONSTRAINT "AK_FgsTenantCompany_TenantId_CompanyGuid";

DROP INDEX dbo."IX_FgsSetupZone_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupTimeSlot_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupTechTrade_TenantId_CompanyId_TradeCode";

DROP INDEX dbo."IX_FgsSetupTechSkillLevel_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupTaxAuthority_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupTax_TenantId_CompanyId_TaxCode";

DROP INDEX dbo."IX_FgsSetupServiceAssetType_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupPriceSheetOther_TenantId_CompanyId_CategoryCode";

DROP INDEX dbo."IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupPriceSheet_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupPostalCode_TenantId_CompanyId_PostalCode";

DROP INDEX dbo."IX_FgsSetupPaymentTerm_TenantId_CompanyId_Name";

DROP INDEX dbo."IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId";

DROP INDEX dbo."IX_FgsSetupPaymentMethod_TenantId_CompanyId_GloPaymentMethodTy~";

DROP INDEX dbo."IX_FgsSetupGLBreak_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsSetupDescription_TenantId_CompanyId_DescriptionTypeCode";

DROP INDEX dbo."IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTy~";

DROP INDEX dbo."IX_FgsLocation_TenantId_CompanyId_MasterEntityTypeId_EntityNum~";

DROP INDEX dbo."IX_FgsCredentialSecret_CredentialProviderId";

DROP INDEX dbo."IX_FgsCredentialSecret_IsActive";

DROP INDEX dbo."IX_FgsCredentialSecret_TenantId_CompanyId";

DROP INDEX dbo."IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId";

DROP INDEX dbo."IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId~";

DROP INDEX dbo."IX_FgsCredentialProviderConfiguration_CredentialProviderId";

DROP INDEX dbo."IX_FgsCredentialProviderConfiguration_TenantId_CompanyId";

DROP INDEX dbo."IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Cred~1";

DROP INDEX dbo."IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Crede~";

DROP INDEX dbo."IX_FgsCredentialProvider_TenantId_CompanyId_Code";

DROP INDEX dbo."IX_FgsCredentialAudit_CredentialSecretId";

DROP INDEX dbo."IX_FgsCredentialAudit_TenantId_CompanyId";

DROP INDEX dbo."IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId";

DROP INDEX dbo."IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId_Ac~";

ALTER TABLE dbo."FgsSetupPaymentMethod" DROP COLUMN "GloPaymentMethodTypeId";

ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "BreakLabel";

ALTER TABLE dbo."FgsLocation" DROP COLUMN "AddressLine3";

ALTER TABLE dbo."FgsLocation" DROP COLUMN "AddressLine4";

ALTER TABLE dbo."FgsLocation" DROP COLUMN "CompanyId";

ALTER TABLE dbo."FgsLocation" DROP COLUMN "TenantId";

ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "CompanyId";

ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "EncryptedDek";

ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "EncryptedSecretValue";

ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "EncryptionKeyId";

ALTER TABLE dbo."FgsCredentialSecret" DROP COLUMN "TenantId";

ALTER TABLE dbo."FgsCredentialProviderConfiguration" DROP COLUMN "CompanyId";

ALTER TABLE dbo."FgsCredentialProviderConfiguration" DROP COLUMN "TenantId";

ALTER TABLE dbo."FgsCredentialAudit" DROP COLUMN "CompanyId";

ALTER TABLE dbo."FgsCredentialAudit" DROP COLUMN "TenantId";

ALTER TABLE dbo."FgsSetupGLBreak" RENAME COLUMN "LogoUrl" TO "Description";

ALTER TABLE dbo."FgsLocation" RENAME COLUMN "MasterEntityTypeId" TO "EntityTypeId";

ALTER INDEX dbo."IX_FgsLocation_MasterEntityTypeId" RENAME TO "IX_FgsLocation_EntityTypeId";

ALTER TABLE dbo."FgsCredentialSecret" RENAME COLUMN "LastRotatedOn" TO "RotatedOn";

ALTER TABLE dbo."FgsCredentialSecret" RENAME COLUMN "IsRevoked" TO "RotationEnabled";

ALTER TABLE dbo."FgsCredentialSecret" RENAME COLUMN "ExpiresOn" TO "LastValidatedOn";

ALTER TABLE dbo."FgsSetupZone" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupTitleOfCourtesy" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupTimeSlot" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupTechTrade" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupTechSkillLevel" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupTaxDetail" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupTaxAuthority" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupTax" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupServiceAssetType" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupServiceAssetMedia" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPriceSheetOther" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPriceSheetMaterial" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPriceSheetLabor" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPriceSheet" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPostalCode" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPaymentTerm" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPaymentMethod" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupPaymentMethod" ADD "PaymentMethodType" text NOT NULL DEFAULT '';

ALTER TABLE dbo."FgsSetupGLBreak" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupGLBreak" ADD "LogoLocationId" uuid;

ALTER TABLE dbo."FgsSetupDescription" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupCommunicationTemplate" ALTER COLUMN "CompanyId" TYPE bigint;

ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD "GloMasterEntityTypeId" integer;

ALTER TABLE dbo."FgsCredentialSecret" ALTER COLUMN "SecretName" TYPE character varying(500);

ALTER TABLE dbo."FgsCredentialSecret" ADD "KmsKeyArn" character varying(1000);

ALTER TABLE dbo."FgsCredentialSecret" ADD "RegionName" character varying(100) NOT NULL DEFAULT '';

ALTER TABLE dbo."FgsCredentialSecret" ADD "Remarks" character varying(1000);

ALTER TABLE dbo."FgsCredentialSecret" ADD "SecretArn" character varying(1000);

ALTER TABLE dbo."FgsCredentialSecret" ADD "VaultProvider" character varying(100) NOT NULL DEFAULT '';

ALTER TABLE dbo."FgsCredentialProvider" ALTER COLUMN "TenantId" DROP NOT NULL;

ALTER TABLE dbo."FgsCredentialProvider" ALTER COLUMN "CompanyId" TYPE bigint;
ALTER TABLE dbo."FgsCredentialProvider" ALTER COLUMN "CompanyId" DROP NOT NULL;

CREATE TABLE dbo."FgsSetupCommunicationToken" (
    "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
    "CompanyId" bigint NOT NULL,
    "CreatedBy" uuid,
    "CreatedOn" timestamptz NOT NULL,
    "Description" text,
    "DisplayName" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "SampleValue" text,
    "TenantId" uuid NOT NULL,
    "TokenCode" text NOT NULL,
    "UpdatedBy" uuid,
    "UpdatedOn" timestamptz,
    CONSTRAINT "PK_FgsSetupCommunicationToken" PRIMARY KEY ("Id")
);

CREATE TABLE dbo."FgsTenantCompanyConfiguration" (
    "TenantId" uuid NOT NULL,
    "CompanyId" bigint NOT NULL,
    "AccountingIntegrationTypeId" integer,
    "BillHoursFromDispatchOrArrive" character varying(20) NOT NULL,
    "BillToStartNumber" bigint NOT NULL,
    "CreatedBy" uuid,
    "CreatedOn" timestamptz NOT NULL,
    "DTEndTime" interval,
    "DTStartTime" interval,
    "EnableAutoArrive" boolean NOT NULL,
    "EnableCallBookingWidget" boolean NOT NULL,
    "EnableCustomerPortal" boolean NOT NULL,
    "EnablePaymentWidget" boolean NOT NULL,
    "EnableRulesManagement" boolean NOT NULL,
    "InvoiceBatchNumberFormat" character varying(200),
    "InvoiceNumberPrefix" character varying(20),
    "IsActive" boolean NOT NULL,
    "OTEndTime" interval,
    "OTStartTime" interval,
    "PONumberPrefix" character varying(20),
    "POStartNumber" bigint NOT NULL,
    "QuoteNumberPrefix" character varying(20),
    "QuoteStartNumber" bigint NOT NULL,
    "SourceCodeRequiredOnServiceLocation" boolean NOT NULL,
    "SourceCodeRequiredOnWorkOrder" boolean NOT NULL,
    "TimeCardOptionId" integer NOT NULL,
    "UpdatedBy" uuid,
    "UpdatedOn" timestamptz,
    "WorkLocationRadiusForAutoArrive" integer,
    "WorkOrderNumberPrefix" character varying(20),
    "WorkOrderStartNumber" bigint NOT NULL,
    CONSTRAINT "PK_FgsTenantCompanyConfiguration" PRIMARY KEY ("TenantId", "CompanyId")
);

CREATE UNIQUE INDEX "IX_FgsCredentialProvider_TenantId_Code" ON dbo."FgsCredentialProvider" ("TenantId", "Code");

CREATE INDEX "IX_FgsSetupCommunicationToken_TenantId_CompanyId" ON dbo."FgsSetupCommunicationToken" ("TenantId", "CompanyId");

DELETE FROM dbo."__EFMigrationsHistory"
WHERE "MigrationId" = '20260512144449_SchemaCleanupAlignment';

COMMIT;

