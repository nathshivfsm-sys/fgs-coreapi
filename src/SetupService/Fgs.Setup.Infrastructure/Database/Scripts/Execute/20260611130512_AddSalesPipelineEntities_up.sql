START TRANSACTION;
ALTER TABLE IF EXISTS setup."FgsEntityTag"
    DROP CONSTRAINT IF EXISTS "FK_FgsEntityTag_GloMasterEntityType_MasterEntityTypeId";
ALTER TABLE IF EXISTS setup."FgsSetupPricingMatrixLabor"
    DROP CONSTRAINT IF EXISTS "FK_FgsSetupPricingMatrixLabor_LaborRateType";
ALTER TABLE IF EXISTS setup."FgsVehicleMaintenance"
    DROP CONSTRAINT IF EXISTS "FK_FgsVehicleMaintenance_GloVehicleMaintenanceType_VehicleMaintenanceTypeId";
DROP INDEX IF EXISTS setup."IX_FgsVehicleMaintenance_VehicleMaintenanceTypeId";
DROP INDEX IF EXISTS setup."IX_FgsSetupPricingMatrixLabor_LaborRateTypeId";
DROP INDEX IF EXISTS setup."IX_FgsEntityTag_MasterEntityTypeId1";

CREATE TABLE setup."FgsSalesActivityType" (
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "ActivityTypeCode" character varying(50) NOT NULL,
    "ActivityTypeName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT TRUE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT TRUE,
    "AllowManualSelection" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsSystem" boolean NOT NULL DEFAULT FALSE,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsSalesActivityType" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsSalesActivityType_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true),
    CONSTRAINT "FK_FgsSalesActivityType_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES setup."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE setup."FgsSalesActivityType" IS 'Stores tenant/company specific sales activity types used by Leads and Opportunities. Seeded from glo.GloSalesActivityType.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."Id" IS 'Unique identifier for the sales activity type.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."TenantId" IS 'Tenant identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."CompanyId" IS 'Company identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."ActivityTypeCode" IS 'Immutable business code for the sales activity type.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."ActivityTypeName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."Description" IS 'Optional description explaining the sales activity type.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."AppliesToLead" IS 'Indicates whether the activity type can be used by Leads.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."AppliesToOpportunity" IS 'Indicates whether the activity type can be used by Opportunities.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."AllowManualSelection" IS 'Indicates whether users may manually select this activity type.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."DisplayOrder" IS 'Controls the order in which activity types are displayed.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."IsSystem" IS 'Indicates whether the activity type was seeded by the system. System records should have immutable business codes.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."IsActive" IS 'Indicates whether the activity type is available for use.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."CreatedBy" IS 'User who created the record.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN setup."FgsSalesActivityType"."UpdatedBy" IS 'User who last updated the record.';

CREATE TABLE setup."FgsSalesDispositionReason" (
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "DispositionReasonCode" character varying(50) NOT NULL,
    "DispositionReasonName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT FALSE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT FALSE,
    "RequireComment" boolean NOT NULL DEFAULT FALSE,
    "IsTerminal" boolean NOT NULL DEFAULT TRUE,
    "AllowManualSelection" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsSystem" boolean NOT NULL DEFAULT FALSE,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsSalesDispositionReason" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsSalesDispositionReason_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true),
    CONSTRAINT "FK_FgsSalesDispositionReason_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES setup."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE setup."FgsSalesDispositionReason" IS 'Stores tenant/company specific sales disposition reasons used when a Lead is Disqualified or an Opportunity is Lost. Seeded from glo.GloSalesDispositionReason.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."Id" IS 'Unique identifier for the sales disposition reason.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."TenantId" IS 'Tenant identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."CompanyId" IS 'Company identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."DispositionReasonCode" IS 'Immutable business code for the disposition reason.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."DispositionReasonName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."Description" IS 'Optional description explaining the disposition reason.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."AppliesToLead" IS 'Indicates whether the reason can be used when a Lead is Disqualified.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."AppliesToOpportunity" IS 'Indicates whether the reason can be used when an Opportunity is Lost.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."RequireComment" IS 'Indicates whether users must provide additional comments when selecting this disposition reason.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."IsTerminal" IS 'Indicates whether selecting this disposition reason should result in a terminal pipeline status.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."AllowManualSelection" IS 'Indicates whether users may manually select this disposition reason.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."DisplayOrder" IS 'Controls the order in which disposition reasons are displayed.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."IsSystem" IS 'Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."IsActive" IS 'Indicates whether the disposition reason is available for use.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."CreatedBy" IS 'User who created the record.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN setup."FgsSalesDispositionReason"."UpdatedBy" IS 'User who last updated the record.';

CREATE TABLE setup."FgsSalesPipelineStatus" (
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "StatusCode" character varying(50) NOT NULL,
    "StatusName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT FALSE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT FALSE,
    "IsTerminal" boolean NOT NULL DEFAULT FALSE,
    "AllowManualSelection" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsSystem" boolean NOT NULL DEFAULT FALSE,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsSalesPipelineStatus" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsSalesPipelineStatus_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true),
    CONSTRAINT "FK_FgsSalesPipelineStatus_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES setup."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE setup."FgsSalesPipelineStatus" IS 'Stores tenant/company specific sales pipeline statuses used by Leads and Opportunities. Seeded from glo.GloSalesPipelineStatus.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."Id" IS 'Unique identifier for the sales pipeline status.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."TenantId" IS 'Tenant identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."CompanyId" IS 'Company identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."StatusCode" IS 'Immutable business code for the sales pipeline status.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."StatusName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."Description" IS 'Optional description explaining the purpose of the status.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."AppliesToLead" IS 'Indicates whether the status can be used by Leads.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."AppliesToOpportunity" IS 'Indicates whether the status can be used by Opportunities.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."IsTerminal" IS 'Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."AllowManualSelection" IS 'Indicates whether users may manually select this status.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."DisplayOrder" IS 'Controls the order in which statuses are displayed.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."IsSystem" IS 'Indicates whether the status was seeded by the system. System records should have immutable business codes.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."IsActive" IS 'Indicates whether the status is available for use.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."CreatedBy" IS 'User who created the record.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN setup."FgsSalesPipelineStatus"."UpdatedBy" IS 'User who last updated the record.';

CREATE TABLE glo."GloSalesActivityOutcome" (
    "Id" smallint GENERATED BY DEFAULT AS IDENTITY,
    "OutcomeCode" character varying(50) NOT NULL,
    "OutcomeName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT TRUE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT TRUE,
    "NextSalesPipelineStatusCode" character varying(50),
    "IsTerminal" boolean NOT NULL DEFAULT FALSE,
    "RequireComment" boolean NOT NULL DEFAULT FALSE,
    "AllowManualSelection" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsActive" boolean NOT NULL,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "UpdatedOn" timestamptz,
    "CreatedBy" character varying(100),
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_GloSalesActivityOutcome" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_GloSalesActivityOutcome_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true)
);
COMMENT ON TABLE glo."GloSalesActivityOutcome" IS 'Master list of sales activity outcomes used by Leads and Opportunities. Outcomes represent the result of a sales interaction and may optionally suggest the next sales pipeline status. Seeded into setup.FgsSalesActivityOutcome.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."Id" IS 'Unique identifier for the sales activity outcome.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."OutcomeCode" IS 'Immutable business code for the sales activity outcome.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."OutcomeName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."Description" IS 'Optional description explaining the sales activity outcome.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."AppliesToLead" IS 'Indicates whether the outcome can be used by Leads.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."AppliesToOpportunity" IS 'Indicates whether the outcome can be used by Opportunities.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."NextSalesPipelineStatusCode" IS 'Suggested sales pipeline status code that should be applied when this outcome is selected.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."IsTerminal" IS 'Indicates whether selecting this outcome typically results in a terminal sales pipeline status.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."RequireComment" IS 'Indicates whether users must provide additional comments when selecting this outcome.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."AllowManualSelection" IS 'Indicates whether users may manually select this outcome.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."DisplayOrder" IS 'Controls the order in which outcomes are displayed.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN glo."GloSalesActivityOutcome"."UpdatedOn" IS 'Date and time the record was last updated.';

CREATE TABLE glo."GloSalesActivityType" (
    "Id" smallint GENERATED BY DEFAULT AS IDENTITY,
    "ActivityTypeCode" character varying(50) NOT NULL,
    "ActivityTypeName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT TRUE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT TRUE,
    "AllowManualSelection" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsActive" boolean NOT NULL,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "UpdatedOn" timestamptz,
    "CreatedBy" character varying(100),
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_GloSalesActivityType" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_GloSalesActivityType_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true)
);
COMMENT ON TABLE glo."GloSalesActivityType" IS 'Master list of sales activity types used by Leads and Opportunities. Seeded into setup.FgsSalesActivityType.';
COMMENT ON COLUMN glo."GloSalesActivityType"."Id" IS 'Unique identifier for the sales activity type.';
COMMENT ON COLUMN glo."GloSalesActivityType"."ActivityTypeCode" IS 'Immutable business code for the sales activity type.';
COMMENT ON COLUMN glo."GloSalesActivityType"."ActivityTypeName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN glo."GloSalesActivityType"."Description" IS 'Optional description explaining the sales activity type.';
COMMENT ON COLUMN glo."GloSalesActivityType"."AppliesToLead" IS 'Indicates whether the activity type can be used by Leads.';
COMMENT ON COLUMN glo."GloSalesActivityType"."AppliesToOpportunity" IS 'Indicates whether the activity type can be used by Opportunities.';
COMMENT ON COLUMN glo."GloSalesActivityType"."AllowManualSelection" IS 'Indicates whether users may manually select this activity type.';
COMMENT ON COLUMN glo."GloSalesActivityType"."DisplayOrder" IS 'Controls the order in which activity types are displayed.';
COMMENT ON COLUMN glo."GloSalesActivityType"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN glo."GloSalesActivityType"."UpdatedOn" IS 'Date and time the record was last updated.';

CREATE TABLE glo."GloSalesDispositionReason" (
    "Id" smallint GENERATED BY DEFAULT AS IDENTITY,
    "DispositionReasonCode" character varying(50) NOT NULL,
    "DispositionReasonName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT FALSE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT FALSE,
    "RequireComment" boolean NOT NULL DEFAULT FALSE,
    "IsTerminal" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsActive" boolean NOT NULL,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "UpdatedOn" timestamptz,
    "CreatedBy" character varying(100),
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_GloSalesDispositionReason" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_GloSalesDispositionReason_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true)
);
COMMENT ON TABLE glo."GloSalesDispositionReason" IS 'Master list of sales disposition reasons used when a Lead is Disqualified or an Opportunity is Lost. Seeded into setup.FgsSalesDispositionReason.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."Id" IS 'Unique identifier for the sales disposition reason.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."DispositionReasonCode" IS 'Immutable business code for the disposition reason.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."DispositionReasonName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."Description" IS 'Optional description explaining the disposition reason.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."AppliesToLead" IS 'Indicates whether the reason can be used when a Lead is Disqualified.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."AppliesToOpportunity" IS 'Indicates whether the reason can be used when an Opportunity is Lost.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."RequireComment" IS 'Indicates whether users must provide additional comments when selecting this disposition reason.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."IsTerminal" IS 'Indicates whether selecting this disposition reason should result in a terminal pipeline status such as Lost or Disqualified.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."DisplayOrder" IS 'Controls the order in which reasons are displayed.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN glo."GloSalesDispositionReason"."UpdatedOn" IS 'Date and time the record was last updated.';

CREATE TABLE glo."GloSalesPipelineStatus" (
    "Id" smallint GENERATED BY DEFAULT AS IDENTITY,
    "StatusCode" character varying(50) NOT NULL,
    "StatusName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT FALSE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT FALSE,
    "IsTerminal" boolean NOT NULL DEFAULT FALSE,
    "AllowManualSelection" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsActive" boolean NOT NULL,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "UpdatedOn" timestamptz,
    "CreatedBy" character varying(100),
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_GloSalesPipelineStatus" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_GloSalesPipelineStatus_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true)
);
COMMENT ON TABLE glo."GloSalesPipelineStatus" IS 'Master list of sales pipeline statuses used by Leads and Opportunities. Seeded into setup.FgsSalesPipelineStatus.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."Id" IS 'Unique identifier for the sales pipeline status.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."StatusCode" IS 'Immutable business code for the sales pipeline status.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."StatusName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."Description" IS 'Optional description explaining the purpose of the status.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."AppliesToLead" IS 'Indicates whether the status can be used by Leads.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."AppliesToOpportunity" IS 'Indicates whether the status can be used by Opportunities.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."IsTerminal" IS 'Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."AllowManualSelection" IS 'Indicates whether users may manually select this status. When false, the status should be reached through workflow actions or automation.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."DisplayOrder" IS 'Controls the order in which statuses are displayed.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN glo."GloSalesPipelineStatus"."UpdatedOn" IS 'Date and time the record was last updated.';

CREATE TABLE setup."FgsSalesActivityOutcome" (
    "Id" bigint GENERATED ALWAYS AS IDENTITY,
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "OutcomeCode" character varying(50) NOT NULL,
    "OutcomeName" character varying(100) NOT NULL,
    "Description" character varying(255),
    "AppliesToLead" boolean NOT NULL DEFAULT TRUE,
    "AppliesToOpportunity" boolean NOT NULL DEFAULT TRUE,
    "NextSalesPipelineStatusId" bigint,
    "IsTerminal" boolean NOT NULL DEFAULT FALSE,
    "RequireComment" boolean NOT NULL DEFAULT FALSE,
    "AllowManualSelection" boolean NOT NULL DEFAULT TRUE,
    "DisplayOrder" smallint NOT NULL DEFAULT 1,
    "IsSystem" boolean NOT NULL DEFAULT FALSE,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    "CreatedBy" character varying(100),
    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),
    CONSTRAINT "PK_FgsSalesActivityOutcome" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_FgsSalesActivityOutcome_AppliesToEntity" CHECK ("AppliesToLead" = true OR "AppliesToOpportunity" = true),
    CONSTRAINT "FK_FgsSalesActivityOutcome_FgsSalesPipelineStatus_NextSalesPipelineStatusId" FOREIGN KEY ("NextSalesPipelineStatusId") REFERENCES setup."FgsSalesPipelineStatus" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_FgsSalesActivityOutcome_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES setup."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
);
COMMENT ON TABLE setup."FgsSalesActivityOutcome" IS 'Stores tenant/company specific sales activity outcomes used by Leads and Opportunities. Outcomes represent the result of a sales interaction and may optionally suggest the next sales pipeline status. Seeded from glo.GloSalesActivityOutcome.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."Id" IS 'Unique identifier for the sales activity outcome.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."TenantId" IS 'Tenant identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."CompanyId" IS 'Company identifier that owns the record.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."OutcomeCode" IS 'Immutable business code for the sales activity outcome.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."OutcomeName" IS 'User-friendly name displayed throughout the application.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."Description" IS 'Optional description explaining the sales activity outcome.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."AppliesToLead" IS 'Indicates whether the outcome can be used by Leads.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."AppliesToOpportunity" IS 'Indicates whether the outcome can be used by Opportunities.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."NextSalesPipelineStatusId" IS 'Suggested sales pipeline status that should be applied when this outcome is selected.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."IsTerminal" IS 'Indicates whether selecting this outcome typically results in a terminal sales pipeline status.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."RequireComment" IS 'Indicates whether users must provide additional comments when selecting this outcome.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."AllowManualSelection" IS 'Indicates whether users may manually select this outcome.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."DisplayOrder" IS 'Controls the order in which outcomes are displayed.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."IsSystem" IS 'Indicates whether the outcome was seeded by the system. System records should have immutable business codes.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."IsActive" IS 'Indicates whether the outcome is available for use.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."CreatedOn" IS 'Date and time the record was created.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."CreatedBy" IS 'User who created the record.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."UpdatedOn" IS 'Date and time the record was last updated.';
COMMENT ON COLUMN setup."FgsSalesActivityOutcome"."UpdatedBy" IS 'User who last updated the record.';

CREATE INDEX "IX_FgsSalesActivityOutcome_NextSalesPipelineStatusId" ON setup."FgsSalesActivityOutcome" ("NextSalesPipelineStatusId");

CREATE INDEX "IX_FgsSalesActivityOutcome_TenantId_CompanyId" ON setup."FgsSalesActivityOutcome" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSalesActivityOutcome_TenantId_CompanyId_DisplayOrder" ON setup."FgsSalesActivityOutcome" ("TenantId", "CompanyId", "DisplayOrder");

CREATE INDEX "IX_FgsSalesActivityOutcome_TenantId_CompanyId_IsActive" ON setup."FgsSalesActivityOutcome" ("TenantId", "CompanyId", "IsActive");

CREATE INDEX "IX_FgsSalesActivityOutcome_TenantId_CompanyId_NextStatusId" ON setup."FgsSalesActivityOutcome" ("TenantId", "CompanyId", "NextSalesPipelineStatusId");

CREATE UNIQUE INDEX "UX_FgsSalesActivityOutcome_TenantId_CompanyId_OutcomeCode" ON setup."FgsSalesActivityOutcome" ("TenantId", "CompanyId", "OutcomeCode");

CREATE UNIQUE INDEX "UX_FgsSalesActivityOutcome_TenantId_CompanyId_OutcomeName" ON setup."FgsSalesActivityOutcome" ("TenantId", "CompanyId", "OutcomeName");

CREATE INDEX "IX_FgsSalesActivityType_TenantId_CompanyId" ON setup."FgsSalesActivityType" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSalesActivityType_TenantId_CompanyId_DisplayOrder" ON setup."FgsSalesActivityType" ("TenantId", "CompanyId", "DisplayOrder");

CREATE INDEX "IX_FgsSalesActivityType_TenantId_CompanyId_IsActive" ON setup."FgsSalesActivityType" ("TenantId", "CompanyId", "IsActive");

CREATE UNIQUE INDEX "UX_FgsSalesActivityType_TenantId_CompanyId_ActivityTypeCode" ON setup."FgsSalesActivityType" ("TenantId", "CompanyId", "ActivityTypeCode");

CREATE UNIQUE INDEX "UX_FgsSalesActivityType_TenantId_CompanyId_ActivityTypeName" ON setup."FgsSalesActivityType" ("TenantId", "CompanyId", "ActivityTypeName");

CREATE INDEX "IX_FgsSalesDispositionReason_TenantId_CompanyId" ON setup."FgsSalesDispositionReason" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSalesDispositionReason_TenantId_CompanyId_DisplayOrder" ON setup."FgsSalesDispositionReason" ("TenantId", "CompanyId", "DisplayOrder");

CREATE INDEX "IX_FgsSalesDispositionReason_TenantId_CompanyId_IsActive" ON setup."FgsSalesDispositionReason" ("TenantId", "CompanyId", "IsActive");

CREATE UNIQUE INDEX "UX_FgsSalesDispReason_TenantId_CompanyId_ReasonCode" ON setup."FgsSalesDispositionReason" ("TenantId", "CompanyId", "DispositionReasonCode");

CREATE UNIQUE INDEX "UX_FgsSalesDispReason_TenantId_CompanyId_ReasonName" ON setup."FgsSalesDispositionReason" ("TenantId", "CompanyId", "DispositionReasonName");

CREATE INDEX "IX_FgsSalesPipelineStatus_TenantId_CompanyId" ON setup."FgsSalesPipelineStatus" ("TenantId", "CompanyId");

CREATE INDEX "IX_FgsSalesPipelineStatus_TenantId_CompanyId_DisplayOrder" ON setup."FgsSalesPipelineStatus" ("TenantId", "CompanyId", "DisplayOrder");

CREATE INDEX "IX_FgsSalesPipelineStatus_TenantId_CompanyId_IsActive" ON setup."FgsSalesPipelineStatus" ("TenantId", "CompanyId", "IsActive");

CREATE UNIQUE INDEX "UX_FgsSalesPipelineStatus_TenantId_CompanyId_StatusCode" ON setup."FgsSalesPipelineStatus" ("TenantId", "CompanyId", "StatusCode");

CREATE UNIQUE INDEX "UX_FgsSalesPipelineStatus_TenantId_CompanyId_StatusName" ON setup."FgsSalesPipelineStatus" ("TenantId", "CompanyId", "StatusName");

CREATE INDEX "IX_GloSalesActivityOutcome_DisplayOrder" ON glo."GloSalesActivityOutcome" ("DisplayOrder");

CREATE UNIQUE INDEX "UX_GloSalesActivityOutcome_OutcomeCode" ON glo."GloSalesActivityOutcome" ("OutcomeCode");

CREATE UNIQUE INDEX "UX_GloSalesActivityOutcome_OutcomeName" ON glo."GloSalesActivityOutcome" ("OutcomeName");

CREATE INDEX "IX_GloSalesActivityType_DisplayOrder" ON glo."GloSalesActivityType" ("DisplayOrder");

CREATE UNIQUE INDEX "UX_GloSalesActivityType_ActivityTypeCode" ON glo."GloSalesActivityType" ("ActivityTypeCode");

CREATE UNIQUE INDEX "UX_GloSalesActivityType_ActivityTypeName" ON glo."GloSalesActivityType" ("ActivityTypeName");

CREATE INDEX "IX_GloSalesDispositionReason_DisplayOrder" ON glo."GloSalesDispositionReason" ("DisplayOrder");

CREATE UNIQUE INDEX "UX_GloSalesDispositionReason_DispositionReasonCode" ON glo."GloSalesDispositionReason" ("DispositionReasonCode");

CREATE UNIQUE INDEX "UX_GloSalesDispositionReason_DispositionReasonName" ON glo."GloSalesDispositionReason" ("DispositionReasonName");

CREATE INDEX "IX_GloSalesPipelineStatus_DisplayOrder" ON glo."GloSalesPipelineStatus" ("DisplayOrder");

CREATE UNIQUE INDEX "UX_GloSalesPipelineStatus_StatusCode" ON glo."GloSalesPipelineStatus" ("StatusCode");

CREATE UNIQUE INDEX "UX_GloSalesPipelineStatus_StatusName" ON glo."GloSalesPipelineStatus" ("StatusName");

INSERT INTO setup."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260611130512_AddSalesPipelineEntities', '10.0.8');

COMMIT;

