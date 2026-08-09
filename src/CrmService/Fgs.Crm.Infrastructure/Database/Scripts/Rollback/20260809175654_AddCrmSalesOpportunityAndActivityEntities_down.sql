START TRANSACTION;

---------------------------------------------------------------------------
-- Drop dependent sales activity and opportunity tables
---------------------------------------------------------------------------

DROP TABLE IF EXISTS crm."FgsSalesActivity";

DROP TABLE IF EXISTS crm."FgsOpportunity";

DROP TYPE IF EXISTS crm."SalesPriority";

---------------------------------------------------------------------------
-- Reverse crm.CrmLead reshape
---------------------------------------------------------------------------

DROP INDEX IF EXISTS crm."IX_CrmLead_TenantId_CompanyId_ConvertedOpportunityId";
DROP INDEX IF EXISTS crm."IX_CrmLead_TenantId_CompanyId_ServiceLocationId";

ALTER TABLE crm."CrmLead"
    DROP COLUMN IF EXISTS "Address1",
    DROP COLUMN IF EXISTS "Address2",
    DROP COLUMN IF EXISTS "City",
    DROP COLUMN IF EXISTS "State",
    DROP COLUMN IF EXISTS "PostalCode",
    DROP COLUMN IF EXISTS "Country",
    DROP COLUMN IF EXISTS "ServiceLocationId",
    DROP COLUMN IF EXISTS "ConvertedOpportunityId",
    DROP COLUMN IF EXISTS "Name";

ALTER TABLE crm."CrmLead"
    ADD COLUMN IF NOT EXISTS "LeadSummary" character varying(255) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS "FirstName" character varying(100),
    ADD COLUMN IF NOT EXISTS "LastName" character varying(100),
    ADD COLUMN IF NOT EXISTS "CompanyName" character varying(200),
    ADD COLUMN IF NOT EXISTS "CustomerTypeId" bigint,
    ADD COLUMN IF NOT EXISTS "ServiceZipCode" character varying(20),
    ADD COLUMN IF NOT EXISTS "QualifiedOn" timestamp with time zone;

ALTER TABLE crm."CrmLead"
    ALTER COLUMN "LeadSummary" DROP DEFAULT;

COMMENT ON TABLE crm."CrmLead"
IS 'Stores inbound sales inquiries and prospects prior to qualification and conversion into customers and opportunities.';

COMMENT ON COLUMN crm."CrmLead"."LeadStatusId"
IS 'Current lead status selected from setup.FgsLeadStatus.';

COMMENT ON COLUMN crm."CrmLead"."LeadSummary"
IS 'Short summary describing the lead inquiry.';

COMMENT ON COLUMN crm."CrmLead"."LeadDescription"
IS 'Detailed description of the lead inquiry and customer requirements.';

COMMENT ON COLUMN crm."CrmLead"."FirstName"
IS 'Lead contact first name.';

COMMENT ON COLUMN crm."CrmLead"."LastName"
IS 'Lead contact last name.';

COMMENT ON COLUMN crm."CrmLead"."CompanyName"
IS 'Company or organization associated with the lead.';

COMMENT ON COLUMN crm."CrmLead"."CustomerTypeId"
IS 'Customer type associated with the lead.';

COMMENT ON COLUMN crm."CrmLead"."ServiceZipCode"
IS 'ZIP or postal code where service is requested.';

COMMENT ON COLUMN crm."CrmLead"."QualifiedOn"
IS 'Date and time the lead was qualified.';

COMMENT ON COLUMN crm."CrmLead"."CustomerId"
IS 'Customer record created from this lead after conversion.';

COMMENT ON COLUMN crm."CrmLead"."ConvertedOn"
IS 'Date and time the lead was converted into a customer.';

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_CustomerTypeId"
ON crm."CrmLead" ("TenantId", "CompanyId", "CustomerTypeId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_PrimaryContactMethodId"
ON crm."CrmLead" ("TenantId", "CompanyId", "PrimaryContactMethodId");

CREATE INDEX IF NOT EXISTS "IX_CrmLead_TenantId_CompanyId_ServiceZipCode"
ON crm."CrmLead" ("TenantId", "CompanyId", "ServiceZipCode");

DELETE FROM crm."__EFMigrationsHistory"
WHERE "MigrationId" = '20260809175654_AddCrmSalesOpportunityAndActivityEntities';

COMMIT;
