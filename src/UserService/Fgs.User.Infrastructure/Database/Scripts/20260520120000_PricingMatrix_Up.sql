-- =============================================================================
-- Migration: 20260520120000_PricingMatrix
-- Script:   Database/Scripts/20260520120000_PricingMatrix_Up.sql
-- Pair with: Database/Migrations/20260520120000_PricingMatrix.cs
-- Replaces legacy FgsSetupPriceSheet* tables with FgsSetupPricingMatrix* per
-- CleanUpTables-ForCursorAI.md (uuid primary/foreign keys).
-- =============================================================================

START TRANSACTION;

-- Drop legacy / partial tables (children first)
DROP TABLE IF EXISTS dbo."FgsSetupPriceSheetLaborTier" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPriceSheetLabor" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPriceSheetMaterial" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPriceSheetOther" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPriceSheet" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixLaborTier" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixLabor" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixMaterialTier" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixOther" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrix" CASCADE;

-- ---------------------------------------------------------------------------
-- FgsSetupPricingMatrix
-- ---------------------------------------------------------------------------
CREATE TABLE dbo."FgsSetupPricingMatrix"
(
    "Id" uuid NOT NULL,

    "TenantId" uuid NOT NULL,
    "CompanyId" bigint NOT NULL,

    "Code" text NOT NULL,
    "Name" text NOT NULL,

    "IsLaborTierStructure" boolean NOT NULL DEFAULT false,
    "IsLaborRateBySkillLevel" boolean NOT NULL DEFAULT false,

    "EffectiveFrom" date NOT NULL,
    "EffectiveTo" date,

    "IsMobileVisible" boolean NOT NULL DEFAULT true,
    "IsActive" boolean NOT NULL DEFAULT true,

    "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),

    CONSTRAINT "PK_FgsSetupPricingMatrix"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrix_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber")
        ON DELETE RESTRICT,

    CONSTRAINT "UQ_FgsSetupPricingMatrix"
        UNIQUE ("TenantId", "CompanyId", "Code")
);

CREATE INDEX "IX_FgsSetupPricingMatrix_TenantId_CompanyId"
    ON dbo."FgsSetupPricingMatrix" ("TenantId", "CompanyId");

-- ---------------------------------------------------------------------------
-- FgsSetupPricingMatrixLabor
-- ---------------------------------------------------------------------------
CREATE TABLE dbo."FgsSetupPricingMatrixLabor"
(
    "Id" uuid NOT NULL,

    "TenantId" uuid NOT NULL,
    "CompanyId" bigint NOT NULL,

    "FgsSetupPricingMatrixId" uuid NOT NULL,
    "FgsSetupLaborRateTypeId" integer NOT NULL,
    "FgsSetupTechSkillLevelId" bigint,

    "BaseRate" numeric(18, 2) NOT NULL,
    "OvertimeMultiplier" numeric(18, 2),
    "DoubleTimeMultiplier" numeric(18, 2),
    "DiscountPercent" numeric(18, 2),

    "IsActive" boolean NOT NULL DEFAULT true,

    "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),

    CONSTRAINT "PK_FgsSetupPricingMatrixLabor"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_PricingMatrix"
        FOREIGN KEY ("FgsSetupPricingMatrixId")
        REFERENCES dbo."FgsSetupPricingMatrix" ("Id")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_LaborRateType"
        FOREIGN KEY ("FgsSetupLaborRateTypeId")
        REFERENCES dbo."GloSetupLaborRateType" ("Id")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_TechSkillLevel"
        FOREIGN KEY ("FgsSetupTechSkillLevelId")
        REFERENCES dbo."FgsSetupTechSkillLevel" ("Id")
        ON DELETE RESTRICT
);

CREATE INDEX "IX_FgsSetupPricingMatrixLabor_TenantId_CompanyId"
    ON dbo."FgsSetupPricingMatrixLabor" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSetupPricingMatrixLabor_FgsSetupPricingMatrixId"
    ON dbo."FgsSetupPricingMatrixLabor" ("FgsSetupPricingMatrixId");

CREATE INDEX "IX_FgsSetupPricingMatrixLabor_FgsSetupLaborRateTypeId"
    ON dbo."FgsSetupPricingMatrixLabor" ("FgsSetupLaborRateTypeId");

CREATE INDEX "IX_FgsSetupPricingMatrixLabor_FgsSetupTechSkillLevelId"
    ON dbo."FgsSetupPricingMatrixLabor" ("FgsSetupTechSkillLevelId");

-- ---------------------------------------------------------------------------
-- FgsSetupPricingMatrixLaborTier
-- ---------------------------------------------------------------------------
CREATE TABLE dbo."FgsSetupPricingMatrixLaborTier"
(
    "Id" uuid NOT NULL,

    "TenantId" uuid NOT NULL,
    "CompanyId" bigint NOT NULL,

    "FgsSetupPricingMatrixLaborId" uuid NOT NULL,

    "SequenceOrder" integer NOT NULL,
    "DurationMinutes" integer NOT NULL,
    "Rate" numeric(18, 2) NOT NULL,

    "IsActive" boolean NOT NULL DEFAULT true,

    "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),

    CONSTRAINT "PK_FgsSetupPricingMatrixLaborTier"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLaborTier_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_FgsSetupPricingMatrixLaborTier_Labor"
        FOREIGN KEY ("FgsSetupPricingMatrixLaborId")
        REFERENCES dbo."FgsSetupPricingMatrixLabor" ("Id")
        ON DELETE RESTRICT
);

CREATE INDEX "IX_FgsSetupPricingMatrixLaborTier_TenantId_CompanyId"
    ON dbo."FgsSetupPricingMatrixLaborTier" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSetupPricingMatrixLaborTier_FgsSetupPricingMatrixLaborId"
    ON dbo."FgsSetupPricingMatrixLaborTier" ("FgsSetupPricingMatrixLaborId");

-- ---------------------------------------------------------------------------
-- FgsSetupPricingMatrixMaterialTier
-- ---------------------------------------------------------------------------
CREATE TABLE dbo."FgsSetupPricingMatrixMaterialTier"
(
    "Id" uuid NOT NULL,

    "TenantId" uuid NOT NULL,
    "CompanyId" bigint NOT NULL,

    "FgsSetupPricingMatrixId" uuid NOT NULL,

    "FromCost" numeric(18, 2) NOT NULL,
    "ToCost" numeric(18, 2),
    "MarkupPercent" numeric(18, 2) NOT NULL,
    "DiscountPercent" numeric(18, 2),

    "IsActive" boolean NOT NULL DEFAULT true,

    "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),

    CONSTRAINT "PK_FgsSetupPricingMatrixMaterialTier"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixMaterialTier_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_FgsSetupPricingMatrixMaterialTier_PricingMatrix"
        FOREIGN KEY ("FgsSetupPricingMatrixId")
        REFERENCES dbo."FgsSetupPricingMatrix" ("Id")
        ON DELETE RESTRICT,

    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_FromCost"
        CHECK ("FromCost" >= 0),

    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_ToCost"
        CHECK ("ToCost" IS NULL OR "ToCost" >= "FromCost"),

    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_MarkupPercent"
        CHECK ("MarkupPercent" >= 0),

    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_DiscountPercent"
        CHECK (
            "DiscountPercent" IS NULL
            OR ("DiscountPercent" >= 0 AND "DiscountPercent" <= 100)
        )
);

CREATE INDEX "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId"
    ON dbo."FgsSetupPricingMatrixMaterialTier" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSetupPricingMatrixMaterialTier_FgsSetupPricingMatrixId"
    ON dbo."FgsSetupPricingMatrixMaterialTier" ("FgsSetupPricingMatrixId");

-- ---------------------------------------------------------------------------
-- FgsSetupPricingMatrixOther
-- ---------------------------------------------------------------------------
CREATE TABLE dbo."FgsSetupPricingMatrixOther"
(
    "Id" uuid NOT NULL,

    "TenantId" uuid NOT NULL,
    "CompanyId" bigint NOT NULL,

    "FgsSetupPricingMatrixId" uuid NOT NULL,

    "CategoryCode" text NOT NULL,
    "Name" text NOT NULL,

    "MarkupPercent" numeric(18, 2),
    "DiscountPercent" numeric(18, 2),

    "IsActive" boolean NOT NULL DEFAULT true,

    "CreatedOn" timestamptz NOT NULL DEFAULT (timezone('utc', now())),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),

    CONSTRAINT "PK_FgsSetupPricingMatrixOther"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixOther_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_FgsSetupPricingMatrixOther_PricingMatrix"
        FOREIGN KEY ("FgsSetupPricingMatrixId")
        REFERENCES dbo."FgsSetupPricingMatrix" ("Id")
        ON DELETE RESTRICT,

    CONSTRAINT "UQ_FgsSetupPricingMatrixOther"
        UNIQUE ("TenantId", "CompanyId", "FgsSetupPricingMatrixId", "CategoryCode"),

    CONSTRAINT "CK_FgsSetupPricingMatrixOther_MarkupPercent"
        CHECK ("MarkupPercent" IS NULL OR "MarkupPercent" >= 0),

    CONSTRAINT "CK_FgsSetupPricingMatrixOther_DiscountPercent"
        CHECK (
            "DiscountPercent" IS NULL
            OR ("DiscountPercent" >= 0 AND "DiscountPercent" <= 100)
        )
);

CREATE INDEX "IX_FgsSetupPricingMatrixOther_TenantId_CompanyId"
    ON dbo."FgsSetupPricingMatrixOther" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSetupPricingMatrixOther_FgsSetupPricingMatrixId"
    ON dbo."FgsSetupPricingMatrixOther" ("FgsSetupPricingMatrixId");

INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260520120000_PricingMatrix', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
