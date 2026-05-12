# FGS Database Table Structures

> Master database schema reference for developers and Cursor AI.

---

## Purpose

This document contains:
- PostgreSQL table structures
- Foreign keys
- Constraints
- Indexes
- Seed data
- Enterprise design notes

Use this as the primary schema reference for:
- Backend API development
- Entity modeling
- EF Core mappings
- Cursor AI context
- Migration generation
- Database reviews

---

# Pasted text.txt

```sql
------------------ FgsCredentialAudit

CREATE TABLE IF NOT EXISTS dbo."FgsCredentialAudit"
(
    "Id" uuid NOT NULL,

    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    "CredentialSecretId" uuid NOT NULL,

    "ActionType" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Remarks" character varying(1000) COLLATE pg_catalog."default",

    "OldVersionNo" integer,
    "NewVersionNo" integer,

    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    CONSTRAINT "PK_FgsCredentialAudit"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsCredentialAudit_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsCredentialAudit_CredentialSecret"
        FOREIGN KEY ("CredentialSecretId")
        REFERENCES dbo."FgsCredentialSecret" ("Id"),

    CONSTRAINT "UQ_FgsCredentialAudit"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "CredentialSecretId",
            "ActionType",
            "NewVersionNo"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsCredentialAudit"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsCredentialAudit_Tenant_Company"
    ON dbo."FgsCredentialAudit"
    (
        "TenantId",
        "CompanyId"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsCredentialAudit_Tenant_Company_Credential"
    ON dbo."FgsCredentialAudit"
    (
        "TenantId",
        "CompanyId",
        "CredentialSecretId"
    );

------------------ FgsCredentialProviderConfiguration

CREATE TABLE IF NOT EXISTS dbo."FgsCredentialProviderConfiguration"
(
    "Id" uuid NOT NULL,

    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    "CredentialProviderId" uuid NOT NULL,

    "ConfigurationKey" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    "ConfigurationValue" text COLLATE pg_catalog."default",

    "Environment" character varying(50) COLLATE pg_catalog."default",

    "IsActive" boolean NOT NULL,

    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsCredentialProviderConfiguration"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsCredentialProviderConfiguration_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsCredentialProviderConfiguration_CredentialProvider"
        FOREIGN KEY ("CredentialProviderId")
        REFERENCES dbo."FgsCredentialProvider" ("Id"),

    CONSTRAINT "UQ_FgsCredentialProviderConfiguration"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "CredentialProviderId",
            "ConfigurationKey",
            "Environment"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsCredentialProviderConfiguration"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsCredentialProviderConfiguration_Tenant_Company"
    ON dbo."FgsCredentialProviderConfiguration"
    (
        "TenantId",
        "CompanyId"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsCredentialProviderConfiguration_Tenant_Company_Provider"
    ON dbo."FgsCredentialProviderConfiguration"
    (
        "TenantId",
        "CompanyId",
        "CredentialProviderId"
    );


------------------ FgsCredentialSecret

-- =========================================================
-- TABLE: FgsCredentialSecret
-- PURPOSE:
-- Stores encrypted credential JSON blobs for a provider
-- using envelope encryption architecture.
-- =========================================================

CREATE TABLE IF NOT EXISTS dbo."FgsCredentialSecret"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Integration provider
    "CredentialProviderId" uuid NOT NULL,

    -- Optional logical name
    -- Example:
    -- Twilio Primary
    -- Stripe Production
    "SecretName" character varying(200) COLLATE pg_catalog."default" NOT NULL,

    -- Encrypted JSON payload
    -- Entire credential object encrypted as ONE blob
    "EncryptedSecretValue" text COLLATE pg_catalog."default" NOT NULL,

    -- Encrypted Data Encryption Key (DEK)
    -- DEK encrypted using AWS KMS master key
    "EncryptedDek" text COLLATE pg_catalog."default" NOT NULL,

    -- AWS KMS Key Identifier
    -- Example:
    -- arn:aws:kms:us-east-1:xxxx:key/xxxxx
    "EncryptionKeyId" character varying(500) COLLATE pg_catalog."default" NOT NULL,

    -- Versioning
    "VersionNo" integer NOT NULL DEFAULT 1,

    -- Rotation tracking
    "LastRotatedOn" timestamp with time zone,
    "ExpiresOn" timestamp with time zone,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,
    "IsRevoked" boolean NOT NULL DEFAULT false,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsCredentialSecret"
        PRIMARY KEY ("Id"),

    -- Tenant + Company FK
    CONSTRAINT "FK_FgsCredentialSecret_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    -- Provider FK
    CONSTRAINT "FK_FgsCredentialSecret_CredentialProvider"
        FOREIGN KEY ("CredentialProviderId")
        REFERENCES dbo."FgsCredentialProvider" ("Id"),

    -- One active version per provider/secret name/version
    CONSTRAINT "UQ_FgsCredentialSecret"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "CredentialProviderId",
            "SecretName",
            "VersionNo"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsCredentialSecret"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsCredentialSecret_Tenant_Company"
    ON dbo."FgsCredentialSecret"
    (
        "TenantId",
        "CompanyId"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsCredentialSecret_Tenant_Company_Provider"
    ON dbo."FgsCredentialSecret"
    (
        "TenantId",
        "CompanyId",
        "CredentialProviderId"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsCredentialSecret_IsActive"
    ON dbo."FgsCredentialSecret"
    (
        "IsActive"
    );

------------------ GloMasterEntityType

CREATE TABLE IF NOT EXISTS dbo."GloMasterEntityType"
(
    "Id" integer NOT NULL,

    -- Short unique code
    -- Example:
    -- CUSTOMER
    -- WORK_ORDER
    -- TECHNICIAN
    "Code" character varying(100) COLLATE pg_catalog."default" NOT NULL,

    -- Indicates whether documents/files
    -- can be attached to this entity type
    "IsDocumentAllowed" boolean NOT NULL DEFAULT false,

    -- Active flag
    "IsActive" boolean NOT NULL DEFAULT true,

    -- UI sorting
    "SortOrder" integer NOT NULL DEFAULT 0,

    -- Audit
    "CreatedOn" timestamp with time zone,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_GloMasterEntityType"
        PRIMARY KEY ("Id"),

    CONSTRAINT "UQ_GloMasterEntityType_Code"
        UNIQUE ("Code")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."GloMasterEntityType"
    OWNER to dbmasteruser;

----- data to seed

INSERT INTO dbo."GloMasterEntityType"
(
    "Id",
    "Code",
    "IsDocumentAllowed",
    "IsActive",
    "SortOrder"
)
VALUES

(1,  'TENANT',            true,  true, 1),
(2,  'TENANT_COMPANY',    true,  true, 2),
(3,  'WORK_ORDER',        true,  true, 3),
(4,  'EMPLOYEE',          true,  true, 4),
(5,  'PURCHASE_ORDER',    true,  true, 5),
(6,  'VENDOR',            true,  true, 6),
(7,  'SUB_CONTRACTOR',    true,  true, 7),
(8,  'BILL_TO',           true,  true, 8),
(9,  'SERVICE_LOCATION',  true,  true, 9),
(10, 'PROPOSAL',          true,  true, 10);


------------------ FgsLocation

CREATE TABLE IF NOT EXISTS dbo."FgsLocation"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Entity Reference
    "MasterEntityTypeId" integer NOT NULL,
    "EntityNumber" bigint,

    -- Address
    "AddressLine1" character varying(200) COLLATE pg_catalog."default",
    "AddressLine2" character varying(200) COLLATE pg_catalog."default",
    "AddressLine3" character varying(200) COLLATE pg_catalog."default",
    "AddressLine4" character varying(200) COLLATE pg_catalog."default",

    "City" character varying(100) COLLATE pg_catalog."default",
    "State" character varying(100) COLLATE pg_catalog."default",
    "County" character varying(100) COLLATE pg_catalog."default",
    "Country" character varying(100) COLLATE pg_catalog."default",

    "PostalCode" character varying(20) COLLATE pg_catalog."default",

    -- Calculated display address
    "FormattedAddress" character varying(1000) COLLATE pg_catalog."default",

    -- Geo Coordinates
    "Latitude" numeric(18,10),
    "Longitude" numeric(18,10),

    -- Google/Map Provider Place Id
    "PlaceId" character varying(500) COLLATE pg_catalog."default",

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsLocation"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsLocation_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsLocation_MasterEntityType"
        FOREIGN KEY ("MasterEntityTypeId")
        REFERENCES dbo."GloMasterEntityType" ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsLocation"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsLocation_Tenant_Company_Entity"
    ON dbo."FgsLocation"
    (
        "TenantId",
        "CompanyId",
        "MasterEntityTypeId",
        "EntityNumber"
    );

------------------ FgsSetupCommunicationTemplate

CREATE TABLE IF NOT EXISTS dbo."FgsSetupCommunicationTemplate"
(
    "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 9223372036854775807
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Template Type
    -- Example:
    -- EMAIL
    -- SMS
    -- PUSH
    "TemplateType" text COLLATE pg_catalog."default" NOT NULL,

    -- Unique business event code
    -- Example:
    -- WORK_ORDER_CREATED
    -- PROPOSAL_APPROVED
    -- PAYMENT_RECEIVED
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- User-friendly template name/description
    -- Example:
    -- Work Order Created Email
    -- Technician En Route SMS
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Email subject
    "Subject" text COLLATE pg_catalog."default",

    -- Template body
    "Body" text COLLATE pg_catalog."default" NOT NULL,

    -- Mobile app visibility
    "IsMobileVisible" boolean NOT NULL DEFAULT false,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupCommunicationTemplate"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupCommunicationTemplate_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupCommunicationTemplate"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "TemplateType",
            "Code"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupCommunicationTemplate"
    OWNER to dbmasteruser;

------------------ GloCommunicationToken [We will seed this later]

CREATE TABLE IF NOT EXISTS dbo."GloCommunicationToken"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Token used inside template
    -- Example:
    -- {{CustomerName}}
    -- {{WorkOrderNumber}}
    "TokenCode" text COLLATE pg_catalog."default" NOT NULL,

    -- User friendly display name
    -- Example:
    -- Customer Name
    -- Work Order Number
    "DisplayName" text COLLATE pg_catalog."default" NOT NULL,

    -- Source table name
    -- Example:
    -- FgsWorkOrder
    -- FgsCustomer
    "SourceTableName" text COLLATE pg_catalog."default" NOT NULL,

    -- Source column name
    -- Example:
    -- WorkOrderNumber
    -- FirstName
    "SourceColumnName" text COLLATE pg_catalog."default" NOT NULL,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_GloCommunicationToken"
        PRIMARY KEY ("Id"),

    CONSTRAINT "UQ_GloCommunicationToken_TokenCode"
        UNIQUE ("TokenCode")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."GloCommunicationToken"
    OWNER to dbmasteruser;

------------------ FgsSetupDescription

CREATE TABLE IF NOT EXISTS dbo."FgsSetupDescription"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Description type
    -- Example:
    -- WORK_ORDER_NOTE
    -- PROPOSAL_NOTE
    -- INVOICE_NOTE
    "DescriptionTypeCode" text COLLATE pg_catalog."default" NOT NULL,

    -- Description body/content
    "Body" text COLLATE pg_catalog."default" NOT NULL,

    -- Optional trade reference
    "FgsSetupTechTradeId" uuid,

    -- UI sorting
    "SortOrder" integer NOT NULL DEFAULT 0,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupDescription"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupDescription_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupDescription_TechTrade"
        FOREIGN KEY ("FgsSetupTechTradeId")
        REFERENCES dbo."FgsSetupTechTrade" ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupDescription"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupDescription_Tenant_Company_Type"
    ON dbo."FgsSetupDescription"
    (
        "TenantId",
        "CompanyId",
        "DescriptionTypeCode"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupDescription_TechTrade"
    ON dbo."FgsSetupDescription"
    (
        "FgsSetupTechTradeId"
    );


------------------ FgsSetupGLBreak

CREATE TABLE IF NOT EXISTS dbo."FgsSetupGLBreak"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Internal GL Break Code
    -- Example:
    -- HVAC
    -- PLUMBING
    -- NORTH_REGION
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Internal Name
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Customer facing label
    -- Example:
    -- Heating & Cooling
    -- Plumbing Services
    "BreakLabel" text COLLATE pg_catalog."default",

    -- Break hierarchy level
    -- Example:
    -- 1 = Division
    -- 2 = Department
    -- 3 = Region
    "BreakLevel" integer NOT NULL,

    -- Optional trade reference
    "FgsSetupTechTradeId" uuid,

    -- Optional logo URL
    "LogoUrl" text COLLATE pg_catalog."default",

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupGLBreak"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupGLBreak_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupGLBreak_TechTrade"
        FOREIGN KEY ("FgsSetupTechTradeId")
        REFERENCES dbo."FgsSetupTechTrade" ("Id"),

    CONSTRAINT "UQ_FgsSetupGLBreak"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupGLBreak"
    OWNER to dbmasteruser;

------------------ 

CREATE TABLE IF NOT EXISTS dbo."GloPaymentMethodType"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Payment method code
    -- Example:
    -- CASH
    -- CHECK
    -- CREDIT_CARD
    -- ACH
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Display label
    -- Example:
    -- Cash
    -- Credit Card
    -- Bank Transfer
    "DisplayName" text COLLATE pg_catalog."default" NOT NULL,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Sort order
    "SortOrder" integer NOT NULL DEFAULT 0,

    CONSTRAINT "PK_GloPaymentMethodType"
        PRIMARY KEY ("Id"),

    CONSTRAINT "UQ_GloPaymentMethodType_Code"
        UNIQUE ("Code")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."GloPaymentMethodType"
    OWNER to dbmasteruser;


INSERT INTO dbo."GloPaymentMethodType"
(
    "Code",
    "DisplayName",
    "SortOrder",
    "IsActive"
)
VALUES
('CASH',         'Cash',          1, true),
('CHECK',        'Check',         2, true),
('CREDIT_CARD',  'Credit Card',   3, true),
('DEBIT_CARD',   'Debit Card',    4, true),
('ACH',          'ACH Transfer',  5, true),
('APPLE_PAY',    'Apple Pay',     6, true),
('GOOGLE_PAY',   'Google Pay',    7, true),
('ZELLE',        'Zelle',         8, true);

------------------ 

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPaymentMethod"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Global payment method type
    "GloPaymentMethodTypeId" integer NOT NULL,

    -- Tenant display override
    -- Example:
    -- Visa / Mastercard
    -- Company Check
    "DisplayName" text COLLATE pg_catalog."default" NOT NULL,

    -- Visible in mobile app
    "IsMobileVisible" boolean NOT NULL DEFAULT true,

    -- Visible in customer portal
    "IsCustomerPortalVisible" boolean NOT NULL DEFAULT true,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPaymentMethod"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPaymentMethod_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupPaymentMethod_PaymentMethodType"
        FOREIGN KEY ("GloPaymentMethodTypeId")
        REFERENCES dbo."GloPaymentMethodType" ("Id"),

    CONSTRAINT "UQ_FgsSetupPaymentMethod"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "GloPaymentMethodTypeId"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPaymentMethod"
    OWNER to dbmasteruser;

------------------ FgsSetupPaymentTerm

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPaymentTerm"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Payment term name
    -- Example:
    -- Net 30
    -- Due On Receipt
    -- Net 15
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Due date calculation method
    -- Example:
    -- NET_DAYS
    -- DUE_ON_RECEIPT
    -- END_OF_MONTH
    "DueDateMethod" text COLLATE pg_catalog."default" NOT NULL,

    -- Number of days for NET_DAYS
    -- Example:
    -- 30
    -- 15
    "NumberOfDays" integer,

    -- Available for Accounts Receivable
    "IsAccountsReceivable" boolean NOT NULL DEFAULT true,

    -- Available for Accounts Payable
    "IsAccountsPayable" boolean NOT NULL DEFAULT true,

    -- Visible in mobile app
    "IsMobileVisible" boolean NOT NULL DEFAULT true,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPaymentTerm"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPaymentTerm_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupPaymentTerm"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Name"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPaymentTerm"
    OWNER to dbmasteruser;


------------------ FgsSetupPostalCode

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPostalCode"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Postal/Zip code
    -- Supports:
    -- US ZIP
    -- Canadian postal codes
    -- International postal formats
    --
    -- Examples:
    -- 77002
    -- 77429
    -- M5V 3L9
    "PostalCode" character varying(20) COLLATE pg_catalog."default" NOT NULL,

    -- Optional service zone
    "FgsSetupZoneId" uuid,

    -- Optional tax configuration
    "FgsSetupTaxId" uuid,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPostalCode"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPostalCode_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupPostalCode_Zone"
        FOREIGN KEY ("FgsSetupZoneId")
        REFERENCES dbo."FgsSetupZone" ("Id"),

    CONSTRAINT "FK_FgsSetupPostalCode_Tax"
        FOREIGN KEY ("FgsSetupTaxId")
        REFERENCES dbo."FgsSetupTax" ("Id"),

    CONSTRAINT "UQ_FgsSetupPostalCode"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "PostalCode"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPostalCode"
    OWNER to dbmasteruser;

------------------ FgsSetupPricingMatrix

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPricingMatrix"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Pricing matrix code
    -- Example:
    -- STANDARD
    -- AFTER_HOURS
    -- COMMERCIAL
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Display name
    -- Example:
    -- Standard Pricing
    -- After Hours Pricing
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Indicates labor pricing uses tier structure
    -- Example:
    -- true  = use tier pricing table
    -- false = use standard labor pricing
    "IsLaborTierStructure" boolean NOT NULL DEFAULT false,

    -- Indicates labor rates vary by skill level
    -- Example:
    -- true  = apprentice/journeyman/master rates
    -- false = single labor rate
    "IsLaborRateBySkillLevel" boolean NOT NULL DEFAULT false,

    -- Effective date range
    "EffectiveFrom" date NOT NULL,
    "EffectiveTo" date,

    -- Visible in mobile app
    "IsMobileVisible" boolean NOT NULL DEFAULT true,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPricingMatrix"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrix_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupPricingMatrix"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPricingMatrix"
    OWNER to dbmasteruser;

------------------ FgsSetupLaborRateType [new table]

CREATE TABLE IF NOT EXISTS dbo."FgsSetupLaborRateType"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Labor rate type code
    -- Example:
    -- STANDARD
    -- OVERTIME
    -- HOLIDAY
    -- WEEKEND
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Display name
    -- Example:
    -- Standard
    -- Overtime
    -- Holiday
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Visible in mobile app
    "IsMobileVisible" boolean NOT NULL DEFAULT true,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Sort order
    "SortOrder" integer NOT NULL DEFAULT 0,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupLaborRateType"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupLaborRateType_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupLaborRateType"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupLaborRateType"
    OWNER to dbmasteruser;

------------------ FgsSetupPricingMatrixLabor

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPricingMatrixLabor"
(
    "Id" uuid NOT NULL,

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Parent pricing matrix
    "FgsSetupPricingMatrixId" uuid NOT NULL,

    -- Tenant labor rate type
    "FgsSetupLaborRateTypeId" integer NOT NULL,

    -- Optional technician skill level
    -- Required only when:
    -- IsLaborRateBySkillLevel = true
    "FgsSetupTechSkillLevelId" uuid,

    -- Base hourly labor rate
    "BaseRate" numeric(18,2) NOT NULL,

    -- Overtime multiplier
    -- Example:
    -- 1.5
    "OvertimeMultiplier" numeric(18,2),

    -- Double time multiplier
    -- Example:
    -- 2.0
    "DoubleTimeMultiplier" numeric(18,2),

    -- Optional discount percentage
    -- Example:
    -- 10 = 10%
    "DiscountPercent" numeric(18,2),

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPricingMatrixLabor"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_PricingMatrix"
        FOREIGN KEY ("FgsSetupPricingMatrixId")
        REFERENCES dbo."FgsSetupPricingMatrix" ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_LaborRateType"
        FOREIGN KEY ("FgsSetupLaborRateTypeId")
        REFERENCES dbo."FgsSetupLaborRateType" ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLabor_TechSkillLevel"
        FOREIGN KEY ("FgsSetupTechSkillLevelId")
        REFERENCES dbo."FgsSetupTechSkillLevel" ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPricingMatrixLabor"
    OWNER to dbmasteruser;


------------------ FgsSetupPricingMatrixLaborTier

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPricingMatrixLaborTier"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Parent labor pricing row
    "FgsSetupPricingMatrixLaborId" uuid NOT NULL,

    -- Tier sequence
    -- Example:
    -- 1
    -- 2
    -- 3
    "SequenceOrder" integer NOT NULL,

    -- Duration in minutes for this tier
    -- Example:
    -- 60
    -- 120
    -- 240
    "DurationMinutes" integer NOT NULL,

    -- Labor rate for this tier
    -- Example:
    -- 125.00
    -- 175.00
    "Rate" numeric(18,2) NOT NULL,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPricingMatrixLaborTier"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLaborTier_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixLaborTier_Labor"
        FOREIGN KEY ("FgsSetupPricingMatrixLaborId")
        REFERENCES dbo."FgsSetupPricingMatrixLabor" ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPricingMatrixLaborTier"
    OWNER to dbmasteruser;

------------------ FgsSetupPricingMatrixMaterialTier

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPricingMatrixMaterialTier"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Parent pricing matrix
    "FgsSetupPricingMatrixId" uuid NOT NULL,

    -- Material cost range start
    "FromCost" numeric(18,2) NOT NULL,

    -- Material cost range end
    -- NULL = no upper limit
    "ToCost" numeric(18,2),

    -- Markup percentage
    -- Example:
    -- 75 = 75%
    "MarkupPercent" numeric(18,2) NOT NULL,

    -- Optional discount percentage
    "DiscountPercent" numeric(18,2),

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPricingMatrixMaterialTier"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixMaterialTier_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixMaterialTier_PricingMatrix"
        FOREIGN KEY ("FgsSetupPricingMatrixId")
        REFERENCES dbo."FgsSetupPricingMatrix" ("Id"),

    -- Prevent invalid negative values
    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_FromCost"
        CHECK ("FromCost" >= 0),

    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_ToCost"
        CHECK ("ToCost" IS NULL OR "ToCost" >= "FromCost"),

    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_MarkupPercent"
        CHECK ("MarkupPercent" >= 0),

    CONSTRAINT "CK_FgsSetupPricingMatrixMaterialTier_DiscountPercent"
        CHECK
        (
            "DiscountPercent" IS NULL
            OR ("DiscountPercent" >= 0 AND "DiscountPercent" <= 100)
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPricingMatrixMaterialTier"
    OWNER to dbmasteruser;


------------------ 

Drop table FgsSetupPriceSheetMaterialRange

------------------ FgsSetupPricingMatrixOther

CREATE TABLE IF NOT EXISTS dbo."FgsSetupPricingMatrixOther"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Parent pricing matrix
    "FgsSetupPricingMatrixId" uuid NOT NULL,

    -- Other pricing category code
    -- Example:
    -- TRIP_CHARGE
    -- DISPOSAL_FEE
    -- ENVIRONMENTAL_FEE
    "CategoryCode" text COLLATE pg_catalog."default" NOT NULL,

    -- Display name
    -- Example:
    -- Trip Charge
    -- Disposal Fee
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Markup percentage
    -- Example:
    -- 25 = 25%
    "MarkupPercent" numeric(18,2),

    -- Discount percentage
    -- Example:
    -- 10 = 10%
    "DiscountPercent" numeric(18,2),

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupPricingMatrixOther"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixOther_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupPricingMatrixOther_PricingMatrix"
        FOREIGN KEY ("FgsSetupPricingMatrixId")
        REFERENCES dbo."FgsSetupPricingMatrix" ("Id"),

    CONSTRAINT "UQ_FgsSetupPricingMatrixOther"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "FgsSetupPricingMatrixId",
            "CategoryCode"
        ),

    -- Data sanity checks
    CONSTRAINT "CK_FgsSetupPricingMatrixOther_MarkupPercent"
        CHECK
        (
            "MarkupPercent" IS NULL
            OR "MarkupPercent" >= 0
        ),

    CONSTRAINT "CK_FgsSetupPricingMatrixOther_DiscountPercent"
        CHECK
        (
            "DiscountPercent" IS NULL
            OR ("DiscountPercent" >= 0 AND "DiscountPercent" <= 100)
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupPricingMatrixOther"
    OWNER to dbmasteruser;


------------------ FgsSetupServiceAssetManufacturer

CREATE TABLE IF NOT EXISTS dbo."FgsSetupServiceAssetManufacturer"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Manufacturer code
    -- Must always be uppercase
    -- Example:
    -- TRANE
    -- LENNOX
    -- CARRIER
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Manufacturer name
    -- Example:
    -- Trane
    -- Lennox
    -- Carrier
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupServiceAssetManufacturer"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupServiceAssetManufacturer_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupServiceAssetManufacturer"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        ),

    -- Force uppercase codes
    CONSTRAINT "CK_FgsSetupServiceAssetManufacturer_Code_Upper"
        CHECK ("Code" = UPPER("Code"))
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupServiceAssetManufacturer"
    OWNER to dbmasteruser;


------------------ 

drop table FgsSetupServiceAssetModelSerialDescription, FgsSetupServiceAssetMedia

------------------ FgsSetupServiceAssetType

CREATE TABLE IF NOT EXISTS dbo."FgsSetupServiceAssetType"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Asset type code
    -- Must always be uppercase
    -- Example:
    -- HVAC
    -- WATER_HEATER
    -- GENERATOR
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Asset type name
    -- Example:
    -- HVAC System
    -- Water Heater
    -- Generator
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupServiceAssetType"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupServiceAssetType_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupServiceAssetType"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        ),

    -- Force uppercase codes
    CONSTRAINT "CK_FgsSetupServiceAssetType_Code_Upper"
        CHECK ("Code" = UPPER("Code"))
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupServiceAssetType"
    OWNER to dbmasteruser;

------------------ FgsSetupServiceAssetModelReference [New table]

CREATE TABLE IF NOT EXISTS dbo."FgsSetupServiceAssetModelReference"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Service asset type
    "FgsSetupServiceAssetTypeId" integer NOT NULL,

    -- Manufacturer reference
    "FgsSetupServiceAssetManufacturerId" integer NOT NULL,

    -- Model number
    -- Example:
    -- XR16
    -- EL296V
    -- GSX140361
    "ModelNumber" text COLLATE pg_catalog."default",

    -- Model description
    -- Example:
    -- XR16 Split System
    -- Elite Series Furnace
    "ModelDescription" text COLLATE pg_catalog."default" NOT NULL,

    -- Optional serial number pattern
    -- Example:
    -- ^[A-Z]{3}[0-9]{6}$
    "SerialNumberPattern" text COLLATE pg_catalog."default",

    -- Optional notes
    "Notes" text COLLATE pg_catalog."default",

    -- URLs/media stored as JSON document
    -- Example:
    -- [
    --   {
    --     "Name": "Installation Manual",
    --     "Url": "https://example.com/manual.pdf",
    --     "MediaType": "PDF"
    --   },
    --   {
    --     "Name": "Wiring Diagram",
    --     "Url": "https://example.com/wiring.jpg",
    --     "MediaType": "IMAGE"
    --   },
    --   {
    --     "Name": "Repair Video",
    --     "Url": "https://youtube.com/example",
    --     "MediaType": "VIDEO"
    --   }
    -- ]
    "UrlsJson" jsonb,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupServiceAssetModelReference"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupServiceAssetModelReference_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupServiceAssetModelReference_AssetType"
        FOREIGN KEY ("FgsSetupServiceAssetTypeId")
        REFERENCES dbo."FgsSetupServiceAssetType" ("Id"),

    CONSTRAINT "FK_FgsSetupServiceAssetModelReference_Manufacturer"
        FOREIGN KEY ("FgsSetupServiceAssetManufacturerId")
        REFERENCES dbo."FgsSetupServiceAssetManufacturer" ("Id"),

    CONSTRAINT "CK_FgsSetupServiceAssetModelReference_UrlsJson"
        CHECK
        (
            "UrlsJson" IS NULL
            OR jsonb_typeof("UrlsJson") = 'array'
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupServiceAssetModelReference"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupServiceAssetModelReference_Manufacturer"
    ON dbo."FgsSetupServiceAssetModelReference"
    (
        "TenantId",
        "CompanyId",
        "FgsSetupServiceAssetManufacturerId"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupServiceAssetModelReference_AssetType"
    ON dbo."FgsSetupServiceAssetModelReference"
    (
        "TenantId",
        "CompanyId",
        "FgsSetupServiceAssetTypeId"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupServiceAssetModelReference_AssetType_Manufacturer"
    ON dbo."FgsSetupServiceAssetModelReference"
    (
        "TenantId",
        "CompanyId",
        "FgsSetupServiceAssetTypeId",
        "FgsSetupServiceAssetManufacturerId"
    );

CREATE INDEX IF NOT EXISTS "IX_ServiceAsset_UrlsJson"
    ON dbo."FgsSetupServiceAssetModelReference"
    USING GIN ("UrlsJson");


------------------ FgsSetupTax

CREATE TABLE IF NOT EXISTS dbo."FgsSetupTax"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Tax code
    -- Must always be uppercase
    -- Example:
    -- SALES_TAX
    -- GST
    -- VAT
    "TaxCode" text COLLATE pg_catalog."default" NOT NULL,

    -- Tax name
    -- Example:
    -- Texas Sales Tax
    -- Canada GST
    -- UK VAT
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Indicates if record came from external provider/system
    -- Example:
    -- true  = imported/synced
    -- false = manually entered by tenant
    "IsExternalSystemRecord" boolean NOT NULL DEFAULT false,

    -- Optional description
    "Description" text COLLATE pg_catalog."default",

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupTax"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupTax_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupTax"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "TaxCode"
        ),

    -- Force uppercase tax codes
    CONSTRAINT "CK_FgsSetupTax_TaxCode_Upper"
        CHECK ("TaxCode" = UPPER("TaxCode"))
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupTax"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTax_TaxCode"
    ON dbo."FgsSetupTax"
    (
        "TenantId",
        "CompanyId",
        "TaxCode"
    );

------------------ FgsSetupTaxAuthority

CREATE TABLE IF NOT EXISTS dbo."FgsSetupTaxAuthority"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Tax authority code
    -- Must always be uppercase
    -- Example:
    -- TX_STATE
    -- CA_GST
    -- QC_QST
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Tax authority name
    -- Example:
    -- Texas Comptroller
    -- Canada GST
    -- Quebec Sales Tax
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Region/state/province code
    -- Example:
    -- TX
    -- AB
    -- QC
    "RegionCode" text COLLATE pg_catalog."default",

    -- Indicates if record came from external provider/system
    -- Example:
    -- true  = imported/synced
    -- false = manually entered by tenant
    "IsExternalSystemRecord" boolean NOT NULL DEFAULT false,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupTaxAuthority"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupTaxAuthority_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupTaxAuthority"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        ),

    -- Force uppercase authority codes
    CONSTRAINT "CK_FgsSetupTaxAuthority_Code_Upper"
        CHECK ("Code" = UPPER("Code")),

    -- Force uppercase region codes
    CONSTRAINT "CK_FgsSetupTaxAuthority_RegionCode_Upper"
        CHECK
        (
            "RegionCode" IS NULL
            OR "RegionCode" = UPPER("RegionCode")
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupTaxAuthority"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTaxAuthority_Code"
    ON dbo."FgsSetupTaxAuthority"
    (
        "TenantId",
        "CompanyId",
        "Code"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTaxAuthority_RegionCode"
    ON dbo."FgsSetupTaxAuthority"
    (
        "TenantId",
        "CompanyId",
        "RegionCode"
    );


------------------ FgsSetupTaxDetail

CREATE TABLE IF NOT EXISTS dbo."FgsSetupTaxDetail"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Tax reference
    "FgsSetupTaxId" integer NOT NULL,

    -- Tax authority/jurisdiction reference
    "FgsSetupTaxAuthorityId" integer NOT NULL,

    -- Effective date range
    "EffectiveFromDate" date NOT NULL,
    "EffectiveToDate" date,

    -- Tax percentage
    -- Example:
    -- 8.25 = 8.25%
    "TaxPercent" numeric(18,4) NOT NULL,

    -- Indicates if record came from external provider/system
    -- Example:
    -- true  = imported/synced
    -- false = manually entered by tenant
    "IsExternalSystemRecord" boolean NOT NULL DEFAULT false,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupTaxDetail"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupTaxDetail_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupTaxDetail_Tax"
        FOREIGN KEY ("FgsSetupTaxId")
        REFERENCES dbo."FgsSetupTax" ("Id"),

    CONSTRAINT "FK_FgsSetupTaxDetail_TaxAuthority"
        FOREIGN KEY ("FgsSetupTaxAuthorityId")
        REFERENCES dbo."FgsSetupTaxAuthority" ("Id"),

    -- Prevent invalid tax percentages
    CONSTRAINT "CK_FgsSetupTaxDetail_TaxPercent"
        CHECK ("TaxPercent" >= 0 AND "TaxPercent" <= 100),

    -- Prevent invalid effective date ranges
    CONSTRAINT "CK_FgsSetupTaxDetail_EffectiveDates"
        CHECK
        (
            "EffectiveToDate" IS NULL
            OR "EffectiveToDate" >= "EffectiveFromDate"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupTaxDetail"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTaxDetail_Tax"
    ON dbo."FgsSetupTaxDetail"
    (
        "TenantId",
        "CompanyId",
        "FgsSetupTaxId",
        "EffectiveFromDate",
        "EffectiveToDate"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTaxDetail_TaxAuthority"
    ON dbo."FgsSetupTaxDetail"
    (
        "TenantId",
        "CompanyId",
        "FgsSetupTaxAuthorityId"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTaxDetail_EffectiveDates"
    ON dbo."FgsSetupTaxDetail"
    (
        "EffectiveFromDate",
        "EffectiveToDate"
    );

------------------ FgsSetupTechSkillLevel

CREATE TABLE IF NOT EXISTS dbo."FgsSetupTechSkillLevel"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Skill level code
    -- Must always be uppercase
    -- Example:
    -- APPRENTICE
    -- JOURNEYMAN
    -- MASTER
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Skill level name
    -- Example:
    -- Apprentice
    -- Journeyman
    -- Master Technician
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Display sort order
    "SortOrder" integer NOT NULL DEFAULT 0,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupTechSkillLevel"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupTechSkillLevel_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupTechSkillLevel"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        ),

    -- Force uppercase skill level codes
    CONSTRAINT "CK_FgsSetupTechSkillLevel_Code_Upper"
        CHECK ("Code" = UPPER("Code")),

    -- Prevent invalid sort orders
    CONSTRAINT "CK_FgsSetupTechSkillLevel_SortOrder"
        CHECK ("SortOrder" >= 0)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupTechSkillLevel"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTechSkillLevel_Code"
    ON dbo."FgsSetupTechSkillLevel"
    (
        "TenantId",
        "CompanyId",
        "Code"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTechSkillLevel_SortOrder"
    ON dbo."FgsSetupTechSkillLevel"
    (
        "TenantId",
        "CompanyId",
        "SortOrder"
    );


------------------ FgsSetupTechTrade

CREATE TABLE IF NOT EXISTS dbo."FgsSetupTechTrade"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Trade code
    -- Must always be uppercase
    -- Example:
    -- HVAC
    -- PLUMBING
    -- ELECTRICAL
    "TradeCode" text COLLATE pg_catalog."default" NOT NULL,

    -- Trade name
    -- Example:
    -- HVAC
    -- Plumbing
    -- Electrical
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Display sort order
    "SortOrder" integer NOT NULL DEFAULT 0,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupTechTrade"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupTechTrade_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupTechTrade"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "TradeCode"
        ),

    -- Force uppercase trade codes
    CONSTRAINT "CK_FgsSetupTechTrade_TradeCode_Upper"
        CHECK ("TradeCode" = UPPER("TradeCode")),

    -- Prevent invalid sort orders
    CONSTRAINT "CK_FgsSetupTechTrade_SortOrder"
        CHECK ("SortOrder" >= 0)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupTechTrade"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTechTrade_TradeCode"
    ON dbo."FgsSetupTechTrade"
    (
        "TenantId",
        "CompanyId",
        "TradeCode"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTechTrade_SortOrder"
    ON dbo."FgsSetupTechTrade"
    (
        "TenantId",
        "CompanyId",
        "SortOrder"
    );


------------------ FgsSetupTimeSlot

CREATE TABLE IF NOT EXISTS dbo."FgsSetupTimeSlot"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Optional zone reference
    "FgsSetupZoneId" integer,

    -- Time slot code
    -- Must always be uppercase
    -- Example:
    -- MORNING
    -- AFTERNOON
    -- EVENING
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Time slot name
    -- Example:
    -- Morning Slot
    -- Afternoon Slot
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Slot start time
    -- Example:
    -- 08:00:00
    "BeginTime" interval NOT NULL,

    -- Slot end time
    -- Example:
    -- 12:00:00
    "EndTime" interval NOT NULL,

    -- Mark technician late after
    -- Example:
    -- 00:15:00 = 15 minutes
    "MarkTechArrivedLateAfter" interval,

    -- Mark work order delayed after
    -- Example:
    -- 00:30:00 = 30 minutes
    "MarkWorkOrderDelayedCompletionAfter" interval,

    -- Mobile visibility
    "IsMobileVisible" boolean NOT NULL DEFAULT true,

    -- Customer portal visibility
    "IsCustomerPortalVisible" boolean NOT NULL DEFAULT true,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupTimeSlot"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupTimeSlot_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsSetupTimeSlot_Zone"
        FOREIGN KEY ("FgsSetupZoneId")
        REFERENCES dbo."FgsSetupZone" ("Id"),

    CONSTRAINT "UQ_FgsSetupTimeSlot"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        ),

    -- Force uppercase codes
    CONSTRAINT "CK_FgsSetupTimeSlot_Code_Upper"
        CHECK ("Code" = UPPER("Code")),

    -- End time must be after begin time
    CONSTRAINT "CK_FgsSetupTimeSlot_TimeRange"
        CHECK ("EndTime" > "BeginTime")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupTimeSlot"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTimeSlot_Code"
    ON dbo."FgsSetupTimeSlot"
    (
        "TenantId",
        "CompanyId",
        "Code"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTimeSlot_Zone"
    ON dbo."FgsSetupTimeSlot"
    (
        "TenantId",
        "CompanyId",
        "FgsSetupZoneId"
    );

------------------ FgsSetupTitleOfCourtesy

CREATE TABLE IF NOT EXISTS dbo."FgsSetupTitleOfCourtesy"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Courtesy title code
    -- Must always be uppercase
    -- Example:
    -- MR
    -- MRS
    -- DR
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Display name
    -- Example:
    -- Mr.
    -- Mrs.
    -- Dr.
    "DisplayName" text COLLATE pg_catalog."default" NOT NULL,

    -- Display order
    "SortOrder" integer NOT NULL DEFAULT 0,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupTitleOfCourtesy"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupTitleOfCourtesy_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupTitleOfCourtesy"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        ),

    -- Force uppercase codes
    CONSTRAINT "CK_FgsSetupTitleOfCourtesy_Code_Upper"
        CHECK ("Code" = UPPER("Code")),

    -- Prevent invalid sort orders
    CONSTRAINT "CK_FgsSetupTitleOfCourtesy_SortOrder"
        CHECK ("SortOrder" >= 0)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupTitleOfCourtesy"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTitleOfCourtesy_Code"
    ON dbo."FgsSetupTitleOfCourtesy"
    (
        "TenantId",
        "CompanyId",
        "Code"
    );

CREATE INDEX IF NOT EXISTS "IX_FgsSetupTitleOfCourtesy_SortOrder"
    ON dbo."FgsSetupTitleOfCourtesy"
    (
        "TenantId",
        "CompanyId",
        "SortOrder"
    );

------------------ FgsSetupZone

CREATE TABLE IF NOT EXISTS dbo."FgsSetupZone"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Zone code
    -- Must always be uppercase
    -- Example:
    -- NORTH
    -- SOUTHWEST
    -- HOUSTON_METRO
    "Code" text COLLATE pg_catalog."default" NOT NULL,

    -- Zone name
    -- Example:
    -- North Zone
    -- Southwest Zone
    -- Houston Metro
    "Name" text COLLATE pg_catalog."default" NOT NULL,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsSetupZone"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSetupZone_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "UQ_FgsSetupZone"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "Code"
        ),

    -- Force uppercase zone codes
    CONSTRAINT "CK_FgsSetupZone_Code_Upper"
        CHECK ("Code" = UPPER("Code"))
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsSetupZone"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsSetupZone_Code"
    ON dbo."FgsSetupZone"
    (
        "TenantId",
        "CompanyId",
        "Code"
    );


------------------ 

drop table FgsTenantCompanyConfiguration


------------------ GloTimeCardOption

CREATE TABLE IF NOT EXISTS dbo."GloTimeCardOption"
(
    "Id" integer NOT NULL,
    "Code" text NOT NULL,
    "Name" text NOT NULL,

    CONSTRAINT "PK_GloTimeCardOption"
        PRIMARY KEY ("Id"),

    CONSTRAINT "UQ_GloTimeCardOption_Code"
        UNIQUE ("Code"),

    CONSTRAINT "CK_GloTimeCardOption_Code_Upper"
        CHECK ("Code" = UPPER("Code"))
);
------------------ FgsTenantServiceSetup [New Setup]

CREATE TABLE IF NOT EXISTS dbo."FgsTenantServiceSetup"
(
    -- Ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Time card setup
    "GloTimeCardOptionId" integer NOT NULL,

    -- Accounting integration
    "AccountingIntegrationTypeId" integer,

    -- Tax integration
    -- Example:
    -- true = using Avalara/external provider
    -- false = internal tax engine
    "UseExternalTaxCalculationProvider" boolean NOT NULL DEFAULT false,

    -- Widgets/features
    "EnableCallBookingWidget" boolean NOT NULL DEFAULT false,
    "EnablePaymentWidget" boolean NOT NULL DEFAULT false,
    "EnableCustomerPortal" boolean NOT NULL DEFAULT false,
    "EnableRulesManagement" boolean NOT NULL DEFAULT false,

    -- Auto arrive
    "EnableAutoArrive" boolean NOT NULL DEFAULT false,

    -- Radius in meters
    "WorkLocationRadiusForAutoArrive" integer,

    -- Overtime window
    "OTStartTime" interval,
    "OTEndTime" interval,

    -- Double-time window
    "DTStartTime" interval,
    "DTEndTime" interval,

    -- Billing method
    -- Example:
    -- DISPATCH
    -- ARRIVE
    "BillHoursFromDispatchOrArrive"
        character varying(20) COLLATE pg_catalog."default" NOT NULL,

    -- Validation rules
    "SourceCodeRequiredOnWorkOrder" boolean NOT NULL DEFAULT false,
    "SourceCodeRequiredOnServiceLocation" boolean NOT NULL DEFAULT false,

    -- Numbering seeds
    "BillToStartNumber" bigint NOT NULL,
    "POStartNumber" bigint NOT NULL,
    "QuoteStartNumber" bigint NOT NULL,
    "WorkOrderStartNumber" bigint NOT NULL,

    -- Number prefixes
    "InvoiceNumberPrefix"
        character varying(20) COLLATE pg_catalog."default",

    "QuoteNumberPrefix"
        character varying(20) COLLATE pg_catalog."default",

    "PONumberPrefix"
        character varying(20) COLLATE pg_catalog."default",

    "WorkOrderNumberPrefix"
        character varying(20) COLLATE pg_catalog."default",

    -- Batch format
    "InvoiceBatchNumberFormat"
        character varying(200) COLLATE pg_catalog."default",

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL,
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsTenantServiceSetup"
        PRIMARY KEY ("TenantId", "CompanyId"),

    CONSTRAINT "FK_FgsTenantServiceSetup_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsTenantServiceSetup_TimeCardOption"
        FOREIGN KEY ("GloTimeCardOptionId")
        REFERENCES dbo."GloTimeCardOption" ("Id"),

    -- Prevent invalid radius
    CONSTRAINT "CK_FgsTenantServiceSetup_WorkLocationRadius"
        CHECK
        (
            "WorkLocationRadiusForAutoArrive" IS NULL
            OR "WorkLocationRadiusForAutoArrive" >= 0
        ),

    -- Validate overtime range
    CONSTRAINT "CK_FgsTenantServiceSetup_OTRange"
        CHECK
        (
            "OTStartTime" IS NULL
            OR "OTEndTime" IS NULL
            OR "OTEndTime" > "OTStartTime"
        ),

    -- Validate double-time range
    CONSTRAINT "CK_FgsTenantServiceSetup_DTRange"
        CHECK
        (
            "DTStartTime" IS NULL
            OR "DTEndTime" IS NULL
            OR "DTEndTime" > "DTStartTime"
        )
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsTenantServiceSetup"
    OWNER to dbmasteruser;

------------------ GloResolutionType [New Table]

CREATE TABLE "GloResolutionType"
(
    "Id" BIGSERIAL PRIMARY KEY,

    "ResolutionTypeCode" VARCHAR(50) NOT NULL,
    "ResolutionTypeName" VARCHAR(200) NOT NULL,

    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,

    "CreatedOn" TIMESTAMP NOT NULL DEFAULT NOW(),
    "CreatedBy" BIGINT NULL,

    "UpdatedOn" TIMESTAMP NULL,
    "UpdatedBy" BIGINT NULL,

    CONSTRAINT "UQ_GloResolutionType_Code"
    UNIQUE ("ResolutionTypeCode")
);

INSERT INTO dbo."GloResolutionType"
(
    "Id",
    "ResolutionTypeCode",
    "ResolutionTypeName"
)
VALUES

(1, 'COMPLETED',      'Completed Successfully'),
(2, 'INCOMPLETE',     'Incomplete Work'),
(3, 'PART_REQUIRED',  'Parts Required'),
(4, 'PARTS_ARRIVED',  'Parts Arrived'),
(5, 'CANCELLED',      'Cancelled');

-------------------- FsgResolutionCode [New table]

CREATE TABLE IF NOT EXISTS dbo."FgsResolutionCode"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY
    (
        INCREMENT 1
        START 1
        MINVALUE 1
        MAXVALUE 2147483647
        CACHE 1
    ),

    -- Multi-tenant ownership
    "TenantId" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,

    -- Resolution type
    "GloResolutionTypeId" integer NOT NULL,

    -- Resolution code
    -- Must always be uppercase
    -- Example:
    -- WAITING_FOR_PARTS
    -- READY_FOR_INSTALL
    -- CUSTOMER_NOT_HOME
    "ResolutionCode" character varying(50)
        COLLATE pg_catalog."default" NOT NULL,

    -- Resolution display name
    -- Example:
    -- Waiting For Parts
    -- Ready For Install
    -- Customer Not Home
    "ResolutionName" character varying(200)
        COLLATE pg_catalog."default" NOT NULL,

    -- Controls whether resolution is visible in mobile app
    "IsMobileVisible" boolean NOT NULL DEFAULT true,

    -- Status
    "IsActive" boolean NOT NULL DEFAULT true,

    -- Audit
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT NOW(),
    "CreatedBy" uuid,

    "UpdatedOn" timestamp with time zone,
    "UpdatedBy" uuid,

    CONSTRAINT "PK_FgsResolutionCode"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsResolutionCode_Company"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "Id"),

    CONSTRAINT "FK_FgsResolutionCode_ResolutionType"
        FOREIGN KEY ("GloResolutionTypeId")
        REFERENCES dbo."GloResolutionType" ("Id"),

    CONSTRAINT "UQ_FgsResolutionCode_Code"
        UNIQUE
        (
            "TenantId",
            "CompanyId",
            "ResolutionCode"
        ),

    -- Force uppercase resolution codes
    CONSTRAINT "CK_FgsResolutionCode_Code_Upper"
        CHECK ("ResolutionCode" = UPPER("ResolutionCode"))
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbo."FgsResolutionCode"
    OWNER to dbmasteruser;

-- =========================================================
-- INDEXES
-- =========================================================

CREATE INDEX IF NOT EXISTS "IX_FgsResolutionCode_Code"
    ON dbo."FgsResolutionCode"
    (
        "TenantId",
        "CompanyId",
        "ResolutionCode"
    );

------------------ 
------------------ 
------------------ 
------------------ 


```


# FGS Tenant Table Structure.txt

```sql
Yes — that is a better enterprise design.

Instead of:

```text id="q2v8mf"
PrimaryLocationId
```

you should explicitly separate:
- physical/business address
- billing/mailing/remit address

because they are often different.

---

# Recommended Changes For `FgsTenant`

Replace:

```text id="z7p4xn"
PrimaryLocationId
```

with:

```text id="t6m9rb"
PhysicalLocationId
BillingLocationId
```

---

# Updated `FgsTenant` Columns

```text id="w3k8qs"
Id

TenantCode

Name
LegalName

Email
PhoneNumber
Website

PhysicalLocationId
BillingLocationId

SubscriptionPlanId

TimeZone
DefaultCurrency
DefaultLanguageId

IsActive

CreatedOn
CreatedBy

UpdatedOn
UpdatedBy
```

---

# Why This Is Better

## PhysicalLocationId

Represents:
- headquarters
- office address
- operational location

Used for:
- maps
- dispatching
- regional settings
- company profile

---

## BillingLocationId

Represents:
- remit-to address
- mailing address
- invoice address

Used for:
- invoicing
- tax documents
- statements
- payments

---

# Real Enterprise Example

| Type | Address |
|---|---|
| Physical | Houston Office |
| Billing | Dallas Lockbox |

Very common in:
- franchises
- corporations
- accounting offices
- national accounts

---

# Recommendation For `FgsTenantCompany`

You should ALSO do the same there.

Replace:

```text id="k9n5xa"
PrimaryLocationId
```

with:

```text id="v1q7mp"
PhysicalLocationId
BillingLocationId
```

This gives much better long-term flexibility.
```

