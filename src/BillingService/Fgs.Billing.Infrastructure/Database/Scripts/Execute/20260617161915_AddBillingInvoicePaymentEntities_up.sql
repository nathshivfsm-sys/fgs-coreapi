DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'billing') THEN
        CREATE SCHEMA billing;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS billing."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260603212540_InitialSchema') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603212540_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'billing') THEN
            CREATE SCHEMA billing;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE TABLE billing."FgsTenantCompanyCache" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "CompanyGuid" uuid NOT NULL,
        "CompanyCode" character varying(100) NOT NULL,
        "CompanyName" character varying(200) NOT NULL,
        "IsActive" boolean NOT NULL,
        "UpdatedOn" timestamptz,
        CONSTRAINT "PK_FgsTenantCompanyCache" PRIMARY KEY ("TenantId", "CompanyId")
    );
    COMMENT ON TABLE billing."FgsTenantCompanyCache" IS 'Local cache of tenant company information used by the billing schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyId" IS 'Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyGuid" IS 'Globally unique company identifier used by integrations and external systems.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyCode" IS 'Unique company code within a tenant.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."CompanyName" IS 'Display name of the company.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."IsActive" IS 'Indicates whether the company is active.';
    COMMENT ON COLUMN billing."FgsTenantCompanyCache"."UpdatedOn" IS 'Timestamp of the most recent synchronization from tenant.FgsTenantCompany.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_CompanyName" ON billing."FgsTenantCompanyCache" ("CompanyName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_IsActive" ON billing."FgsTenantCompanyCache" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    CREATE UNIQUE INDEX "UQ_FgsTenantCompanyCache_CompanyGuid" ON billing."FgsTenantCompanyCache" ("CompanyGuid");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260604131918_AddFgsTenantCompanyCache') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260604131918_AddFgsTenantCompanyCache', '10.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE TABLE billing."FgsInvoiceBatch" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "BatchNumber" character varying(50) NOT NULL,
        "BatchDate" date NOT NULL,
        "InvoiceCount" integer NOT NULL DEFAULT 0,
        "InvoiceSubtotal" numeric(18,2) NOT NULL DEFAULT 0.0,
        "TotalTax" numeric(18,2) NOT NULL DEFAULT 0.0,
        "InvoiceTotal" numeric(18,2) NOT NULL DEFAULT 0.0,
        "IsClosed" boolean NOT NULL DEFAULT FALSE,
        "ClosedOn" timestamp,
        "ClosedBy" bigint,
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamp,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsInvoiceBatch" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsInvoiceBatch_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE TABLE billing."FgsPayment" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "PaymentNumber" character varying(50) NOT NULL,
        "CustomerId" bigint NOT NULL,
        "ServiceLocationId" bigint,
        "PaymentTypeId" integer NOT NULL,
        "PaymentMethodId" integer NOT NULL,
        "PaymentStatusId" integer NOT NULL,
        "SourceType" character varying(50),
        "SourceId" bigint,
        "PaymentDate" date NOT NULL,
        "AccountingDate" date NOT NULL,
        "ReferenceNumber" character varying(100),
        "BankAccountId" bigint,
        "PaymentAmount" numeric(18,2) NOT NULL,
        "AppliedAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "DepositDate" date,
        "PaymentNote" text,
        "ExternalAccountingId" character varying(100),
        "ExternalAccountingSyncToken" character varying(100),
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamp,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsPayment" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsPayment_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE billing."FgsPayment" IS 'Stores customer payment transactions received for invoices, estimates, service agreements, deposits, refunds, and other billing-related activities.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE TABLE billing."FgsInvoice" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "InvoiceNumber" character varying(50) NOT NULL,
        "InvoiceTypeId" smallint NOT NULL,
        "CustomerId" bigint NOT NULL,
        "ServiceLocationId" bigint NOT NULL,
        "WorkOrderId" bigint,
        "ProjectId" bigint,
        "ServiceAgreementId" bigint,
        "MaintenanceVisitId" bigint,
        "ServiceJobNum" character varying(100),
        "IsAgreementBilling" boolean NOT NULL DEFAULT FALSE,
        "IsRecurringInvoice" boolean NOT NULL DEFAULT FALSE,
        "RecurringScheduleId" bigint,
        "WorkOrderNumber" character varying(50),
        "JobTypeId" integer,
        "LeadEmployeeId" bigint,
        "CustomerPONumber" character varying(100),
        "InvoiceDate" date NOT NULL,
        "AccountingDate" date NOT NULL,
        "DueDate" date,
        "NetTermId" integer,
        "PreferredPaymentMethodId" integer,
        "LaborPricingMatrixId" bigint,
        "MaterialPricingMatrixId" bigint,
        "OtherPricingMatrixId" bigint,
        "GLBreak1Id" integer,
        "GLBreak2Id" integer,
        "TaxingAuthorityJson" jsonb,
        "BillToAddressJson" jsonb,
        "ServiceLocationAddressJson" jsonb,
        "CompanyAddressJson" jsonb,
        "InvoiceTemplateId" bigint,
        "IsSigned" boolean NOT NULL DEFAULT FALSE,
        "SignedOn" timestamp,
        "InvoiceSubtotal" numeric(18,2) NOT NULL DEFAULT 0.0,
        "TotalDiscount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "TaxableAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "TotalTax" numeric(18,2) NOT NULL DEFAULT 0.0,
        "InvoiceTotal" numeric(18,2) NOT NULL DEFAULT 0.0,
        "AppliedAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "BalanceDue" numeric(18,2) NOT NULL DEFAULT 0.0,
        "IsApproved" boolean NOT NULL DEFAULT FALSE,
        "ApprovedBy" bigint,
        "ApprovedOn" timestamp,
        "IsPosted" boolean NOT NULL DEFAULT FALSE,
        "PostedBy" bigint,
        "PostedOn" timestamp,
        "InvoiceBatchId" bigint,
        "ExternalAccountingId" character varying(100),
        "ExternalAccountingSyncToken" character varying(100),
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamp,
        "UpdatedBy" bigint,
        "RowVersion" bigint NOT NULL DEFAULT 1,
        CONSTRAINT "PK_FgsInvoice" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsInvoice_InvoiceBatch" FOREIGN KEY ("InvoiceBatchId") REFERENCES billing."FgsInvoiceBatch" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsInvoice_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE TABLE billing."FgsPaymentTransaction" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "PaymentId" bigint NOT NULL,
        "TransactionTypeId" integer NOT NULL,
        "TransactionMethodId" integer NOT NULL,
        "PaymentProcessorId" integer NOT NULL,
        "TransactionId" character varying(150) NOT NULL,
        "OriginalTransactionId" character varying(150),
        "AuthorizationCode" character varying(100),
        "ProcessorStatus" character varying(50),
        "CardHolderName" character varying(200),
        "CardLast4" character varying(4),
        "BankAccountLast4" character varying(4),
        "TransactionAmount" numeric(18,2) NOT NULL,
        "TransactionDate" timestamp NOT NULL,
        "UserId" bigint,
        "UserName" character varying(200),
        "Source" character varying(50),
        "TransactionDataJson" jsonb,
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamp,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsPaymentTransaction" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsPaymentTransaction_Payment" FOREIGN KEY ("PaymentId") REFERENCES billing."FgsPayment" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsPaymentTransaction_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE billing."FgsPaymentTransaction" IS 'Stores payment processor transaction records associated with customer payments.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE TABLE billing."FgsInvoiceDetail" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "InvoiceId" bigint NOT NULL,
        "ParentLineId" bigint,
        "LineNumber" integer NOT NULL,
        "BillingCategoryId" integer NOT NULL,
        "ItemCode" character varying(100),
        "ItemDescription" text NOT NULL,
        "IsInventory" boolean NOT NULL DEFAULT FALSE,
        "MasterPartNum" character varying(100),
        "InventoryItemId" bigint,
        "PriceBookItemId" bigint,
        "LaborRateTypeId" integer,
        "TechnicianId" bigint,
        "Quantity" numeric(18,4) NOT NULL DEFAULT 1.0,
        "UnitCost" numeric(18,4) NOT NULL DEFAULT 0.0,
        "ExtendedCost" numeric(18,2) NOT NULL DEFAULT 0.0,
        "UnitPrice" numeric(18,4) NOT NULL DEFAULT 0.0,
        "ExtendedPrice" numeric(18,2) NOT NULL DEFAULT 0.0,
        "IsTaxable" boolean NOT NULL DEFAULT FALSE,
        "GLBreak1Id" integer,
        "GLBreak2Id" integer,
        "LineAddedFrom" character varying(50),
        "LineAddedFromId" bigint,
        "AddedSource" character varying(50),
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamp,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsInvoiceDetail" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsInvoiceDetail_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsInvoiceDetail_Invoice" FOREIGN KEY ("InvoiceId") REFERENCES billing."FgsInvoice" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsInvoiceDetail_ParentLine" FOREIGN KEY ("ParentLineId") REFERENCES billing."FgsInvoiceDetail" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE TABLE billing."FgsInvoicePaymentApplication" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "PaymentId" bigint NOT NULL,
        "InvoiceId" bigint NOT NULL,
        "DisplayOrder" smallint NOT NULL DEFAULT 1,
        "AppliedAmount" numeric(18,2) NOT NULL,
        "AppliedOn" timestamp NOT NULL DEFAULT (now()),
        "ApplicationNote" text,
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamp,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsInvoicePaymentApplication" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsInvoicePaymentApplication_Invoice" FOREIGN KEY ("InvoiceId") REFERENCES billing."FgsInvoice" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsInvoicePaymentApplication_Payment" FOREIGN KEY ("PaymentId") REFERENCES billing."FgsPayment" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsInvoicePaymentApplication_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE billing."FgsInvoicePaymentApplication" IS 'Stores payment allocation records between payments and invoices.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_AccountingDate" ON billing."FgsInvoice" ("TenantId", "CompanyId", "AccountingDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_CustomerId" ON billing."FgsInvoice" ("TenantId", "CompanyId", "CustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_InvoiceBatchId" ON billing."FgsInvoice" ("TenantId", "CompanyId", "InvoiceBatchId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_InvoiceBatchId1" ON billing."FgsInvoice" ("InvoiceBatchId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_InvoiceDate" ON billing."FgsInvoice" ("TenantId", "CompanyId", "InvoiceDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_ServiceAgreementId" ON billing."FgsInvoice" ("TenantId", "CompanyId", "ServiceAgreementId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_ServiceLocationId" ON billing."FgsInvoice" ("TenantId", "CompanyId", "ServiceLocationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoice_WorkOrderId" ON billing."FgsInvoice" ("TenantId", "CompanyId", "WorkOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsInvoice_TenantCompany_InvoiceNumber" ON billing."FgsInvoice" ("TenantId", "CompanyId", "InvoiceNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoiceBatch_BatchDate" ON billing."FgsInvoiceBatch" ("TenantId", "CompanyId", "BatchDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoiceBatch_TenantCompany" ON billing."FgsInvoiceBatch" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsInvoiceBatch_TenantCompany_BatchNumber" ON billing."FgsInvoiceBatch" ("TenantId", "CompanyId", "BatchNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoiceDetail_InvoiceId" ON billing."FgsInvoiceDetail" ("TenantId", "CompanyId", "InvoiceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoiceDetail_InvoiceId_LineNumber" ON billing."FgsInvoiceDetail" ("TenantId", "CompanyId", "InvoiceId", "LineNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoiceDetail_InvoiceId1" ON billing."FgsInvoiceDetail" ("InvoiceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoiceDetail_ParentLineId" ON billing."FgsInvoiceDetail" ("ParentLineId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoicePaymentApplication_AppliedOn" ON billing."FgsInvoicePaymentApplication" ("TenantId", "CompanyId", "AppliedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoicePaymentApplication_Invoice" ON billing."FgsInvoicePaymentApplication" ("TenantId", "CompanyId", "InvoiceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoicePaymentApplication_InvoiceId" ON billing."FgsInvoicePaymentApplication" ("InvoiceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoicePaymentApplication_Payment" ON billing."FgsInvoicePaymentApplication" ("TenantId", "CompanyId", "PaymentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsInvoicePaymentApplication_TenantCompany" ON billing."FgsInvoicePaymentApplication" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsInvoicePaymentApplication_PaymentInvoice" ON billing."FgsInvoicePaymentApplication" ("PaymentId", "InvoiceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_AccountingDate" ON billing."FgsPayment" ("TenantId", "CompanyId", "AccountingDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_BankAccount" ON billing."FgsPayment" ("TenantId", "CompanyId", "BankAccountId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_Customer" ON billing."FgsPayment" ("TenantId", "CompanyId", "CustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_PaymentDate" ON billing."FgsPayment" ("TenantId", "CompanyId", "PaymentDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_ServiceLocation" ON billing."FgsPayment" ("TenantId", "CompanyId", "ServiceLocationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_Source" ON billing."FgsPayment" ("TenantId", "CompanyId", "SourceType", "SourceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_Status" ON billing."FgsPayment" ("TenantId", "CompanyId", "PaymentStatusId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPayment_TenantCompany" ON billing."FgsPayment" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsPayment_TenantCompany_PaymentNumber" ON billing."FgsPayment" ("TenantId", "CompanyId", "PaymentNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPaymentTransaction_OriginalTransactionId" ON billing."FgsPaymentTransaction" ("TenantId", "CompanyId", "OriginalTransactionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPaymentTransaction_Payment" ON billing."FgsPaymentTransaction" ("TenantId", "CompanyId", "PaymentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPaymentTransaction_PaymentId" ON billing."FgsPaymentTransaction" ("PaymentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPaymentTransaction_Processor" ON billing."FgsPaymentTransaction" ("TenantId", "CompanyId", "PaymentProcessorId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPaymentTransaction_ProcessorStatus" ON billing."FgsPaymentTransaction" ("TenantId", "CompanyId", "ProcessorStatus");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPaymentTransaction_TenantCompany" ON billing."FgsPaymentTransaction" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE INDEX "IX_FgsPaymentTransaction_TransactionDate" ON billing."FgsPaymentTransaction" ("TenantId", "CompanyId", "TransactionDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsPaymentTransaction_TransactionId" ON billing."FgsPaymentTransaction" ("TenantId", "CompanyId", "TransactionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260617161915_AddBillingInvoicePaymentEntities') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260617161915_AddBillingInvoicePaymentEntities', '10.0.8');
    END IF;
END $EF$;
COMMIT;

