START TRANSACTION;

---------------------------------------------------------------------------
-- ENUM: crm.SalesPriority
---------------------------------------------------------------------------

DO $$
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM pg_type t
        JOIN pg_namespace n ON n.oid = t.typnamespace
        WHERE t.typname = 'SalesPriority'
          AND n.nspname = 'crm'
    )
    THEN
        CREATE TYPE crm."SalesPriority" AS ENUM
        (
            'LOW',
            'NORMAL',
            'HIGH'
        );
    END IF;
END
$$;

---------------------------------------------------------------------------
-- ALTER: crm.CrmLead
---------------------------------------------------------------------------

DROP INDEX IF EXISTS crm."IX_CrmLead_TenantId_CompanyId_CustomerTypeId";
DROP INDEX IF EXISTS crm."IX_CrmLead_TenantId_CompanyId_PrimaryContactMethodId";
DROP INDEX IF EXISTS crm."IX_CrmLead_TenantId_CompanyId_ServiceZipCode";

ALTER TABLE crm."CrmLead"
    DROP COLUMN IF EXISTS "CompanyName",
    DROP COLUMN IF EXISTS "CustomerTypeId",
    DROP COLUMN IF EXISTS "FirstName",
    DROP COLUMN IF EXISTS "LastName",
    DROP COLUMN IF EXISTS "LeadSummary",
    DROP COLUMN IF EXISTS "QualifiedOn",
    DROP COLUMN IF EXISTS "ServiceZipCode";

ALTER TABLE crm."CrmLead"
    ADD COLUMN IF NOT EXISTS "Name" character varying(200) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS "Address1" character varying(200),
    ADD COLUMN IF NOT EXISTS "Address2" character varying(200),
    ADD COLUMN IF NOT EXISTS "City" character varying(100),
    ADD COLUMN IF NOT EXISTS "State" character varying(100),
    ADD COLUMN IF NOT EXISTS "PostalCode" character varying(20),
    ADD COLUMN IF NOT EXISTS "Country" character varying(100),
    ADD COLUMN IF NOT EXISTS "ServiceLocationId" bigint,
    ADD COLUMN IF NOT EXISTS "ConvertedOpportunityId" bigint;

ALTER TABLE crm."CrmLead"
    ALTER COLUMN "Name" DROP DEFAULT;

ALTER TABLE crm."CrmLead"
    ALTER COLUMN "LeadReceivedOn" SET DEFAULT now();

COMMENT ON TABLE crm."CrmLead"
IS 'Stores sales leads/prospects received from the website, office users, technicians, referrals, campaigns, or other configured lead sources. A Lead may remain in the Lead pipeline, be disqualified/lost, be associated with an existing customer, or be converted into an Opportunity. Lead activities are stored separately in crm.FgsSalesActivity.';

COMMENT ON COLUMN crm."CrmLead"."LeadStatusId"
IS 'Current status of the lead selected from the configured sales pipeline statuses applicable to leads.';

COMMENT ON COLUMN crm."CrmLead"."LeadSourceId"
IS 'Source that generated the lead selected from setup.FgsLeadSource.';

COMMENT ON COLUMN crm."CrmLead"."CampaignId"
IS 'Marketing campaign associated with the lead.';

COMMENT ON COLUMN crm."CrmLead"."Name"
IS 'Name of the person or contact submitting or associated with the lead.';

COMMENT ON COLUMN crm."CrmLead"."LeadDescription"
IS 'Comments and details describing the customer inquiry, service need, or information provided with the lead.';

COMMENT ON COLUMN crm."CrmLead"."Email"
IS 'Primary email address for the lead.';

COMMENT ON COLUMN crm."CrmLead"."Phone"
IS 'Primary phone number for the lead.';

COMMENT ON COLUMN crm."CrmLead"."PrimaryContactMethodId"
IS 'Preferred or originating contact method for the lead.';

COMMENT ON COLUMN crm."CrmLead"."Address1"
IS 'Primary street address where service is requested.';

COMMENT ON COLUMN crm."CrmLead"."Address2"
IS 'Additional address information such as apartment, suite, unit, building, or floor.';

COMMENT ON COLUMN crm."CrmLead"."City"
IS 'City where service is requested.';

COMMENT ON COLUMN crm."CrmLead"."State"
IS 'State, province, or administrative region where service is requested.';

COMMENT ON COLUMN crm."CrmLead"."PostalCode"
IS 'Postal or ZIP code where service is requested.';

COMMENT ON COLUMN crm."CrmLead"."Country"
IS 'Country where service is requested.';

COMMENT ON COLUMN crm."CrmLead"."AssignedToUserId"
IS 'User assigned to work the lead.';

COMMENT ON COLUMN crm."CrmLead"."CustomerId"
IS 'Existing customer associated with the lead, when applicable.';

COMMENT ON COLUMN crm."CrmLead"."ServiceLocationId"
IS 'Optional service location associated with the lead.';

COMMENT ON COLUMN crm."CrmLead"."LeadReceivedOn"
IS 'Date and time the lead was originally received.';

COMMENT ON COLUMN crm."CrmLead"."DisqualificationReasonId"
IS 'Reason the lead was disqualified selected from setup.FgsLeadDisqualificationReason.';

COMMENT ON COLUMN crm."CrmLead"."DisqualifiedOn"
IS 'Date and time the lead was disqualified.';

COMMENT ON COLUMN crm."CrmLead"."ConvertedOpportunityId"
IS 'Opportunity created when the lead was converted.';

COMMENT ON COLUMN crm."CrmLead"."ConvertedOn"
IS 'Date and time the lead was converted into an opportunity.';

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId"
ON crm."CrmLead" ("TenantId", "CompanyId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_LeadStatusId"
ON crm."CrmLead" ("TenantId", "CompanyId", "LeadStatusId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_LeadSourceId"
ON crm."CrmLead" ("TenantId", "CompanyId", "LeadSourceId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_CampaignId"
ON crm."CrmLead" ("TenantId", "CompanyId", "CampaignId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_AssignedToUserId"
ON crm."CrmLead" ("TenantId", "CompanyId", "AssignedToUserId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_CustomerId"
ON crm."CrmLead" ("TenantId", "CompanyId", "CustomerId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_ServiceLocationId"
ON crm."CrmLead" ("TenantId", "CompanyId", "ServiceLocationId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId"
ON crm."CrmLead" ("TenantId", "CompanyId", "DisqualificationReasonId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_LeadReceivedOn"
ON crm."CrmLead" ("TenantId", "CompanyId", "LeadReceivedOn");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_ConvertedOpportunityId"
ON crm."CrmLead" ("TenantId", "CompanyId", "ConvertedOpportunityId");

---------------------------------------------------------------------------
-- TABLE: crm.FgsOpportunity
---------------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS crm."FgsOpportunity"
(
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint NOT NULL
        GENERATED ALWAYS AS IDENTITY
        (
            INCREMENT 1
            START 1
            MINVALUE 1
            MAXVALUE 9223372036854775807
            CACHE 1
        ),
    "LeadId" bigint NULL,
    "OpportunityStatusId" bigint NOT NULL,
    "LeadSourceId" bigint NULL,
    "CampaignId" bigint NULL,
    "Name" character varying(200) NOT NULL,
    "Description" text NULL,
    "CustomerId" bigint NOT NULL,
    "ServiceLocationId" bigint NULL,
    "AssignedToUserId" bigint NULL,
    "EstimatedAmount" numeric(18,2) NULL,
    "SoldAmount" numeric(18,2) NULL,
    "ExpectedCloseOn" timestamptz NULL,
    "WonOn" timestamptz NULL,
    "LostOn" timestamptz NULL,
    "DispositionReasonId" bigint NULL,
    "EstimateId" bigint NULL,
    "WorkOrderId" bigint NULL,
    "CreatedOn" timestamptz NOT NULL DEFAULT now(),
    "CreatedBy" character varying(100) NULL,
    "UpdatedOn" timestamptz NULL,
    "UpdatedBy" character varying(100) NULL,

    CONSTRAINT "PK_FgsOpportunity"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsOpportunity_FgsTenantCompanyCache_TenantId_CompanyId"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId")
        ON UPDATE NO ACTION
        ON DELETE RESTRICT
);

ALTER TABLE IF EXISTS crm."FgsOpportunity"
    OWNER TO dbmasteruser;

COMMENT ON TABLE crm."FgsOpportunity"
IS 'Stores qualified sales opportunities that originate from Leads or are created directly by users. An Opportunity represents an active sales pursuit and may ultimately result in an Estimate or Work Order.';

COMMENT ON COLUMN crm."FgsOpportunity"."TenantId"
IS 'Tenant that owns the opportunity.';

COMMENT ON COLUMN crm."FgsOpportunity"."CompanyId"
IS 'Company within the tenant that owns the opportunity.';

COMMENT ON COLUMN crm."FgsOpportunity"."Id"
IS 'Unique identifier for the opportunity.';

COMMENT ON COLUMN crm."FgsOpportunity"."LeadId"
IS 'Optional Lead from which the opportunity was created. NULL when the opportunity was created directly without a Lead.';

COMMENT ON COLUMN crm."FgsOpportunity"."OpportunityStatusId"
IS 'Current status of the opportunity selected from the configured sales pipeline statuses applicable to opportunities.';

COMMENT ON COLUMN crm."FgsOpportunity"."LeadSourceId"
IS 'Optional source associated with the opportunity. When the opportunity originated from a Lead, this may be copied from the Lead source.';

COMMENT ON COLUMN crm."FgsOpportunity"."CampaignId"
IS 'Optional marketing campaign associated with the opportunity.';

COMMENT ON COLUMN crm."FgsOpportunity"."Name"
IS 'Name used to identify the sales opportunity.';

COMMENT ON COLUMN crm."FgsOpportunity"."Description"
IS 'Detailed description of the opportunity, customer requirements, sales information, or other relevant comments.';

COMMENT ON COLUMN crm."FgsOpportunity"."CustomerId"
IS 'Customer associated with the opportunity. A customer is required for an active opportunity. The customer may be an existing customer or one created during Lead conversion.';

COMMENT ON COLUMN crm."FgsOpportunity"."ServiceLocationId"
IS 'Optional service location associated with the opportunity.';

COMMENT ON COLUMN crm."FgsOpportunity"."AssignedToUserId"
IS 'Salesperson or user currently responsible for working the opportunity.';

COMMENT ON COLUMN crm."FgsOpportunity"."EstimatedAmount"
IS 'Current estimated sales value of the opportunity used for sales forecasting. This value may change as the opportunity progresses.';

COMMENT ON COLUMN crm."FgsOpportunity"."SoldAmount"
IS 'Actual sales amount agreed upon when the opportunity is won. NULL until the opportunity is marked as won.';

COMMENT ON COLUMN crm."FgsOpportunity"."ExpectedCloseOn"
IS 'Expected date and time when the opportunity is anticipated to close.';

COMMENT ON COLUMN crm."FgsOpportunity"."WonOn"
IS 'Date and time when the opportunity was marked as won.';

COMMENT ON COLUMN crm."FgsOpportunity"."LostOn"
IS 'Date and time when the opportunity was marked as lost.';

COMMENT ON COLUMN crm."FgsOpportunity"."DispositionReasonId"
IS 'Reason the opportunity was lost, selected from the configured sales disposition reasons.';

COMMENT ON COLUMN crm."FgsOpportunity"."EstimateId"
IS 'Estimate created from the opportunity when the sales process results in an Estimate.';

COMMENT ON COLUMN crm."FgsOpportunity"."WorkOrderId"
IS 'Work Order created from the opportunity when the sales process results directly in a Work Order.';

COMMENT ON COLUMN crm."FgsOpportunity"."CreatedOn"
IS 'Date and time when the opportunity record was created.';

COMMENT ON COLUMN crm."FgsOpportunity"."CreatedBy"
IS 'User or process that created the opportunity record.';

COMMENT ON COLUMN crm."FgsOpportunity"."UpdatedOn"
IS 'Date and time when the opportunity record was last updated.';

COMMENT ON COLUMN crm."FgsOpportunity"."UpdatedBy"
IS 'User or process that last updated the opportunity record.';

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_LeadId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "LeadId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_OpportunityStatusId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "OpportunityStatusId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_LeadSourceId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "LeadSourceId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_CampaignId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "CampaignId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_CustomerId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "CustomerId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_ServiceLocationId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "ServiceLocationId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_AssignedToUserId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "AssignedToUserId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_DispositionReasonId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "DispositionReasonId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_ExpectedCloseOn"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "ExpectedCloseOn");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_EstimateId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "EstimateId");

CREATE INDEX IF NOT EXISTS "IX_FgsOpportunity_TenantId_CompanyId_WorkOrderId"
ON crm."FgsOpportunity" ("TenantId", "CompanyId", "WorkOrderId");

---------------------------------------------------------------------------
-- TABLE: crm.FgsSalesActivity
---------------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS crm."FgsSalesActivity"
(
    "TenantId" bigint NOT NULL,
    "CompanyId" bigint NOT NULL,
    "Id" bigint NOT NULL
        GENERATED ALWAYS AS IDENTITY
        (
            INCREMENT 1
            START 1
            MINVALUE 1
            MAXVALUE 9223372036854775807
            CACHE 1
        ),
    "LeadId" bigint NULL,
    "OpportunityId" bigint NULL,
    "ActivityTypeId" bigint NOT NULL,
    "AssignedToUserId" bigint NULL,
    "ScheduledOn" timestamptz NULL,
    "EstimatedHours" numeric(6,2) NULL,
    "StartedOn" timestamptz NULL,
    "CompletedOn" timestamptz NULL,
    "ActualHours" numeric(6,2) NULL,
    "PerformedByUserId" bigint NULL,
    "SalesActivityOutcomeId" bigint NULL,
    "OutcomeDetails" text NULL,
    "Comments" text NULL,
    "RequiresFollowUp" boolean NOT NULL DEFAULT false,
    "FollowUpOn" timestamptz NULL,
    "FollowUpActivityId" bigint NULL,
    "Latitude" numeric(10,7) NULL,
    "Longitude" numeric(10,7) NULL,
    "IsSystemGenerated" boolean NOT NULL DEFAULT false,
    "Priority" crm."SalesPriority" NOT NULL DEFAULT 'NORMAL'::crm."SalesPriority",
    "CreatedOn" timestamptz NOT NULL DEFAULT now(),
    "CreatedBy" character varying(100) NULL,
    "UpdatedOn" timestamptz NULL,
    "UpdatedBy" character varying(100) NULL,

    CONSTRAINT "PK_FgsSalesActivity"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsSalesActivity_FgsTenantCompanyCache_TenantId_CompanyId"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES crm."FgsTenantCompanyCache" ("TenantId", "CompanyId")
        ON UPDATE NO ACTION
        ON DELETE RESTRICT,

    CONSTRAINT "FK_FgsSalesActivity_Lead"
        FOREIGN KEY ("LeadId")
        REFERENCES crm."CrmLead" ("Id")
        ON UPDATE NO ACTION
        ON DELETE CASCADE,

    CONSTRAINT "FK_FgsSalesActivity_Opportunity"
        FOREIGN KEY ("OpportunityId")
        REFERENCES crm."FgsOpportunity" ("Id")
        ON UPDATE NO ACTION
        ON DELETE CASCADE,

    CONSTRAINT "FK_FgsSalesActivity_FollowUpActivity"
        FOREIGN KEY ("FollowUpActivityId")
        REFERENCES crm."FgsSalesActivity" ("Id")
        ON UPDATE NO ACTION
        ON DELETE SET NULL,

    CONSTRAINT "CK_FgsSalesActivity_LeadOrOpportunity"
        CHECK
        (
            ("LeadId" IS NOT NULL AND "OpportunityId" IS NULL)
            OR
            ("LeadId" IS NULL AND "OpportunityId" IS NOT NULL)
        ),

    CONSTRAINT "CK_FgsSalesActivity_CompletedRequiresStarted"
        CHECK
        (
            "CompletedOn" IS NULL
            OR "StartedOn" IS NOT NULL
        ),

    CONSTRAINT "CK_FgsSalesActivity_CompletedAfterStarted"
        CHECK
        (
            "StartedOn" IS NULL
            OR "CompletedOn" IS NULL
            OR "CompletedOn" >= "StartedOn"
        ),

    CONSTRAINT "CK_FgsSalesActivity_EstimatedHours"
        CHECK
        (
            "EstimatedHours" IS NULL
            OR "EstimatedHours" > 0
        ),

    CONSTRAINT "CK_FgsSalesActivity_ActualHours"
        CHECK
        (
            "ActualHours" IS NULL
            OR "ActualHours" > 0
        )
);

ALTER TABLE IF EXISTS crm."FgsSalesActivity"
    OWNER TO dbmasteruser;

COMMENT ON TABLE crm."FgsSalesActivity"
IS 'Stores scheduled and completed sales activities for Leads and Opportunities, including calls, emails, meetings, site visits, follow-ups, and system-generated activities. Activities can be scheduled on the dispatch board and completed with an outcome, resulting pipeline status, comments, and optional follow-up activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."TenantId"
IS 'Tenant that owns the sales activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."CompanyId"
IS 'Company within the tenant that owns the sales activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."Id"
IS 'Unique identifier for the sales activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."LeadId"
IS 'Lead associated with the activity. Exactly one of LeadId or OpportunityId must be populated.';

COMMENT ON COLUMN crm."FgsSalesActivity"."OpportunityId"
IS 'Opportunity associated with the activity. Exactly one of OpportunityId or LeadId must be populated.';

COMMENT ON COLUMN crm."FgsSalesActivity"."ActivityTypeId"
IS 'Activity type selected from the configured sales activity types, such as Call, Email, Visit, Meeting, or Follow-up.';

COMMENT ON COLUMN crm."FgsSalesActivity"."AssignedToUserId"
IS 'User responsible for performing the scheduled sales activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."ScheduledOn"
IS 'Date and time when the activity is scheduled to occur. Used to place the activity on the dispatch board.';

COMMENT ON COLUMN crm."FgsSalesActivity"."EstimatedHours"
IS 'Expected amount of time required to perform the scheduled activity, expressed in hours. Used for scheduling and dispatch capacity planning.';

COMMENT ON COLUMN crm."FgsSalesActivity"."StartedOn"
IS 'Optional date and time when the user started performing the activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."CompletedOn"
IS 'Optional date and time when the activity was completed. When StartedOn and CompletedOn are provided, ActualHours may be calculated from the elapsed time.';

COMMENT ON COLUMN crm."FgsSalesActivity"."ActualHours"
IS 'Actual amount of time spent performing the activity, expressed in hours. The value may be calculated from StartedOn and CompletedOn or entered directly by the user when start and completion times are not tracked.';

COMMENT ON COLUMN crm."FgsSalesActivity"."PerformedByUserId"
IS 'User who actually performed or completed the activity. This may differ from the user originally assigned to the activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."SalesActivityOutcomeId"
IS 'Outcome selected when the activity is completed. The outcome may determine the resulting pipeline status, whether another activity should be created, or whether the Lead should be converted to an Opportunity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."OutcomeDetails"
IS 'Additional details describing the selected activity outcome, including specific results, customer response, information communicated, or other details associated with the outcome.';

COMMENT ON COLUMN crm."FgsSalesActivity"."Comments"
IS 'Comments or notes entered while scheduling, performing, or completing the activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."RequiresFollowUp"
IS 'Indicates whether another sales activity is required after this activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."FollowUpOn"
IS 'Date and time requested for the follow-up activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."FollowUpActivityId"
IS 'Activity created as the follow-up to this activity.';

COMMENT ON COLUMN crm."FgsSalesActivity"."Latitude"
IS 'Latitude captured when the activity is performed, when location capture is enabled.';

COMMENT ON COLUMN crm."FgsSalesActivity"."Longitude"
IS 'Longitude captured when the activity is performed, when location capture is enabled.';

COMMENT ON COLUMN crm."FgsSalesActivity"."IsSystemGenerated"
IS 'Indicates whether the activity was created automatically by the system rather than manually by a user.';

COMMENT ON COLUMN crm."FgsSalesActivity"."Priority"
IS 'Priority of the sales activity used to indicate the urgency with which the activity should be performed.';

COMMENT ON COLUMN crm."FgsSalesActivity"."CreatedOn"
IS 'Date and time when the sales activity record was created.';

COMMENT ON COLUMN crm."FgsSalesActivity"."CreatedBy"
IS 'User or process that created the sales activity record.';

COMMENT ON COLUMN crm."FgsSalesActivity"."UpdatedOn"
IS 'Date and time when the sales activity record was last updated.';

COMMENT ON COLUMN crm."FgsSalesActivity"."UpdatedBy"
IS 'User or process that last updated the sales activity record.';

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_LeadId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "LeadId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_OpportunityId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "OpportunityId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_ActivityTypeId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "ActivityTypeId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_AssignedToUserId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "AssignedToUserId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_ScheduledOn"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "ScheduledOn");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_PerformedByUserId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "PerformedByUserId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_SalesActivityOutcomeId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "SalesActivityOutcomeId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_FollowUpOn"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "FollowUpOn");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_FollowUpActivityId"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "FollowUpActivityId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_TenantId_CompanyId_CompletedOn"
ON crm."FgsSalesActivity" ("TenantId", "CompanyId", "CompletedOn");

-- EF also creates single-column FK supporting indexes
CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_LeadId"
ON crm."FgsSalesActivity" ("LeadId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_OpportunityId"
ON crm."FgsSalesActivity" ("OpportunityId");

CREATE INDEX IF NOT EXISTS "IX_FgsSalesActivity_FollowUpActivityId"
ON crm."FgsSalesActivity" ("FollowUpActivityId");

INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260809175654_AddCrmSalesOpportunityAndActivityEntities', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
