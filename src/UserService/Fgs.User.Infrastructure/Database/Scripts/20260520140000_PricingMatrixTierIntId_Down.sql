-- =============================================================================
-- Migration: 20260520140000_PricingMatrixTierIntId (rollback)
-- Restores uuid PK tier tables (pre–integer identity).
-- =============================================================================

START TRANSACTION;

DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixLaborTier" CASCADE;
DROP TABLE IF EXISTS dbo."FgsSetupPricingMatrixMaterialTier" CASCADE;

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

COMMIT;
