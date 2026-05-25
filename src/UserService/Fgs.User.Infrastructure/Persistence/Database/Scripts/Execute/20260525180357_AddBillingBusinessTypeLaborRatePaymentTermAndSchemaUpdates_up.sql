START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" DROP CONSTRAINT "FK_FgsSetupPaymentMethod_GloPayType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."FgsSetupGLBreakTechTrade";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."GloJobTypeCategorySubCategory";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."UX_GloSeedTableMapping_SeedCode";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" DROP CONSTRAINT "UQ_FgsSetupPaymentMethod";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsSetupPaymentMethod_TenantId_CompanyId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP CONSTRAINT "UQ_FgsSetupGLBreak";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP INDEX dbo."IX_FgsSetupGLBreak_TenantId_CompanyId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" DROP COLUMN "GloPaymentMethodTypeId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "LogoUrl";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    DROP TABLE dbo."GloSetupDescriptionType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."GloSetupDescriptionType" (
        "Id" smallint GENERATED ALWAYS AS IDENTITY,
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
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."GloJobTypeSubCategory" ADD "BusinessTypeId" integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsTenant" ADD "TenantGuid" uuid NOT NULL DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ALTER COLUMN "IsMobileVisible" SET DEFAULT TRUE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ALTER COLUMN "IsCustomerPortalVisible" SET DEFAULT TRUE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD "SortOrder" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ALTER COLUMN "BreakLevel" TYPE smallint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD "AddressId" uuid;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD "LogoFileId" bigint;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD "Trades" text[];
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupPaymentMethod" ADD CONSTRAINT "UQ_FgsSetupPaymentMethod" UNIQUE ("TenantId", "CompanyId", "DisplayName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD CONSTRAINT "UQ_FgsSetupGLBreak" UNIQUE ("TenantId", "CompanyId", "Code", "BreakLevel");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."FgsBillingCategory" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "BillingCategoryType" character varying(2) NOT NULL,
        "BillingCategoryName" character varying(100) NOT NULL,
        "Description" text,
        "DisplayOrder" smallint NOT NULL DEFAULT 1,
        "IsSystemDefined" boolean NOT NULL DEFAULT FALSE,
        "ShowToFieldTech" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT "PK_FgsBillingCategory" PRIMARY KEY ("Id"),
        CONSTRAINT "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType" UNIQUE ("TenantId", "CompanyId", "BillingCategoryType"),
        CONSTRAINT "FK_FgsBillingCategory_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."FgsBusinessType" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Code" character varying(100) NOT NULL,
        "Name" character varying(200) NOT NULL,
        "Description" text,
        "DisplayOrder" smallint NOT NULL DEFAULT 1,
        "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT "PK_FgsBusinessType" PRIMARY KEY ("Id"),
        CONSTRAINT "UQ_FgsBusinessType_TenantId_CompanyId_Code" UNIQUE ("TenantId", "CompanyId", "Code"),
        CONSTRAINT "FK_FgsBusinessType_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."FgsSetupLaborRateType" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Name" text NOT NULL,
        "Description" text,
        "SortOrder" integer NOT NULL DEFAULT 0,
        "IsSystem" boolean NOT NULL DEFAULT FALSE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT "PK_FgsSetupLaborRateType" PRIMARY KEY ("Id"),
        CONSTRAINT "UQ_FgsSetupLaborRateType_TenantId_CompanyId_Name" UNIQUE ("TenantId", "CompanyId", "Name"),
        CONSTRAINT "FK_FgsSetupLaborRateType_FgsTenantCompany_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE TABLE dbo."GloSetupPaymentTerm" (
        "Id" smallint GENERATED ALWAYS AS IDENTITY,
        "Name" text NOT NULL,
        "DueDateMethod" text NOT NULL,
        "NumberOfDays" integer,
        "IsAccountsReceivable" boolean NOT NULL DEFAULT TRUE,
        "IsAccountsPayable" boolean NOT NULL DEFAULT FALSE,
        "IsMobileVisible" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT "PK_GloSetupPaymentTerm" PRIMARY KEY ("Id"),
        CONSTRAINT "UQ_GloSetupPaymentTerm_Name" UNIQUE ("Name")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_GloSeedTableMapping_SeedCode" ON dbo."GloSeedTableMapping" ("SeedCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE UNIQUE INDEX "UX_GloSeedTableMapping_SeedCode_TargetTableName" ON dbo."GloSeedTableMapping" ("SeedCode", "TargetTableName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_GloJobTypeSubCategory_BusinessTypeId" ON dbo."GloJobTypeSubCategory" ("BusinessTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE UNIQUE INDEX "IX_FgsTenant_TenantGuid" ON dbo."FgsTenant" ("TenantGuid");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupPaymentMethod_TenantId_CompanyId_IsActive" ON dbo."FgsSetupPaymentMethod" ("TenantId", "CompanyId", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupGLBreak_AddressId" ON dbo."FgsSetupGLBreak" ("AddressId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupGLBreak_LogoFileId" ON dbo."FgsSetupGLBreak" ("LogoFileId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupGLBreak_TenantId_CompanyId_BreakLevel" ON dbo."FgsSetupGLBreak" ("TenantId", "CompanyId", "BreakLevel");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD CONSTRAINT "CK_FgsSetupGLBreak_BreakLevel" CHECK ("BreakLevel" IN (1, 2));
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsBillingCategory_TenantId_CompanyId_IsActive" ON dbo."FgsBillingCategory" ("TenantId", "CompanyId", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsBusinessType_TenantId_CompanyId_IsActive" ON dbo."FgsBusinessType" ("TenantId", "CompanyId", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    CREATE INDEX "IX_FgsSetupLaborRateType_TenantId_CompanyId_IsActive" ON dbo."FgsSetupLaborRateType" ("TenantId", "CompanyId", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD CONSTRAINT "FK_FgsSetupGLBreak_FgsFile_LogoFileId" FOREIGN KEY ("LogoFileId") REFERENCES dbo."FgsFile" ("Id") ON DELETE SET NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."FgsSetupGLBreak" ADD CONSTRAINT "FK_FgsSetupGLBreak_FgsLocation_AddressId" FOREIGN KEY ("AddressId") REFERENCES dbo."FgsLocation" ("Id") ON DELETE SET NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    ALTER TABLE dbo."GloJobTypeSubCategory" ADD CONSTRAINT "FK_GloJobTypeSubCategory_GloBusinessType_BusinessTypeId" FOREIGN KEY ("BusinessTypeId") REFERENCES dbo."GloBusinessType" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    COMMENT ON TABLE dbo."FgsSetupGLBreak" IS 'Stores GL break configuration for financial reporting segmentation by trade, location, or organizational unit.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Id" IS 'Surrogate primary key.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."TenantId" IS 'Owning tenant identifier.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Code" IS 'Unique GL break code within tenant, company, and break level scope.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Name" IS 'Display name of the GL break.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."BreakLabel" IS 'Optional label shown in UI for the break.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."BreakLevel" IS 'Break hierarchy level. Allowed values: 1, 2.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Trades" IS 'Optional array of technician trade codes associated with this GL break.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."LogoFileId" IS 'Optional reference to uploaded logo file in FgsFile.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."AddressId" IS 'Optional reference to break address in FgsLocation.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."IsActive" IS 'Indicates whether the GL break is active.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CreatedOn" IS 'UTC timestamp when the record was created.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CreatedBy" IS 'User or process that created the record.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."UpdatedOn" IS 'UTC timestamp of the last update.';
    COMMENT ON COLUMN dbo."FgsSetupGLBreak"."UpdatedBy" IS 'User or process that last updated the record.';

    COMMENT ON TABLE dbo."FgsBillingCategory" IS 'Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."Id" IS 'Primary key identity of the billing category record.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."TenantId" IS 'Tenant identifier owning this billing category.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."CompanyId" IS 'Company identifier within the tenant owning this billing category.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."BillingCategoryType" IS 'Short billing category code such as IN, PM, SR, or other tenant-defined values.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."BillingCategoryName" IS 'Display name of the billing category shown throughout the application.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."Description" IS 'Optional internal description or notes for the billing category.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."DisplayOrder" IS 'Controls sorting/display order of billing categories in dropdowns and setup screens.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."IsSystemDefined" IS 'Indicates whether the billing category was system seeded or manually created by the tenant/company.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."ShowToFieldTech" IS 'Indicates whether the billing category is visible to field technicians in mobile and field service applications.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."IsActive" IS 'Indicates whether the billing category is active and available for use.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."CreatedOn" IS 'Date and time the billing category record was created.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."CreatedBy" IS 'User identifier that created the billing category record.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."UpdatedOn" IS 'Date and time the billing category record was last updated.';
    COMMENT ON COLUMN dbo."FgsBillingCategory"."UpdatedBy" IS 'User identifier that last updated the billing category record.';

    COMMENT ON TABLE dbo."FgsBusinessType" IS 'Stores tenant/company specific business types used throughout the application. Seeded initially from GloBusinessType but independently managed by each tenant/company.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."Id" IS 'Primary key identity of the business type record.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."TenantId" IS 'Tenant identifier owning this business type.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."CompanyId" IS 'Company identifier within the tenant owning this business type.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."Code" IS 'Unique business type code within the tenant/company.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."Name" IS 'Display name of the business type shown throughout the application.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."Description" IS 'Optional internal description or notes for the business type.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."DisplayOrder" IS 'Controls sorting/display order of business types in dropdowns and setup screens.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."IsActive" IS 'Indicates whether the business type is active and available for use.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."CreatedOn" IS 'Date and time the business type record was created.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."UpdatedOn" IS 'Date and time the business type record was last updated.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."CreatedBy" IS 'User identifier that created the business type record.';
    COMMENT ON COLUMN dbo."FgsBusinessType"."UpdatedBy" IS 'User identifier that last updated the business type record.';

    COMMENT ON COLUMN dbo."GloJobTypeSubCategory"."BusinessTypeId" IS 'Optional business type associated with this job type subcategory. NULL means shared across all business types.';

    COMMENT ON TABLE dbo."GloSetupDescriptionType" IS 'Stores global setup description types used throughout the system for organizing setup descriptions and configuration text.';
    COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Id" IS 'Primary key identity of the setup description type record.';
    COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Code" IS 'Unique code representing the setup description type.';
    COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Name" IS 'Display name of the setup description type.';
    COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Description" IS 'Optional description or notes for the setup description type.';
    COMMENT ON COLUMN dbo."GloSetupDescriptionType"."IsActive" IS 'Indicates whether the setup description type is active and available for use.';
    COMMENT ON COLUMN dbo."GloSetupDescriptionType"."CreatedOn" IS 'Date and time the setup description type record was created.';
    COMMENT ON COLUMN dbo."GloSetupDescriptionType"."UpdatedOn" IS 'Date and time the setup description type record was last updated.';

    COMMENT ON TABLE dbo."FgsSetupLaborRateType" IS 'Stores tenant/company specific labor rate types used for pricing, billing, overtime, emergency rates, and other labor configurations. Seeded initially from GloSetupLaborRateType but independently managed by each tenant/company.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."Id" IS 'Primary key identity of the labor rate type record.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."TenantId" IS 'Tenant identifier owning this labor rate type.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."CompanyId" IS 'Company identifier within the tenant owning this labor rate type.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."Name" IS 'Display name of the labor rate type.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."Description" IS 'Optional description or notes for the labor rate type.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."SortOrder" IS 'Controls sorting/display order of labor rate types in dropdowns and setup screens.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."IsSystem" IS 'Indicates whether the labor rate type was seeded by the system or manually created by the tenant/company.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."IsActive" IS 'Indicates whether the labor rate type is active and available for use.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."CreatedOn" IS 'Date and time the labor rate type record was created.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."CreatedBy" IS 'User identifier that created the labor rate type record.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."UpdatedOn" IS 'Date and time the labor rate type record was last updated.';
    COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."UpdatedBy" IS 'User identifier that last updated the labor rate type record.';

    COMMENT ON TABLE dbo."GloSetupPaymentTerm" IS 'Stores global payment term master data used to seed tenant/company payment terms for accounts receivable and accounts payable operations.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."Id" IS 'Primary key identity of the payment term record.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."Name" IS 'Display name of the payment term.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."DueDateMethod" IS 'Defines how the due date is calculated such as NetDays, EndOfMonth, DueOnReceipt, or FixedDayOfMonth.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."NumberOfDays" IS 'Number of days used for due date calculations when applicable.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsAccountsReceivable" IS 'Indicates whether the payment term is available for customer invoicing/accounts receivable.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsAccountsPayable" IS 'Indicates whether the payment term is available for vendor billing/accounts payable.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsMobileVisible" IS 'Indicates whether the payment term is visible in mobile applications.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."CreatedOn" IS 'Date and time the payment term record was created.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."CreatedBy" IS 'User identifier that created the payment term record.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."UpdatedOn" IS 'Date and time the payment term record was last updated.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."UpdatedBy" IS 'User identifier that last updated the payment term record.';
    COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsActive" IS 'Indicates whether the payment term is active and available for use.';

    COMMENT ON TABLE dbo."FgsSetupPaymentMethod" IS 'Stores tenant/company specific payment methods used throughout invoicing, customer payments, vendor payments, mobile applications, and customer portals.';
    COMMENT ON COLUMN dbo."FgsSetupPaymentMethod"."SortOrder" IS 'Controls sorting/display order of payment methods in dropdowns and setup screens.';

    COMMENT ON COLUMN dbo."FgsTenant"."TenantGuid" IS 'Stable UUID identifier for the tenant used in external integrations and cross-service references.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo."__EFMigrationsHistory" WHERE "MigrationId" = '20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates') THEN
    INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260525180357_AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates', '10.0.8');
    END IF;
END $EF$;
COMMIT;

