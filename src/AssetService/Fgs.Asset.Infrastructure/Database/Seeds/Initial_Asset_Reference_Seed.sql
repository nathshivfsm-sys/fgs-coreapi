-- Initial_Asset_Reference_Seed.sql
-- Idempotent default FgsAssetStatus rows per tenant/company.
-- Run manually after migrations and FgsTenantCompanyCache is populated (via Setup seed).

INSERT INTO asset."FgsAssetStatus"
(
    "TenantId",
    "CompanyId",
    "Code",
    "Name",
    "Description",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    tc."TenantId",
    tc."CompanyId",
    s."Code",
    s."Name",
    s."Description",
    true,
    CURRENT_TIMESTAMP,
    'seed'
FROM asset."FgsTenantCompanyCache" tc
CROSS JOIN (
    VALUES
        ('ACTIVE', 'Active', 'Asset is active and in service.'),
        ('INACTIVE', 'Inactive', 'Asset is temporarily out of service.'),
        ('RETIRED', 'Retired', 'Asset has been retired from service.'),
        ('DECOMMISSIONED', 'Decommissioned', 'Asset has been permanently decommissioned.')
) AS s("Code", "Name", "Description")
WHERE NOT EXISTS (
    SELECT 1
    FROM asset."FgsAssetStatus" existing
    WHERE existing."TenantId" = tc."TenantId"
      AND existing."CompanyId" = tc."CompanyId"
      AND existing."Code" = s."Code"
);
