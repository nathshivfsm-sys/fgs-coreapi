-- Platform sentinel tenant/company for global credentials (TenantId=0, CompanyNumber=0).
-- Safe to run multiple times.

INSERT INTO tenant."FgsTenant" ("Id", "TenantCode", "Name", "FgsTenantStatusId", "IsActive", "CreatedOn")
OVERRIDING SYSTEM VALUE
SELECT 0, 'platform', 'Platform Global', 3, true, timezone('utc', now())
WHERE NOT EXISTS (SELECT 1 FROM tenant."FgsTenant" WHERE "Id" = 0);

INSERT INTO tenant."FgsTenantCompany"
(
    "TenantId",
    "CompanyGuid",
    "CompanyNumber",
    "BusinessTypeId",
    "Code",
    "Name",
    "IsActive",
    "CreatedOn"
)
SELECT
    0,
    '00000000-0000-0000-0000-000000000000'::uuid,
    0,
    (SELECT "Id" FROM glo."GloBusinessType" ORDER BY "Id" LIMIT 1),
    'platform',
    'Platform Global',
    true,
    timezone('utc', now())
WHERE NOT EXISTS (
    SELECT 1 FROM tenant."FgsTenantCompany"
    WHERE "TenantId" = 0 AND "CompanyNumber" = 0
);
