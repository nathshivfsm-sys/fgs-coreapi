-- =============================================================================
-- Migration: 20260510183547_InitialCreate
-- Script:   20260510183547_InitialCreate_Up.sql
-- Database: PostgreSQL (schema: dbo)
--
-- Behavior:
--   1. Ensures schema "dbo" exists.
--   2. Ensures "__EFMigrationsHistory" exists (IF NOT EXISTS).
--   3. Applies InitialCreate DDL only when MigrationId is not yet recorded.
--   4. Inserts MigrationId '20260510183547_InitialCreate' (EF migration name)
--      and ProductVersion into "__EFMigrationsHistory" after successful DDL.
-- =============================================================================

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dbo') THEN
        CREATE SCHEMA dbo;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS dbo."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dbo') THEN
            CREATE SCHEMA dbo;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsCredentialAudit" (
        "Id" uuid NOT NULL,
        "CredentialSecretId" uuid NOT NULL,
        "ActionType" character varying(100) NOT NULL,
        "Remarks" character varying(1000),
        "OldVersionNo" integer,
        "NewVersionNo" integer,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        CONSTRAINT "PK_FgsCredentialAudit" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsCredentialProvider" (
        "Id" uuid NOT NULL,
        "TenantId" uuid,
        "CompanyId" bigint,
        "CredentialProviderTypeId" integer NOT NULL,
        "Code" character varying(100) NOT NULL,
        "Name" character varying(200) NOT NULL,
        "Environment" character varying(50) NOT NULL,
        "Description" character varying(1000),
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsCredentialProvider" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsCredentialProviderConfiguration" (
        "Id" uuid NOT NULL,
        "CredentialProviderId" uuid NOT NULL,
        "ConfigurationKey" character varying(200) NOT NULL,
        "ConfigurationValue" text,
        "Environment" character varying(50),
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsCredentialProviderConfiguration" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsCredentialSecret" (
        "Id" uuid NOT NULL,
        "CredentialProviderId" uuid NOT NULL,
        "VaultProvider" character varying(100) NOT NULL,
        "SecretName" character varying(500) NOT NULL,
        "SecretArn" character varying(1000),
        "RegionName" character varying(100) NOT NULL,
        "KmsKeyArn" character varying(1000),
        "RotationEnabled" boolean NOT NULL,
        "VersionNo" integer NOT NULL,
        "RotatedOn" timestamptz,
        "LastValidatedOn" timestamptz,
        "Remarks" character varying(1000),
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsCredentialSecret" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsLocation" (
        "Id" uuid NOT NULL,
        "EntityTypeId" integer NOT NULL,
        "EntityNumber" bigint,
        "AddressLine1" character varying(200),
        "AddressLine2" character varying(200),
        "City" character varying(100),
        "State" character varying(100),
        "County" character varying(100),
        "Country" character varying(100),
        "PostalCode" character varying(20),
        "FormattedAddress" character varying(1000),
        "Latitude" numeric(18,10),
        "Longitude" numeric(18,10),
        "PlaceId" character varying(500),
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsLocation" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupCommunicationTemplate" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TemplateType" text NOT NULL,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "Subject" text,
        "Body" text NOT NULL,
        "IsMobileVisible" boolean NOT NULL,
        "GloMasterEntityTypeId" integer,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupCommunicationTemplate" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupCommunicationToken" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TokenCode" text NOT NULL,
        "DisplayName" text NOT NULL,
        "Description" text,
        "SampleValue" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupCommunicationToken" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupDescription" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "DescriptionTypeCode" text NOT NULL,
        "Body" text NOT NULL,
        "FgsSetupTechTradeId" bigint,
        "SortOrder" integer NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupDescription" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupGLBreak" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "BreakLevel" integer NOT NULL,
        "FgsSetupTechTradeId" bigint,
        "LogoLocationId" uuid,
        "Description" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupGLBreak" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPaymentMethod" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "PaymentMethodType" text NOT NULL,
        "DisplayName" text NOT NULL,
        "IsMobileVisible" boolean NOT NULL,
        "IsCustomerPortalVisible" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPaymentMethod" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPaymentTerm" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "DueDateMethod" text NOT NULL,
        "NumberOfDays" integer,
        "IsAccountsReceivable" boolean NOT NULL,
        "IsAccountsPayable" boolean NOT NULL,
        "IsMobileVisible" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPaymentTerm" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPostalCode" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "PostalCode" text NOT NULL,
        "FgsSetupZoneId" bigint,
        "FgsSetupTaxId" bigint,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPostalCode" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPriceSheet" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "EffectiveFrom" date NOT NULL,
        "EffectiveTo" date,
        "IsMobileVisible" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPriceSheet" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPriceSheetLabor" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupPriceSheetId" bigint NOT NULL,
        "FgsSetupTechSkillLevelId" bigint,
        "RateType" text NOT NULL,
        "BaseRate" numeric NOT NULL,
        "OvertimeMultiplier" numeric,
        "DoubleTimeMultiplier" numeric,
        "DiscountPercent" numeric,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPriceSheetLabor" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPriceSheetLaborTier" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupPriceSheetLaborId" bigint NOT NULL,
        "SequenceOrder" integer NOT NULL,
        "DurationMinutes" integer NOT NULL,
        "Rate" numeric NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPriceSheetLaborTier" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPriceSheetMaterial" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupPriceSheetId" bigint NOT NULL,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "DefaultMarkupPercent" numeric,
        "DefaultDiscountPercent" numeric,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPriceSheetMaterial" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPriceSheetMaterialRange" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupPriceSheetMaterialId" bigint NOT NULL,
        "CostFrom" numeric NOT NULL,
        "CostTo" numeric NOT NULL,
        "MarkupPercent" numeric NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPriceSheetMaterialRange" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupPriceSheetOther" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupPriceSheetId" bigint NOT NULL,
        "CategoryCode" text NOT NULL,
        "Name" text NOT NULL,
        "MarkupPercent" numeric,
        "DiscountPercent" numeric,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupPriceSheetOther" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupServiceAssetManufacturer" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "Description" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupServiceAssetManufacturer" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupServiceAssetMedia" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupServiceAssetTypeId" bigint,
        "Title" text NOT NULL,
        "MediaUrl" text NOT NULL,
        "ContentType" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupServiceAssetMedia" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupServiceAssetModelSerialDescription" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupServiceAssetManufacturerId" bigint,
        "ModelDescription" text NOT NULL,
        "SerialNumberPattern" text,
        "Notes" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupServiceAssetModelSerialDescription" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupServiceAssetType" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "Description" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupServiceAssetType" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupTax" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TaxCode" text NOT NULL,
        "Name" text NOT NULL,
        "Description" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupTax" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupTaxAuthority" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "RegionCode" text,
        "Description" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupTaxAuthority" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupTaxDetail" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "FgsSetupTaxId" bigint NOT NULL,
        "FgsSetupTaxAuthorityId" bigint NOT NULL,
        "EffectiveFromDate" date NOT NULL,
        "EffectiveToDate" date,
        "TaxPercent" numeric NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupTaxDetail" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupTechSkillLevel" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "Description" text,
        "SortOrder" integer,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupTechSkillLevel" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupTechTrade" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TradeCode" text NOT NULL,
        "Name" text NOT NULL,
        "Description" text,
        "SortOrder" integer,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupTechTrade" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupTimeSlot" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "BeginTime" interval NOT NULL,
        "EndTime" interval NOT NULL,
        "MarkTechArrivedLateAfter" interval,
        "MarkWorkOrderDelayedCompletionAfter" interval,
        "IsMobileVisible" boolean NOT NULL,
        "IsCustomerPortalVisible" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupTimeSlot" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupTitleOfCourtesy" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "DisplayName" text NOT NULL,
        "SortOrder" integer,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupTitleOfCourtesy" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsSetupZone" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "Code" text NOT NULL,
        "Name" text NOT NULL,
        "Description" text,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_FgsSetupZone" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsTenant" (
        "Id" uuid NOT NULL,
        "TenantCode" character varying(50) NOT NULL,
        "Name" character varying(200) NOT NULL,
        "LegalName" character varying(300),
        "Email" character varying(300),
        "PhoneNumber" character varying(50),
        "Website" character varying(500),
        "PhysicalLocationId" uuid,
        "BillingLocationId" uuid,
        "SubscriptionPlanId" integer,
        "TimeZone" character varying(100),
        "DefaultCurrency" character varying(20),
        "DefaultLanguageId" integer,
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsTenant" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsTenantCompany" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "CompanyGuid" uuid NOT NULL,
        "TenantId" uuid NOT NULL,
        "CompanyNumber" bigint NOT NULL,
        "BusinessTypeId" integer NOT NULL,
        "Code" character varying(100) NOT NULL,
        "Name" character varying(200) NOT NULL,
        "LegalName" character varying(300),
        "Email" character varying(300),
        "PhoneNumber" character varying(50),
        "Website" character varying(500),
        "TaxId" character varying(100),
        "PhysicalLocationId" uuid,
        "BillingLocationId" uuid,
        "FullLogoUrl" character varying(1000),
        "CompactLogoUrl" character varying(1000),
        "IconLogoUrl" character varying(1000),
        "FaviconUrl" character varying(1000),
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsTenantCompany" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE TABLE dbo."FgsTenantCompanyConfiguration" (
        "TenantId" uuid NOT NULL,
        "CompanyId" bigint NOT NULL,
        "TimeCardOptionId" integer NOT NULL,
        "AccountingIntegrationTypeId" integer,
        "EnableCallBookingWidget" boolean NOT NULL,
        "EnablePaymentWidget" boolean NOT NULL,
        "EnableCustomerPortal" boolean NOT NULL,
        "EnableRulesManagement" boolean NOT NULL,
        "EnableAutoArrive" boolean NOT NULL,
        "WorkLocationRadiusForAutoArrive" integer,
        "OTStartTime" interval,
        "OTEndTime" interval,
        "DTStartTime" interval,
        "DTEndTime" interval,
        "BillHoursFromDispatchOrArrive" character varying(20) NOT NULL,
        "SourceCodeRequiredOnWorkOrder" boolean NOT NULL,
        "SourceCodeRequiredOnServiceLocation" boolean NOT NULL,
        "BillToStartNumber" bigint NOT NULL,
        "POStartNumber" bigint NOT NULL,
        "QuoteStartNumber" bigint NOT NULL,
        "WorkOrderStartNumber" bigint NOT NULL,
        "InvoiceNumberPrefix" character varying(20),
        "QuoteNumberPrefix" character varying(20),
        "PONumberPrefix" character varying(20),
        "WorkOrderNumberPrefix" character varying(20),
        "InvoiceBatchNumberFormat" character varying(200),
        "IsActive" boolean NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" uuid,
        "UpdatedOn" timestamptz,
        "UpdatedBy" uuid,
        CONSTRAINT "PK_FgsTenantCompanyConfiguration" PRIMARY KEY ("TenantId", "CompanyId")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_FgsCredentialProvider_TenantId_Code" ON dbo."FgsCredentialProvider" ("TenantId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsLocation_EntityTypeId" ON dbo."FgsLocation" ("EntityTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId" ON dbo."FgsSetupCommunicationTemplate" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupCommunicationToken_TenantId_CompanyId" ON dbo."FgsSetupCommunicationToken" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupDescription_TenantId_CompanyId" ON dbo."FgsSetupDescription" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupGLBreak_TenantId_CompanyId" ON dbo."FgsSetupGLBreak" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPaymentMethod_TenantId_CompanyId" ON dbo."FgsSetupPaymentMethod" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPaymentTerm_TenantId_CompanyId" ON dbo."FgsSetupPaymentTerm" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPostalCode_TenantId_CompanyId" ON dbo."FgsSetupPostalCode" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPriceSheet_TenantId_CompanyId" ON dbo."FgsSetupPriceSheet" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPriceSheetLabor_TenantId_CompanyId" ON dbo."FgsSetupPriceSheetLabor" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPriceSheetLaborTier_TenantId_CompanyId" ON dbo."FgsSetupPriceSheetLaborTier" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId" ON dbo."FgsSetupPriceSheetMaterial" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPriceSheetMaterialRange_TenantId_CompanyId" ON dbo."FgsSetupPriceSheetMaterialRange" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupPriceSheetOther_TenantId_CompanyId" ON dbo."FgsSetupPriceSheetOther" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId" ON dbo."FgsSetupServiceAssetManufacturer" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupServiceAssetMedia_TenantId_CompanyId" ON dbo."FgsSetupServiceAssetMedia" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupServiceAssetModelSerialDescription_TenantId_Company~" ON dbo."FgsSetupServiceAssetModelSerialDescription" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupServiceAssetType_TenantId_CompanyId" ON dbo."FgsSetupServiceAssetType" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupTax_TenantId_CompanyId" ON dbo."FgsSetupTax" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupTaxAuthority_TenantId_CompanyId" ON dbo."FgsSetupTaxAuthority" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupTaxDetail_TenantId_CompanyId" ON dbo."FgsSetupTaxDetail" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupTechSkillLevel_TenantId_CompanyId" ON dbo."FgsSetupTechSkillLevel" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupTechTrade_TenantId_CompanyId" ON dbo."FgsSetupTechTrade" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupTimeSlot_TenantId_CompanyId" ON dbo."FgsSetupTimeSlot" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId" ON dbo."FgsSetupTitleOfCourtesy" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE INDEX "IX_FgsSetupZone_TenantId_CompanyId" ON dbo."FgsSetupZone" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_FgsTenant_TenantCode" ON dbo."FgsTenant" ("TenantCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_FgsTenantCompany_TenantId_Code" ON dbo."FgsTenantCompany" ("TenantId", "Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_FgsTenantCompany_TenantId_CompanyNumber" ON dbo."FgsTenantCompany" ("TenantId", "CompanyNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260510183547_InitialCreate') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260510183547_InitialCreate', '10.0.0');
    END IF;
END $EF$;
COMMIT;

