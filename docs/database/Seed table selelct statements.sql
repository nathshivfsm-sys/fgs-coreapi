-- ============================================================
-- 1) Set your defaults here (run this whole script in pgAdmin)
-- ============================================================
DROP TABLE IF EXISTS seed_params;

CREATE TEMP TABLE seed_params (
    tenant_id  bigint NOT NULL,
    company_id bigint NOT NULL
);

INSERT INTO seed_params (tenant_id, company_id)
VALUES (24, 1);   -- << change tenant / company here

-- ============================================================
-- 2) Cache tables
-- ============================================================
SELECT 'setup.FgsTenantCompanyCache' AS table_name, c.*
FROM setup."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'identity.FgsTenantCompanyCache' AS table_name, c.*
FROM identity."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'identity.FgsRole' AS table_name, r.*
FROM identity."FgsRole" r
CROSS JOIN seed_params p
WHERE r."TenantId" = p.tenant_id AND r."CompanyId" = p.company_id
ORDER BY r."RoleCode";

SELECT 'billing.FgsTenantCompanyCache' AS table_name, c.*
FROM billing."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'crm.FgsTenantCompanyCache' AS table_name, c.*
FROM crm."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'dispatch.FgsTenantCompanyCache' AS table_name, c.*
FROM dispatch."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'inventory.FgsTenantCompanyCache' AS table_name, c.*
FROM inventory."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'notification.FgsTenantCompanyCache' AS table_name, c.*
FROM notification."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'reporting.FgsTenantCompanyCache' AS table_name, c.*
FROM reporting."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'integration.FgsTenantCompanyCache' AS table_name, c.*
FROM integration."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'asset.FgsTenantCompanyCache' AS table_name, c.*
FROM asset."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'svc.FgsTenantCompanyCache' AS table_name, c.*
FROM svc."FgsTenantCompanyCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

SELECT 'asset.FgsServiceLocationCache' AS table_name, c.*
FROM asset."FgsServiceLocationCache" c
CROSS JOIN seed_params p
WHERE c."TenantId" = p.tenant_id AND c."CompanyId" = p.company_id;

-- ============================================================
-- 3) Setup catalogs
-- ============================================================
SELECT 'setup.FgsBillingCategory' AS table_name, t.*
FROM setup."FgsBillingCategory" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

-- FgsJobTypeCategory / FgsJobTypeSubCategory: tenant-managed; no longer seeded from glo.

SELECT 'setup.FgsLeadSource' AS table_name, t.*
FROM setup."FgsLeadSource" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsLeadStatus' AS table_name, t.*
FROM setup."FgsLeadStatus" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsLeadDisqualificationReason' AS table_name, t.*
FROM setup."FgsLeadDisqualificationReason" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSalesPipelineStatus' AS table_name, t.*
FROM setup."FgsSalesPipelineStatus" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSalesDispositionReason' AS table_name, t.*
FROM setup."FgsSalesDispositionReason" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSalesActivityType' AS table_name, t.*
FROM setup."FgsSalesActivityType" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSalesActivityOutcome' AS table_name, t.*
FROM setup."FgsSalesActivityOutcome" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSetupPaymentMethod' AS table_name, t.*
FROM setup."FgsSetupPaymentMethod" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsResolutionCode' AS table_name, t.*
FROM setup."FgsResolutionCode" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSetupLaborRateType' AS table_name, t.*
FROM setup."FgsSetupLaborRateType" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSetupTechSkillLevel' AS table_name, t.*
FROM setup."FgsSetupTechSkillLevel" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsTag' AS table_name, t.*
FROM setup."FgsTag" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSetupTechTrade' AS table_name, t.*
FROM setup."FgsSetupTechTrade" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSetupTitleOfCourtesy' AS table_name, t.*
FROM setup."FgsSetupTitleOfCourtesy" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSetupZone' AS table_name, t.*
FROM setup."FgsSetupZone" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'setup.FgsSetupPaymentTerm' AS table_name, t.*
FROM setup."FgsSetupPaymentTerm" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'inventory.FgsInventoryCategory' AS table_name, t.*
FROM inventory."FgsInventoryCategory" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'inventory.FgsInventorySubCategory' AS table_name, t.*
FROM inventory."FgsInventorySubCategory" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

-- ============================================================
-- 4) CRM catalogs
-- ============================================================
SELECT 'crm.FgsEstimateFlavor' AS table_name, t.*
FROM crm."FgsEstimateFlavor" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;

SELECT 'crm.FgsEstimateStatus' AS table_name, t.*
FROM crm."FgsEstimateStatus" t
CROSS JOIN seed_params p
WHERE t."TenantId" = p.tenant_id AND t."CompanyId" = p.company_id;