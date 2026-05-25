START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP CONSTRAINT "FK_FgsSetupGLBreak_FgsFile_LogoFileId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP CONSTRAINT "FK_FgsSetupGLBreak_FgsLocation_AddressId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."GloJobTypeSubCategory" DROP CONSTRAINT "FK_GloJobTypeSubCategory_GloBusinessType_BusinessTypeId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."FgsBillingCategory";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."FgsBusinessType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."FgsSetupLaborRateType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."GloSetupPaymentTerm";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_GloSeedTableMapping_SeedCode";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."UX_GloSeedTableMapping_SeedCode_TargetTableName";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_GloJobTypeSubCategory_BusinessTypeId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsTenant_TenantGuid";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" DROP CONSTRAINT "UQ_FgsSetupPaymentMethod";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsSetupPaymentMethod_TenantId_CompanyId_IsActive";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP CONSTRAINT "UQ_FgsSetupGLBreak";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsSetupGLBreak_AddressId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsSetupGLBreak_LogoFileId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsSetupGLBreak_TenantId_CompanyId_BreakLevel";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP CONSTRAINT "CK_FgsSetupGLBreak_BreakLevel";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."GloJobTypeSubCategory" DROP COLUMN "BusinessTypeId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsTenant" DROP COLUMN "TenantGuid";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" DROP COLUMN "SortOrder";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "AddressId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "LogoFileId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "Trades";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."GloSetupDescriptionType";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."GloSetupDescriptionType" (
        "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
        "Code" character varying(100) NOT NULL,
        "Name" character varying(200) NOT NULL,
        "Description" text,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
        "UpdatedOn" timestamptz,
        CONSTRAINT "PK_GloSetupDescriptionType" PRIMARY KEY ("Id"),
        CONSTRAINT "UQ_GloSetupDescriptionType_Code" UNIQUE ("Code")
    );
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ALTER COLUMN "IsMobileVisible" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ALTER COLUMN "IsCustomerPortalVisible" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD "GloPaymentMethodTypeId" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ALTER COLUMN "BreakLevel" TYPE integer;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD "LogoUrl" text;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD CONSTRAINT "UQ_FgsSetupPaymentMethod" UNIQUE ("TenantId", "CompanyId", "GloPaymentMethodTypeId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD CONSTRAINT "UQ_FgsSetupGLBreak" UNIQUE ("TenantId", "CompanyId", "Code");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."FgsSetupGLBreakTechTrade" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "FgsSetupGLBreakId" bigint NOT NULL,
        "FgsSetupTechTradeId" bigint NOT NULL,
        "CreatedBy" character varying(100),
        "CreatedOn" timestamptz NOT NULL,
        "IsActive" boolean NOT NULL,
        "UpdatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        CONSTRAINT "PK_FgsSetupGLBreakTechTrade" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsSetupGLBreakTechTrade_FgsSetupGLBreak_FgsSetupGLBreakId" FOREIGN KEY ("FgsSetupGLBreakId") REFERENCES dbo."FgsSetupGLBreak" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsSetupGLBreakTechTrade_FgsSetupTechTrade_FgsSetupTechTrad~" FOREIGN KEY ("FgsSetupTechTradeId") REFERENCES dbo."FgsSetupTechTrade" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsSetupGLBreakTechTrade_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE RESTRICT
    );
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."GloJobTypeCategorySubCategory" (
        "BusinessTypeId" integer NOT NULL,
        "CategoryId" smallint NOT NULL,
        "SubCategoryId" smallint NOT NULL,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        CONSTRAINT "PK_GloJobTypeCategorySubCategory" PRIMARY KEY ("BusinessTypeId", "CategoryId", "SubCategoryId"),
        CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloBusinessType_BusinessTypeId" FOREIGN KEY ("BusinessTypeId") REFERENCES dbo."GloBusinessType" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloJobTypeCategory_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES dbo."GloJobTypeCategory" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloJobTypeSubCategory_SubCategoryId" FOREIGN KEY ("SubCategoryId") REFERENCES dbo."GloJobTypeSubCategory" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE UNIQUE INDEX "UX_GloSeedTableMapping_SeedCode" ON dbo."GloSeedTableMapping" ("SeedCode");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId" ON dbo."FgsSetupPaymentMethod" ("GloPaymentMethodTypeId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupPaymentMethod_TenantId_CompanyId" ON dbo."FgsSetupPaymentMethod" ("TenantId", "CompanyId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupGLBreak_TenantId_CompanyId" ON dbo."FgsSetupGLBreak" ("TenantId", "CompanyId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE UNIQUE INDEX "IX_FgsSetupGLBreakTechTrade_FgsSetupGLBreakId_FgsSetupTechTrad~" ON dbo."FgsSetupGLBreakTechTrade" ("FgsSetupGLBreakId", "FgsSetupTechTradeId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupGLBreakTechTrade_FgsSetupTechTradeId" ON dbo."FgsSetupGLBreakTechTrade" ("FgsSetupTechTradeId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupGLBreakTechTrade_TenantId_CompanyId" ON dbo."FgsSetupGLBreakTechTrade" ("TenantId", "CompanyId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_GloJobTypeCategorySubCategory_CategoryId" ON dbo."GloJobTypeCategorySubCategory" ("CategoryId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_GloJobTypeCategorySubCategory_SubCategoryId" ON dbo."GloJobTypeCategorySubCategory" ("SubCategoryId");
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD CONSTRAINT "FK_FgsSetupPaymentMethod_GloPayType" FOREIGN KEY ("GloPaymentMethodTypeId") REFERENCES dbo."GloPaymentMethodType" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DELETE FROM dbo."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates';
    END IF;
END $EF$;
COMMIT;

