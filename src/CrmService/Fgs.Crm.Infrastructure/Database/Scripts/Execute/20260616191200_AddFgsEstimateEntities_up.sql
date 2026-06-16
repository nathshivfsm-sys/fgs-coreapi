START TRANSACTION;
CREATE TABLE crm."FgsEstimateClause" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "ClauseTypeId" bigint NOT NULL,
    "ClauseName" character varying(255) NOT NULL,
    "ClauseText" text NOT NULL,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateClause" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateClause_DisplayOrder" CHECK ("DisplayOrder" > 0),
    CONSTRAINT "FK_FgsEstimateClause_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateClause" IS 'Stores reusable estimate clauses that may be used across estimates and estimate templates.';
COMMENT ON COLUMN crm."FgsEstimateClause"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateClause"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateClause"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateClause"."ClauseTypeId" IS 'Clause type such as Inclusion, Exclusion, or Terms and Conditions.';
COMMENT ON COLUMN crm."FgsEstimateClause"."ClauseName" IS 'User-friendly clause name.';
COMMENT ON COLUMN crm."FgsEstimateClause"."ClauseText" IS 'Customer-facing clause text displayed on estimate documents.';
COMMENT ON COLUMN crm."FgsEstimateClause"."DisplayOrder" IS 'Default display order.';
COMMENT ON COLUMN crm."FgsEstimateClause"."IsActive" IS 'Indicates whether the clause is available for use.';
COMMENT ON COLUMN crm."FgsEstimateClause"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateClause"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateClause"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateClause"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimateFlavor" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "FlavorCode" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "BackgroundColor" character varying(20) NOT NULL,
    "TextColor" character varying(20) NOT NULL,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateFlavor" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FgsEstimateFlavor_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateFlavor" IS 'Stores estimate flavor definitions used to visually categorize estimate options such as Good, Better, Best, Popular, Premium, Bronze, Silver, and Gold.';

CREATE TABLE crm."FgsEstimateStatus" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "StatusCode" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateStatus" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FgsEstimateStatus_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateStatus" IS 'Stores estimate statuses available to a specific tenant/company. StatusCode is immutable and used by application business logic.';
COMMENT ON COLUMN crm."FgsEstimateStatus"."StatusCode" IS 'Immutable system status code used by application business logic.';
COMMENT ON COLUMN crm."FgsEstimateStatus"."Name" IS 'User-facing display name that may be customized by the tenant.';

CREATE TABLE crm."FgsEstimateTemplateCategory" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "CategoryCode" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500),
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateTemplateCategory" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateTemplateCategory_DisplayOrder" CHECK ("DisplayOrder" > 0),
    CONSTRAINT "FK_FgsEstimateTemplateCategory_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateTemplateCategory" IS 'Stores estimate template categories used to organize estimate templates into logical groups.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."CategoryCode" IS 'Unique internal category code within a company.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."Name" IS 'User-facing category name.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."Description" IS 'Optional category description.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."DisplayOrder" IS 'Controls display sequence of categories.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateTemplateCategory"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimate" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "EstimateNumber" character varying(50) NOT NULL,
    "EstimateStatusId" bigint NOT NULL,
    "EstimateTypeId" bigint NOT NULL,
    "EstimateSourceId" bigint,
    "OpportunityId" bigint,
    "CustomerId" bigint NOT NULL,
    "ServiceLocationId" bigint NOT NULL,
    "WorkOrderId" bigint,
    "JobTypeId" bigint,
    "PaymentTermId" bigint,
    "PaymentMethodId" bigint,
    "Break1Id" bigint,
    "Break2Id" bigint,
    "QuoteName" character varying(255) NOT NULL,
    "QuoteDescription" text,
    "EstimateDate" date NOT NULL,
    "ExpirationDate" date,
    "QuotedByEmployeeId" bigint,
    "SoldByEmployeeId" bigint,
    "SelectedEstimateOptionId" bigint,
    "SignedBy" character varying(255),
    "SignedOn" timestamptz,
    "SignatureFileId" bigint,
    "TaxAuthoritySnapshotJson" jsonb,
    "MaterialPricingMatrixId" bigint,
    "LaborPricingMatrixId" bigint,
    "OtherPricingMatrixId" bigint,
    "SubtotalAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "DiscountAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "TaxAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "TotalAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "GrossProfitAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "GrossProfitPercent" numeric(9,4) NOT NULL DEFAULT 0.0,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimate" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FgsEstimate_EstimateStatus" FOREIGN KEY ("EstimateStatusId") REFERENCES crm."FgsEstimateStatus" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimate_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimate" IS 'Stores estimate header information and pricing totals for the selected/sold estimate option.';
COMMENT ON COLUMN crm."FgsEstimate"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimate"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimate"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimate"."EstimateNumber" IS 'User-facing estimate number.';
COMMENT ON COLUMN crm."FgsEstimate"."EstimateStatusId" IS 'Current estimate status.';
COMMENT ON COLUMN crm."FgsEstimate"."EstimateTypeId" IS 'Estimate presentation style such as Single Option or Good Better Best.';
COMMENT ON COLUMN crm."FgsEstimate"."EstimateSourceId" IS 'Source that originated the estimate.';
COMMENT ON COLUMN crm."FgsEstimate"."OpportunityId" IS 'Associated opportunity.';
COMMENT ON COLUMN crm."FgsEstimate"."CustomerId" IS 'Associated customer.';
COMMENT ON COLUMN crm."FgsEstimate"."ServiceLocationId" IS 'Service location where work will be performed.';
COMMENT ON COLUMN crm."FgsEstimate"."WorkOrderId" IS 'Work order generated from the estimate.';
COMMENT ON COLUMN crm."FgsEstimate"."JobTypeId" IS 'Job type associated with the estimate.';
COMMENT ON COLUMN crm."FgsEstimate"."PaymentTermId" IS 'Payment terms applicable to the estimate.';
COMMENT ON COLUMN crm."FgsEstimate"."PaymentMethodId" IS 'Preferred payment method for the estimate.';
COMMENT ON COLUMN crm."FgsEstimate"."Break1Id" IS 'First accounting segment used for GL exports and reporting.';
COMMENT ON COLUMN crm."FgsEstimate"."Break2Id" IS 'Second accounting segment used for GL exports and reporting.';
COMMENT ON COLUMN crm."FgsEstimate"."QuoteName" IS 'User-facing quote name.';
COMMENT ON COLUMN crm."FgsEstimate"."QuoteDescription" IS 'Detailed quote description presented to the customer.';
COMMENT ON COLUMN crm."FgsEstimate"."EstimateDate" IS 'Date estimate was created.';
COMMENT ON COLUMN crm."FgsEstimate"."ExpirationDate" IS 'Date estimate expires.';
COMMENT ON COLUMN crm."FgsEstimate"."QuotedByEmployeeId" IS 'Employee who prepared or presented the estimate.';
COMMENT ON COLUMN crm."FgsEstimate"."SoldByEmployeeId" IS 'Employee credited with the sale.';
COMMENT ON COLUMN crm."FgsEstimate"."SelectedEstimateOptionId" IS 'Estimate option selected by the customer.';
COMMENT ON COLUMN crm."FgsEstimate"."SignedBy" IS 'Name entered by the person signing the estimate.';
COMMENT ON COLUMN crm."FgsEstimate"."SignedOn" IS 'Date and time the estimate was signed.';
COMMENT ON COLUMN crm."FgsEstimate"."SignatureFileId" IS 'File identifier pointing to the signature image stored in file.FgsFile.';
COMMENT ON COLUMN crm."FgsEstimate"."TaxAuthoritySnapshotJson" IS 'Historical snapshot of tax authority codes, names, and rates used for tax calculations.';
COMMENT ON COLUMN crm."FgsEstimate"."MaterialPricingMatrixId" IS 'Material pricing matrix used for pricing calculations.';
COMMENT ON COLUMN crm."FgsEstimate"."LaborPricingMatrixId" IS 'Labor pricing matrix used for pricing calculations.';
COMMENT ON COLUMN crm."FgsEstimate"."OtherPricingMatrixId" IS 'Other pricing matrix used for pricing calculations.';
COMMENT ON COLUMN crm."FgsEstimate"."SubtotalAmount" IS 'Subtotal before discounts and taxes.';
COMMENT ON COLUMN crm."FgsEstimate"."DiscountAmount" IS 'Total discount amount.';
COMMENT ON COLUMN crm."FgsEstimate"."TaxAmount" IS 'Total tax amount.';
COMMENT ON COLUMN crm."FgsEstimate"."TotalAmount" IS 'Final estimate amount.';
COMMENT ON COLUMN crm."FgsEstimate"."GrossProfitAmount" IS 'Gross profit amount.';
COMMENT ON COLUMN crm."FgsEstimate"."GrossProfitPercent" IS 'Gross profit percentage.';
COMMENT ON COLUMN crm."FgsEstimate"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimate"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimate"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimate"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimateTemplate" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "CategoryId" bigint NOT NULL,
    "TemplateCode" character varying(50) NOT NULL,
    "Name" character varying(255) NOT NULL,
    "TemplateDescription" text,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "ShowToFieldTechnician" boolean NOT NULL DEFAULT TRUE,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateTemplate" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateTemplate_DisplayOrder" CHECK ("DisplayOrder" > 0),
    CONSTRAINT "FK_FgsEstimateTemplate_Category" FOREIGN KEY ("CategoryId") REFERENCES crm."FgsEstimateTemplateCategory" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimateTemplate_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateTemplate" IS 'Stores reusable estimate templates used to generate estimate options and pricing lines.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."CategoryId" IS 'Template category.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."TemplateCode" IS 'Unique internal template code within a company.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."Name" IS 'User-facing template name.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."TemplateDescription" IS 'Description copied into estimate description when estimate is generated from template.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."DisplayOrder" IS 'Controls display sequence within a category.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."ShowToFieldTechnician" IS 'Indicates whether template-generated content should be visible to field technicians.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."IsActive" IS 'Indicates whether template is available for use.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateTemplate"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimateClauseItem" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "EstimateId" bigint NOT NULL,
    "ClauseId" bigint,
    "ClauseTypeId" bigint NOT NULL,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "ClauseName" character varying(255) NOT NULL,
    "ClauseText" text NOT NULL,
    "ShowOnProposal" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateClauseItem" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateClauseItem_DisplayOrder" CHECK ("DisplayOrder" > 0),
    CONSTRAINT "FK_FgsEstimateClauseItem_Clause" FOREIGN KEY ("ClauseId") REFERENCES crm."FgsEstimateClause" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_FgsEstimateClauseItem_Estimate" FOREIGN KEY ("EstimateId") REFERENCES crm."FgsEstimate" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_FgsEstimateClauseItem_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateClauseItem" IS 'Stores estimate-specific clause snapshots. Changes to the clause library do not affect existing estimates.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."EstimateId" IS 'Parent estimate.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."ClauseId" IS 'Source clause from crm.FgsEstimateClause.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."ClauseTypeId" IS 'Snapshot of clause type such as Inclusion, Exclusion, or Terms and Conditions.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."DisplayOrder" IS 'Controls display sequence on estimate documents.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."ClauseName" IS 'Snapshot of clause name at the time it was added to the estimate.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."ClauseText" IS 'Snapshot of clause text at the time it was added to the estimate.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."ShowOnProposal" IS 'Indicates whether the clause should be displayed on customer-facing proposal documents.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateClauseItem"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimateOption" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "EstimateId" bigint NOT NULL,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "OptionName" character varying(255) NOT NULL,
    "OptionDescription" text,
    "IsRecommended" boolean NOT NULL DEFAULT FALSE,
    "IsSelected" boolean NOT NULL DEFAULT FALSE,
    "SelectedOn" timestamptz,
    "SubtotalAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "DiscountAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "TaxAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "TotalAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
    "InternalNotes" text,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateOption" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateOption_DiscountAmount" CHECK ("DiscountAmount" >= 0),
    CONSTRAINT "CK_FgsEstimateOption_DisplayOrder" CHECK ("DisplayOrder" > 0),
    CONSTRAINT "CK_FgsEstimateOption_SubtotalAmount" CHECK ("SubtotalAmount" >= 0),
    CONSTRAINT "CK_FgsEstimateOption_TaxAmount" CHECK ("TaxAmount" >= 0),
    CONSTRAINT "CK_FgsEstimateOption_TotalAmount" CHECK ("TotalAmount" >= 0),
    CONSTRAINT "FK_FgsEstimateOption_Estimate" FOREIGN KEY ("EstimateId") REFERENCES crm."FgsEstimate" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimateOption_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateOption" IS 'Stores sellable estimate options/packages belonging to an estimate. Detailed pricing is stored in crm.FgsEstimateOptionLine.';
COMMENT ON COLUMN crm."FgsEstimateOption"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateOption"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateOption"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateOption"."EstimateId" IS 'Parent estimate.';
COMMENT ON COLUMN crm."FgsEstimateOption"."DisplayOrder" IS 'Display order within the estimate.';
COMMENT ON COLUMN crm."FgsEstimateOption"."OptionName" IS 'Customer-facing option name.';
COMMENT ON COLUMN crm."FgsEstimateOption"."OptionDescription" IS 'Detailed customer-facing option description.';
COMMENT ON COLUMN crm."FgsEstimateOption"."IsRecommended" IS 'Indicates whether the option is highlighted as the recommended option.';
COMMENT ON COLUMN crm."FgsEstimateOption"."IsSelected" IS 'Indicates whether the customer selected this option.';
COMMENT ON COLUMN crm."FgsEstimateOption"."SelectedOn" IS 'Date and time the option was selected by the customer.';
COMMENT ON COLUMN crm."FgsEstimateOption"."SubtotalAmount" IS 'Option subtotal amount.';
COMMENT ON COLUMN crm."FgsEstimateOption"."DiscountAmount" IS 'Option discount amount.';
COMMENT ON COLUMN crm."FgsEstimateOption"."TaxAmount" IS 'Option tax amount.';
COMMENT ON COLUMN crm."FgsEstimateOption"."TotalAmount" IS 'Option total amount.';
COMMENT ON COLUMN crm."FgsEstimateOption"."InternalNotes" IS 'Internal notes not visible to customers.';
COMMENT ON COLUMN crm."FgsEstimateOption"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateOption"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateOption"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateOption"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimateTemplateOption" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "EstimateTemplateId" bigint NOT NULL,
    "EstimateFlavorId" bigint NOT NULL,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "OptionName" character varying(255) NOT NULL,
    "OptionDescription" text,
    "ShowOnProposal" boolean NOT NULL DEFAULT TRUE,
    "ShowPriceOnProposal" boolean NOT NULL DEFAULT TRUE,
    "IsSelectedByDefault" boolean NOT NULL DEFAULT FALSE,
    "AllowQuantityChange" boolean NOT NULL DEFAULT TRUE,
    "AllowPriceChange" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateTemplateOption" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateTemplateOption_DisplayOrder" CHECK ("DisplayOrder" > 0),
    CONSTRAINT "FK_FgsEstimateTemplateOption_EstimateFlavor" FOREIGN KEY ("EstimateFlavorId") REFERENCES crm."FgsEstimateFlavor" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimateTemplateOption_EstimateTemplate" FOREIGN KEY ("EstimateTemplateId") REFERENCES crm."FgsEstimateTemplate" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimateTemplateOption_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateTemplateOption" IS 'Stores reusable estimate options belonging to an estimate template. Template options are copied into estimate options when a template is applied.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."EstimateTemplateId" IS 'Parent estimate template.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."EstimateFlavorId" IS 'Flavor assigned to the option such as Standard, Good, Better, Best, or Add-On.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."DisplayOrder" IS 'Controls display sequence within the template.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."OptionName" IS 'Customer-facing option name copied to the estimate option.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."OptionDescription" IS 'Customer-facing option description copied to the estimate option.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."ShowOnProposal" IS 'Indicates whether the option should be displayed on customer-facing proposals.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."ShowPriceOnProposal" IS 'Indicates whether pricing should be displayed on customer-facing proposals.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."IsSelectedByDefault" IS 'Indicates whether the option should be selected by default when the template is applied.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."AllowQuantityChange" IS 'Indicates whether quantity may be modified after template application.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."AllowPriceChange" IS 'Indicates whether pricing may be modified after template application.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOption"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimateOptionLine" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "EstimateOptionId" bigint NOT NULL,
    "ParentLineId" bigint,
    "TemplateId" bigint,
    "TemplateLineId" bigint,
    "DisplayOrder" integer NOT NULL DEFAULT 1,
    "BillingCategoryId" bigint NOT NULL,
    "ItemCode" character varying(100),
    "RateOfDayId" bigint,
    "Description" text NOT NULL,
    "ShowOnProposal" boolean NOT NULL DEFAULT TRUE,
    "ShowPriceOnProposal" boolean NOT NULL DEFAULT TRUE,
    "ShowToFieldTechnician" boolean NOT NULL DEFAULT TRUE,
    "Source" character varying(100),
    "Quantity" numeric(18,4) NOT NULL DEFAULT 1.0,
    "UnitCost" numeric(18,4) NOT NULL DEFAULT 0.0,
    "ExtendedCost" numeric(18,2) NOT NULL DEFAULT 0.0,
    "UnitPrice" numeric(18,4) NOT NULL DEFAULT 0.0,
    "ExtendedPrice" numeric(18,2) NOT NULL DEFAULT 0.0,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateOptionLine" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateOptionLine_Quantity" CHECK ("Quantity" >= 0),
    CONSTRAINT "CK_FgsEstimateOptionLine_UnitCost" CHECK ("UnitCost" >= 0),
    CONSTRAINT "CK_FgsEstimateOptionLine_UnitPrice" CHECK ("UnitPrice" >= 0),
    CONSTRAINT "FK_FgsEstimateOptionLine_EstimateOption" FOREIGN KEY ("EstimateOptionId") REFERENCES crm."FgsEstimateOption" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_FgsEstimateOptionLine_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimateOptionLine_ParentLine" FOREIGN KEY ("ParentLineId") REFERENCES crm."FgsEstimateOptionLine" ("Id") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateOptionLine" IS 'Stores detailed pricing lines belonging to an estimate option. Supports materials, labor, services, discounts, taxes, fees, and hierarchical pricing structures.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."EstimateOptionId" IS 'Parent estimate option.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."ParentLineId" IS 'Parent estimate option line used for service breakdowns, discounts, taxes, bundles, rebates, and other hierarchical structures.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."TemplateId" IS 'Source estimate template.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."TemplateLineId" IS 'Source estimate template option line.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."DisplayOrder" IS 'Display sequence within the estimate option.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."BillingCategoryId" IS 'Billing category such as Material, Labor, Service, Equipment, Discount, Tax, Fee, or Other.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."ItemCode" IS 'Associated item identifier. May represent inventory, non-inventory, service, labor, fee, or miscellaneous items.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."RateOfDayId" IS 'Rate of day applied to labor pricing such as Regular, Overtime, Double Time, Weekend, Holiday, or Emergency.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."Description" IS 'Customer-facing description, service description, tax authority name, or other detail text.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."ShowOnProposal" IS 'Indicates whether the line should be displayed on customer-facing proposal documents.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."ShowPriceOnProposal" IS 'Indicates whether price and amount should be displayed on customer-facing proposal documents.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."ShowToFieldTechnician" IS 'Indicates whether the line should be visible to field technicians.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."Source" IS 'Indicates where the line originated such as Manual, Template, ServiceItem, PricingMatrix, Bundle, Import, Clone, or System.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."Quantity" IS 'Quantity associated with the line.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."UnitCost" IS 'Cost per unit.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."ExtendedCost" IS 'Quantity multiplied by UnitCost.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."UnitPrice" IS 'Selling price per unit.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."ExtendedPrice" IS 'Quantity multiplied by UnitPrice.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateOptionLine"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE TABLE crm."FgsEstimateTemplateOptionLine" (
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "EstimateTemplateOptionId" bigint NOT NULL,
    "ParentLineId" bigint,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "BillingCategoryId" bigint NOT NULL,
    "ItemId" bigint,
    "RateOfDayId" bigint,
    "Description" character varying(500) NOT NULL,
    "ShowOnProposal" boolean NOT NULL DEFAULT TRUE,
    "ShowPriceOnProposal" boolean NOT NULL DEFAULT TRUE,
    "AllowQuantityChange" boolean NOT NULL DEFAULT TRUE,
    "AllowPriceChange" boolean NOT NULL DEFAULT TRUE,
    "Source" character varying(50),
    "Quantity" numeric(18,4) NOT NULL DEFAULT 1.0,
    "UnitCost" numeric(18,2) NOT NULL DEFAULT 0.0,
    "ExtendedCost" numeric(18,2) NOT NULL DEFAULT 0.0,
    "UnitPrice" numeric(18,2) NOT NULL DEFAULT 0.0,
    "ExtendedPrice" numeric(18,2) NOT NULL DEFAULT 0.0,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsEstimateTemplateOptionLine" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsEstimateTemplateOptionLine_DisplayOrder" CHECK ("DisplayOrder" > 0),
    CONSTRAINT "CK_FgsEstimateTemplateOptionLine_Quantity" CHECK ("Quantity" >= 0),
    CONSTRAINT "CK_FgsEstimateTemplateOptionLine_UnitCost" CHECK ("UnitCost" >= 0),
    CONSTRAINT "FK_FgsEstimateTemplateOptionLine_EstimateTemplateOption" FOREIGN KEY ("EstimateTemplateOptionId") REFERENCES crm."FgsEstimateTemplateOption" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimateTemplateOptionLine_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsEstimateTemplateOptionLine_ParentLine" FOREIGN KEY ("ParentLineId") REFERENCES crm."FgsEstimateTemplateOptionLine" ("Id") ON DELETE RESTRICT
);
COMMENT ON TABLE crm."FgsEstimateTemplateOptionLine" IS 'Stores detailed pricing lines belonging to an estimate template option and are copied into estimate option lines when a template is applied.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."TenantId" IS 'Tenant identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."CompanyId" IS 'Company identifier.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."Id" IS 'Primary key.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."EstimateTemplateOptionId" IS 'Parent estimate template option.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."ParentLineId" IS 'Parent template option line used for service breakdowns, bundles, discounts, rebates, credits, and other hierarchical pricing structures.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."DisplayOrder" IS 'Display sequence within the template option.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."BillingCategoryId" IS 'Billing category such as Material, Labor, Service, Equipment, Discount, Tax, or Other.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."ItemId" IS 'Item associated with the line.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."RateOfDayId" IS 'Rate of day applied to labor pricing such as Regular, Overtime, Double Time, Weekend, Holiday, or Emergency.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."Description" IS 'Customer-facing description or tax authority name.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."ShowOnProposal" IS 'Indicates whether the line should be displayed on customer-facing proposals.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."ShowPriceOnProposal" IS 'Indicates whether pricing amounts should be displayed on customer-facing proposals.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."AllowQuantityChange" IS 'Indicates whether quantity may be modified after template application.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."AllowPriceChange" IS 'Indicates whether pricing may be modified after template application.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."Source" IS 'Identifies where the line originated such as Manual, ServiceItem, PricingMatrix, Bundle, Import, or Clone.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."Quantity" IS 'Default quantity applied when template is used.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."UnitCost" IS 'Default cost per unit.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."ExtendedCost" IS 'Quantity multiplied by UnitCost.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."UnitPrice" IS 'Default selling price per unit.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."ExtendedPrice" IS 'Quantity multiplied by UnitPrice.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."CreatedBy" IS 'User or process that created the record.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN crm."FgsEstimateTemplateOptionLine"."UpdatedBy" IS 'User or process that last updated the record.';

CREATE INDEX "IX_FgsEstimate_EstimateStatusId" ON crm."FgsEstimate" ("EstimateStatusId");

CREATE INDEX "IX_FgsEstimate_TenantId_CompanyId" ON crm."FgsEstimate" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimate_TenantId_CompanyId_CustomerId" ON crm."FgsEstimate" ("TenantId", "CompanyId", "CustomerId");

CREATE INDEX "IX_FgsEstimate_TenantId_CompanyId_EstimateStatusId" ON crm."FgsEstimate" ("TenantId", "CompanyId", "EstimateStatusId");

CREATE INDEX "IX_FgsEstimate_TenantId_CompanyId_ServiceLocationId" ON crm."FgsEstimate" ("TenantId", "CompanyId", "ServiceLocationId");

CREATE UNIQUE INDEX "UX_FgsEstimate_TenantId_CompanyId_EstimateNumber" ON crm."FgsEstimate" ("TenantId", "CompanyId", "EstimateNumber");

CREATE UNIQUE INDEX "UX_FgsEstimate_TenantId_CompanyId_OpportunityId" ON crm."FgsEstimate" ("TenantId", "CompanyId", "OpportunityId") WHERE "OpportunityId" IS NOT NULL;

CREATE UNIQUE INDEX "UX_FgsEstimate_TenantId_CompanyId_WorkOrderId" ON crm."FgsEstimate" ("TenantId", "CompanyId", "WorkOrderId") WHERE "WorkOrderId" IS NOT NULL;

CREATE INDEX "IX_FgsEstimateClause_TenantId_CompanyId" ON crm."FgsEstimateClause" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimateClause_TenantId_CompanyId_ClauseTypeId" ON crm."FgsEstimateClause" ("TenantId", "CompanyId", "ClauseTypeId");

CREATE INDEX "IX_FgsEstimateClause_TenantId_CompanyId_DisplayOrder" ON crm."FgsEstimateClause" ("TenantId", "CompanyId", "DisplayOrder");

CREATE UNIQUE INDEX "UX_FgsEstimateClause_TenantId_CompanyId_ClauseTypeId_ClauseName" ON crm."FgsEstimateClause" ("TenantId", "CompanyId", "ClauseTypeId", "ClauseName");

CREATE INDEX "IX_FgsEstimateClauseItem_ClauseId" ON crm."FgsEstimateClauseItem" ("ClauseId");

CREATE INDEX "IX_FgsEstimateClauseItem_EstimateId" ON crm."FgsEstimateClauseItem" ("EstimateId");

CREATE INDEX "IX_FgsEstimateClauseItem_TenantId_CompanyId" ON crm."FgsEstimateClauseItem" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimateClauseItem_TenantId_CompanyId_ClauseTypeId" ON crm."FgsEstimateClauseItem" ("TenantId", "CompanyId", "ClauseTypeId");

CREATE INDEX "IX_FgsEstimateClauseItem_TenantId_CompanyId_DisplayOrder" ON crm."FgsEstimateClauseItem" ("TenantId", "CompanyId", "DisplayOrder");

CREATE INDEX "IX_FgsEstimateClauseItem_TenantId_CompanyId_EstimateId" ON crm."FgsEstimateClauseItem" ("TenantId", "CompanyId", "EstimateId");

CREATE UNIQUE INDEX "UX_FgsEstimateClauseItem_TenantId_CompanyId_EstimateId_DisplayOrder" ON crm."FgsEstimateClauseItem" ("TenantId", "CompanyId", "EstimateId", "DisplayOrder");

CREATE INDEX "IX_FgsEstimateFlavor_TenantId_CompanyId" ON crm."FgsEstimateFlavor" ("TenantId", "CompanyId");

CREATE UNIQUE INDEX "UX_FgsEstimateFlavor_TenantId_CompanyId_FlavorCode" ON crm."FgsEstimateFlavor" ("TenantId", "CompanyId", "FlavorCode");

CREATE INDEX "IX_FgsEstimateOption_EstimateId" ON crm."FgsEstimateOption" ("EstimateId");

CREATE INDEX "IX_FgsEstimateOption_TenantId_CompanyId" ON crm."FgsEstimateOption" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimateOption_TenantId_CompanyId_EstimateId" ON crm."FgsEstimateOption" ("TenantId", "CompanyId", "EstimateId");

CREATE INDEX "IX_FgsEstimateOptionLine_EstimateOptionId" ON crm."FgsEstimateOptionLine" ("EstimateOptionId");

CREATE INDEX "IX_FgsEstimateOptionLine_ParentLineId" ON crm."FgsEstimateOptionLine" ("ParentLineId");

CREATE INDEX "IX_FgsEstimateOptionLine_TenantId_CompanyId" ON crm."FgsEstimateOptionLine" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimateOptionLine_TenantId_CompanyId_DisplayOrder" ON crm."FgsEstimateOptionLine" ("TenantId", "CompanyId", "EstimateOptionId", "DisplayOrder");

CREATE INDEX "IX_FgsEstimateOptionLine_TenantId_CompanyId_EstimateOptionId" ON crm."FgsEstimateOptionLine" ("TenantId", "CompanyId", "EstimateOptionId");

CREATE INDEX "IX_FgsEstimateOptionLine_TenantId_CompanyId_ParentLineId" ON crm."FgsEstimateOptionLine" ("TenantId", "CompanyId", "ParentLineId");

CREATE INDEX "IX_FgsEstimateStatus_TenantId_CompanyId" ON crm."FgsEstimateStatus" ("TenantId", "CompanyId");

CREATE UNIQUE INDEX "UX_FgsEstimateStatus_TenantId_CompanyId_Name" ON crm."FgsEstimateStatus" ("TenantId", "CompanyId", "Name");

CREATE UNIQUE INDEX "UX_FgsEstimateStatus_TenantId_CompanyId_StatusCode" ON crm."FgsEstimateStatus" ("TenantId", "CompanyId", "StatusCode");

CREATE INDEX "IX_FgsEstimateTemplate_CategoryId" ON crm."FgsEstimateTemplate" ("CategoryId");

CREATE INDEX "IX_FgsEstimateTemplate_TenantId_CompanyId" ON crm."FgsEstimateTemplate" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimateTemplate_TenantId_CompanyId_CategoryId" ON crm."FgsEstimateTemplate" ("TenantId", "CompanyId", "CategoryId");

CREATE UNIQUE INDEX "UX_FgsEstimateTemplate_TenantId_CompanyId_CategoryId_Name" ON crm."FgsEstimateTemplate" ("TenantId", "CompanyId", "CategoryId", "Name");

CREATE UNIQUE INDEX "UX_FgsEstimateTemplate_TenantId_CompanyId_TemplateCode" ON crm."FgsEstimateTemplate" ("TenantId", "CompanyId", "TemplateCode");

CREATE INDEX "IX_FgsEstimateTemplateCategory_TenantId_CompanyId" ON crm."FgsEstimateTemplateCategory" ("TenantId", "CompanyId");

CREATE UNIQUE INDEX "UX_FgsEstimateTemplateCategory_TenantId_CompanyId_CategoryCode" ON crm."FgsEstimateTemplateCategory" ("TenantId", "CompanyId", "CategoryCode");

CREATE UNIQUE INDEX "UX_FgsEstimateTemplateCategory_TenantId_CompanyId_Name" ON crm."FgsEstimateTemplateCategory" ("TenantId", "CompanyId", "Name");

CREATE INDEX "IX_FgsEstimateTemplateOption_EstimateFlavorId" ON crm."FgsEstimateTemplateOption" ("EstimateFlavorId");

CREATE INDEX "IX_FgsEstimateTemplateOption_EstimateTemplateId" ON crm."FgsEstimateTemplateOption" ("EstimateTemplateId");

CREATE INDEX "IX_FgsEstimateTemplateOption_TenantId_CompanyId" ON crm."FgsEstimateTemplateOption" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateFlavorId" ON crm."FgsEstimateTemplateOption" ("TenantId", "CompanyId", "EstimateFlavorId");

CREATE INDEX "IX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateTemplateId" ON crm."FgsEstimateTemplateOption" ("TenantId", "CompanyId", "EstimateTemplateId");

CREATE UNIQUE INDEX "UX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateTemplateId_DisplayOrder" ON crm."FgsEstimateTemplateOption" ("TenantId", "CompanyId", "EstimateTemplateId", "DisplayOrder");

CREATE INDEX "IX_FgsEstimateTemplateOptionLine_EstimateTemplateOptionId" ON crm."FgsEstimateTemplateOptionLine" ("EstimateTemplateOptionId");

CREATE INDEX "IX_FgsEstimateTemplateOptionLine_ParentLineId" ON crm."FgsEstimateTemplateOptionLine" ("ParentLineId");

CREATE INDEX "IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId" ON crm."FgsEstimateTemplateOptionLine" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId_EstimateTemplateOptionId" ON crm."FgsEstimateTemplateOptionLine" ("TenantId", "CompanyId", "EstimateTemplateOptionId");

CREATE INDEX "IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId_ParentLineId" ON crm."FgsEstimateTemplateOptionLine" ("TenantId", "CompanyId", "ParentLineId");

INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260616191200_AddFgsEstimateEntities', '10.0.8');

COMMIT;

