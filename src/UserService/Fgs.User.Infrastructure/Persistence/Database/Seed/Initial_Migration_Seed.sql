-- =============================================================================
-- Seed: CleanUpTables global reference data (User Service)
-- Run manually after: 20260518163137_Initial_Migration_Up.sql
-- Not part of EF migration / Up / Down scripts.
--
-- Idempotent: each insert skips rows that already exist (matched by natural key).
-- Defaults where applicable:
--   CreatedOn = UTC now
--   CreatedBy = 'System'
-- =============================================================================

START TRANSACTION;

-- GloLocationType (Code = Name)
INSERT INTO dbo."GloLocationType"
(
    "Code",
    "Name",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."Code",
    v."Name",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ('BILLING',  'BILLING',  true),
        ('SHIPPING', 'SHIPPING', true),
        ('PHYSICAL', 'PHYSICAL', true),
        ('SERVICE',  'SERVICE',  true),
        ('MAILING',  'MAILING',  true),
        ('HQ',       'HQ',       true),
        ('REMITTO',  'REMITTO',  true),
        ('JOBSITE',  'JOBSITE',  true)
) AS v("Code", "Name", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloLocationType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloLocationType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloLocationType"), 1),
    true);

-- GloMasterEntityType
INSERT INTO dbo."GloMasterEntityType"
(
    "Code",
    "IsDocumentAllowed",
    "IsActive",
    "SortOrder",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    v."Code",
    v."IsDocumentAllowed",
    v."IsActive",
    v."SortOrder",
    timezone('utc', now()),
    'System'
FROM (
    VALUES
        ('TENANT',          true, true,  1),
        ('COMPANY',         true, true,  2),
        ('SERVICELOCATION', true, true,  3),
        ('BILLTO',          true, true,  4),
        ('VENDOR',          true, true,  5),
        ('SUBCONTRACTOR',   true, true,  6),
        ('LEAD',            true, true,  7),
        ('PROPOSAL',        true, true,  8),
        ('CUSTOMER',        true, true,  9),
        ('WORKORDER',       true, true, 10),
        ('INVOICE',         true, true, 11)
) AS v("Code", "IsDocumentAllowed", "IsActive", "SortOrder")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloMasterEntityType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloMasterEntityType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloMasterEntityType"), 1),
    true);

-- GloLanguage
INSERT INTO dbo."GloLanguage"
(
    "LanguageCode",
    "LanguageName",
    "CultureCode",
    "IsActive"
)
SELECT
    v."LanguageCode",
    v."LanguageName",
    v."CultureCode",
    v."IsActive"
FROM (
    VALUES
        ('EN', 'English', 'en-US', true),
        ('ES', 'Spanish', 'es-US', true),
        ('FR', 'French',  'fr-FR', true)
) AS v("LanguageCode", "LanguageName", "CultureCode", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloLanguage" t
    WHERE t."LanguageCode" = v."LanguageCode"
);

-- GloAccountingIntegrationType
INSERT INTO dbo."GloAccountingIntegrationType"
(
    "Code",
    "Name",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."Code",
    v."Name",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ('NONE',             'No Accounting',     true),
        ('QUICKBOOKSONLINE', 'QuickBooks Online', true),
        ('SAGEINTACCT',      'Sage Intacct',      true)
) AS v("Code", "Name", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloAccountingIntegrationType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloAccountingIntegrationType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloAccountingIntegrationType"), 1),
    true);

-- GloTimeCardOption
INSERT INTO dbo."GloTimeCardOption"
(
    "Code",
    "Name"
)
SELECT
    v."Code",
    v."Name"
FROM (
    VALUES
        ('NONE',                   'No formal technician time tracking workflow'),
        ('DISPATCHARRIVECOMPLETE', 'Tracks dispatch, arrival, and completion timestamps'),
        ('CHECKINCHECKOUT',        'Technician manually checks in and checks out')
) AS v("Code", "Name")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloTimeCardOption" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloTimeCardOption"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloTimeCardOption"), 1),
    true);

-- GloBusinessType (explicit Id: sequential 1..n; OTHER last)
INSERT INTO dbo."GloBusinessType"
(
    "Id",
    "Code",
    "Name",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."Id",
    v."Code",
    v."Name",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ( 1, 'HVAC',            'HVAC',             true),
        ( 2, 'PLUMBING',        'Plumbing',         true),
        ( 3, 'ELECTRICAL',      'Electrical',       true),
        ( 4, 'PESTCONTROL',     'Pest Control',     true),
        ( 5, 'LAWNCARE',        'Lawn Care',        true),
        ( 6, 'TRASHPICKUP',     'Trash Pickup',     true),
        ( 7, 'GARAGEDOOR',      'Garage Door',      true),
        ( 8, 'HOUSECLEANING',   'House Cleaning',   true),
        ( 9, 'PAINTING',        'Painting',         true),
        (10, 'CARPETCLEANING',  'Carpet Cleaning',  true),
        (11, 'WINDOWCLEANING',  'Window Cleaning',  true),
        (12, 'HOLIDAYLIGHTING', 'Holiday Lighting', true),
        (13, 'OTHER',           'Other',            true)
) AS v("Id", "Code", "Name", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloBusinessType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloBusinessType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloBusinessType"), 1),
    true);

-- GloBillingCategory (no CreatedOn/CreatedBy columns)
INSERT INTO dbo."GloBillingCategory"
(
    "BillingCategoryType",
    "BillingCategoryName"
)
SELECT
    v."BillingCategoryType",
    v."BillingCategoryName"
FROM (
    VALUES
        ('EQ', 'Equipment'),
        ('MT', 'Material'),
        ('LB', 'Labor'),
        ('SB', 'Sub Contractor'),
        ('SF', 'Service Fee'),
        ('SH', 'Shipping'),
        ('TX', 'Tax'),
        ('DS', 'Discount'),
        ('OT', 'Other')
) AS v("BillingCategoryType", "BillingCategoryName")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloBillingCategory" t
    WHERE t."BillingCategoryType" = v."BillingCategoryType"
);

-- GloCountry (no CreatedOn/CreatedBy columns)
INSERT INTO dbo."GloCountry"
(
    "CountryCode",
    "CountryName",
    "CurrencyCode",
    "IsActive"
)
SELECT
    v."CountryCode",
    v."CountryName",
    v."CurrencyCode",
    v."IsActive"
FROM (
    VALUES
        ('US', 'United States', 'USD', true),
        ('CA', 'Canada', 'CAD', true)
) AS v("CountryCode", "CountryName", "CurrencyCode", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloCountry" t
    WHERE t."CountryCode" = v."CountryCode"
);

-- GloCredentialCategory
INSERT INTO dbo."GloCredentialCategory"
(
    "Code",
    "Name",
    "IsActive",
    "CreatedOn",
    "UpdatedOn"
)
SELECT
    v."Code",
    v."Name",
    v."IsActive",
    timezone('utc', now()),
    NULL::timestamptz
FROM (
    VALUES
        ('API_KEY', 'API Key', true),
        ('OAUTH', 'OAuth Credentials', true),
        ('DATABASE', 'Database Credentials', true),
        ('SMTP', 'SMTP Email Credentials', true),
        ('AWS', 'AWS Access Credentials', true),
        ('AZURE', 'Azure Access Credentials', true),
        ('PAYMENT_GATEWAY', 'Payment Gateway Credentials', true),
        ('TWILIO', 'Twilio Credentials', true),
        ('STRIPE', 'Stripe Credentials', true),
        ('QUICKBOOKS', 'QuickBooks Credentials', true),
        ('SERVICE_ACCOUNT', 'Service Account Credentials', true),
        ('SSH', 'SSH Credentials', true),
        ('ENCRYPTION', 'Encryption Keys', true),
        ('WEBHOOK', 'Webhook Secret', true)
) AS v("Code", "Name", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloCredentialCategory" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloCredentialCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloCredentialCategory"), 1),
    true);

-- GloCredentialProviderType
INSERT INTO dbo."GloCredentialProviderType"
(
    "Code",
    "Name",
    "IsActive",
    "CreatedOn",
    "UpdatedOn"
)
SELECT
    v."Code",
    v."Name",
    v."IsActive",
    timezone('utc', now()),
    NULL::timestamptz
FROM (
    VALUES
        ('AWS', 'Amazon Web Services', true),
        ('AZURE', 'Microsoft Azure', true),
        ('TWILIO', 'Twilio', true),
        ('STRIPE', 'Stripe', true),
        ('PAYPAL', 'PayPal', true),
        ('QUICKBOOKS', 'QuickBooks', true),
        ('SHOPIFY', 'Shopify', true),
        ('HUBSPOT', 'HubSpot', true),
        ('MAILCHIMP', 'Mailchimp', true),
        ('SENDGRID', 'SendGrid', true),
        ('GOOGLE', 'Google Services', true),
        ('MICROSOFT', 'Microsoft Services', true),
        ('META', 'Meta / Facebook', true),
        ('DOCUSIGN', 'DocuSign', true),
        ('CUSTOM', 'Custom Provider', true),
        ('OTHER', 'Other Provider', true)
) AS v("Code", "Name", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloCredentialProviderType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloCredentialProviderType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloCredentialProviderType"), 1),
    true);

-- GloStateProvince (requires GloCountry; no CreatedOn/CreatedBy columns)
INSERT INTO dbo."GloStateProvince"
(
    "CountryCode",
    "StateProvinceCode",
    "StateProvinceName",
    "IsActive"
)
SELECT
    v."CountryCode",
    v."StateProvinceCode",
    v."StateProvinceName",
    v."IsActive"
FROM (
    VALUES
        -- United States
        ('US', 'AL', 'Alabama', true),
        ('US', 'AK', 'Alaska', true),
        ('US', 'AZ', 'Arizona', true),
        ('US', 'AR', 'Arkansas', true),
        ('US', 'CA', 'California', true),
        ('US', 'CO', 'Colorado', true),
        ('US', 'CT', 'Connecticut', true),
        ('US', 'DE', 'Delaware', true),
        ('US', 'FL', 'Florida', true),
        ('US', 'GA', 'Georgia', true),
        ('US', 'HI', 'Hawaii', true),
        ('US', 'ID', 'Idaho', true),
        ('US', 'IL', 'Illinois', true),
        ('US', 'IN', 'Indiana', true),
        ('US', 'IA', 'Iowa', true),
        ('US', 'KS', 'Kansas', true),
        ('US', 'KY', 'Kentucky', true),
        ('US', 'LA', 'Louisiana', true),
        ('US', 'ME', 'Maine', true),
        ('US', 'MD', 'Maryland', true),
        ('US', 'MA', 'Massachusetts', true),
        ('US', 'MI', 'Michigan', true),
        ('US', 'MN', 'Minnesota', true),
        ('US', 'MS', 'Mississippi', true),
        ('US', 'MO', 'Missouri', true),
        ('US', 'MT', 'Montana', true),
        ('US', 'NE', 'Nebraska', true),
        ('US', 'NV', 'Nevada', true),
        ('US', 'NH', 'New Hampshire', true),
        ('US', 'NJ', 'New Jersey', true),
        ('US', 'NM', 'New Mexico', true),
        ('US', 'NY', 'New York', true),
        ('US', 'NC', 'North Carolina', true),
        ('US', 'ND', 'North Dakota', true),
        ('US', 'OH', 'Ohio', true),
        ('US', 'OK', 'Oklahoma', true),
        ('US', 'OR', 'Oregon', true),
        ('US', 'PA', 'Pennsylvania', true),
        ('US', 'RI', 'Rhode Island', true),
        ('US', 'SC', 'South Carolina', true),
        ('US', 'SD', 'South Dakota', true),
        ('US', 'TN', 'Tennessee', true),
        ('US', 'TX', 'Texas', true),
        ('US', 'UT', 'Utah', true),
        ('US', 'VT', 'Vermont', true),
        ('US', 'VA', 'Virginia', true),
        ('US', 'WA', 'Washington', true),
        ('US', 'WV', 'West Virginia', true),
        ('US', 'WI', 'Wisconsin', true),
        ('US', 'WY', 'Wyoming', true),
        ('US', 'DC', 'District of Columbia', true),
        -- Canada
        ('CA', 'AB', 'Alberta', true),
        ('CA', 'BC', 'British Columbia', true),
        ('CA', 'MB', 'Manitoba', true),
        ('CA', 'NB', 'New Brunswick', true),
        ('CA', 'NL', 'Newfoundland and Labrador', true),
        ('CA', 'NS', 'Nova Scotia', true),
        ('CA', 'ON', 'Ontario', true),
        ('CA', 'PE', 'Prince Edward Island', true),
        ('CA', 'QC', 'Quebec', true),
        ('CA', 'SK', 'Saskatchewan', true),
        ('CA', 'NT', 'Northwest Territories', true),
        ('CA', 'NU', 'Nunavut', true),
        ('CA', 'YT', 'Yukon', true)
) AS v("CountryCode", "StateProvinceCode", "StateProvinceName", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloStateProvince" t
    WHERE t."CountryCode" = v."CountryCode"
      AND t."StateProvinceCode" = v."StateProvinceCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloStateProvince"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloStateProvince"), 1),
    true);

-- GloPaymentMethodType (no CreatedOn/CreatedBy columns)
INSERT INTO dbo."GloPaymentMethodType"
(
    "Code",
    "DisplayName",
    "SortOrder",
    "IsActive"
)
SELECT
    v."Code",
    v."DisplayName",
    v."SortOrder",
    v."IsActive"
FROM (
    VALUES
        ('CASH', 'Cash', 1, true),
        ('CHECK', 'Check', 2, true),
        ('CREDIT_CARD', 'Credit Card', 3, true),
        ('DEBIT_CARD', 'Debit Card', 4, true),
        ('ACH', 'ACH Transfer', 5, true),
        ('APPLE_PAY', 'Apple Pay', 6, true),
        ('GOOGLE_PAY', 'Google Pay', 7, true),
        ('ZELLE', 'Zelle', 8, true)
) AS v("Code", "DisplayName", "SortOrder", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloPaymentMethodType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloPaymentMethodType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloPaymentMethodType"), 1),
    true);

-- GloResolutionType
INSERT INTO dbo."GloResolutionType"
(
    "Id",
    "ResolutionTypeCode",
    "ResolutionTypeName",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    v."Id",
    v."ResolutionTypeCode",
    v."ResolutionTypeName",
    v."IsActive",
    timezone('utc', now()),
    'System'
FROM (
    VALUES
        (1, 'COMPLETED',      'Completed Successfully', true),
        (2, 'INCOMPLETE',     'Incomplete Work',        true),
        (3, 'PART_REQUIRED',  'Parts Required',         true),
        (4, 'PARTS_ARRIVED',  'Parts Arrived',          true),
        (5, 'CANCELLED',      'Cancelled',              true)
) AS v("Id", "ResolutionTypeCode", "ResolutionTypeName", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloResolutionType" t
    WHERE t."ResolutionTypeCode" = v."ResolutionTypeCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloResolutionType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloResolutionType"), 1),
    true);

-- GloRole (global system roles)
INSERT INTO dbo."GloRole"
(
    "RoleCode",
    "Name",
    "Description",
    "RoleLevel",
    "IsAssignable",
    "IsSystemRole",
    "SortOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."RoleCode",
    v."Name",
    v."Description",
    v."RoleLevel",
    v."IsAssignable",
    v."IsSystemRole",
    v."SortOrder",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        -- Internal FGS system roles
        ('SYSTEM_ADMIN',              'System Administrator',        'Full internal platform administration access.',              'SYSTEM',  false, true,  1::smallint, true),
        ('IMPLEMENTATION_SPECIALIST', 'Implementation Specialist',   'Helps onboard and configure customer tenants.',              'SYSTEM',  false, true,  2::smallint, true),
        ('SUPPORT_AGENT',             'Support Agent',               'Provides customer support and troubleshooting.',               'SYSTEM',  false, true,  3::smallint, true),
        ('BILLING_ADMIN',             'Billing Administrator',       'Manages subscriptions, invoices, and customer payments.',      'SYSTEM',  false, true,  4::smallint, true),
        ('SALES_ADMIN',               'Sales Administrator',         'Creates demo tenants and supports sales operations.',          'SYSTEM',  false, true,  5::smallint, true),
        ('READONLY_AUDITOR',          'Readonly Auditor',            'Internal audit and compliance access.',                        'SYSTEM',  false, true,  6::smallint, true),
        -- Tenant roles
        ('TENANT_ADMIN',              'Tenant Administrator',      'Super administrator for all companies under the tenant.',      'TENANT',  false, false, 10::smallint, true),
        -- Company roles
        ('COMPANY_ADMIN',             'Company Administrator',       'Full administrator for a single company.',                     'COMPANY', true,  false, 20::smallint, true),
        ('OPERATIONS_MANAGER',        'Operations Manager',          'Manages overall company operations.',                          'COMPANY', true,  false, 21::smallint, true),
        ('DISPATCHER',                'Dispatcher',                  'Schedules and dispatches service work.',                       'COMPANY', true,  false, 22::smallint, true),
        ('BILLING',                   'Billing Specialist',          'Handles invoicing and billing operations.',                    'COMPANY', true,  false, 23::smallint, true),
        ('CSR',                       'Customer Service Representative', 'Handles customer communication and service requests.',     'COMPANY', true,  false, 24::smallint, true),
        ('OFFICE_USER',               'Office User',                 'Standard office employee with limited access.',                'COMPANY', true,  false, 25::smallint, true),
        -- Field roles
        ('SERVICE_MANAGER',           'Service Manager',             'Manages all field operations and technicians.',                'FIELD',   true,  false, 30::smallint, true),
        ('FIELD_SUPERVISOR',          'Field Supervisor',            'Supervises assigned field technicians and teams.',             'FIELD',   true,  false, 31::smallint, true),
        ('FIELD_TECH',                'Field Technician',            'Performs field service work and job completion.',              'FIELD',   true,  false, 32::smallint, true)
) AS v("RoleCode", "Name", "Description", "RoleLevel", "IsAssignable", "IsSystemRole", "SortOrder", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloRole" t
    WHERE t."RoleCode" = v."RoleCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloRole"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloRole"), 1),
    true);

-- GloSetupDescriptionType
INSERT INTO dbo."GloSetupDescriptionType"
(
    "Id",
    "Code",
    "Name",
    "Description",
    "IsActive",
    "CreatedOn"
)
SELECT
    gen_random_uuid(),
    v."Code",
    v."Name",
    v."Description",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('ReasonForCall',        'Reason For Call',        'Predefined reason for call descriptions'),
        ('Recommendations',      'Recommendations',        'Predefined recommendation descriptions'),
        ('WorkSummary',          'Work Summary',           'Predefined work summary descriptions'),
        ('AgreementDescription', 'Agreement Description',  'Predefined agreement descriptions')
) AS v("Code", "Name", "Description")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSetupDescriptionType" t
    WHERE t."Code" = v."Code"
);

-- GloSetupLaborRateType
INSERT INTO dbo."GloSetupLaborRateType"
(
    "Name",
    "Description",
    "SortOrder",
    "IsSystem",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    v."Name",
    v."Description",
    v."SortOrder",
    v."IsSystem",
    v."IsActive",
    timezone('utc', now()),
    'System'
FROM (
    VALUES
        ('Regular',     'Standard labor rate',    1, true, true),
        ('Overtime',    'Overtime labor rate',    2, true, true),
        ('Double-Time', 'Double-time labor rate', 3, true, true),
        ('Holiday',     'Holiday labor rate',     4, true, true),
        ('Weekend',     'Weekend labor rate',     5, true, true)
) AS v("Name", "Description", "SortOrder", "IsSystem", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSetupLaborRateType" t
    WHERE t."Name" = v."Name"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloSetupLaborRateType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSetupLaborRateType"), 1),
    true);

-- GloSetupTenantStatus (Id 1 = default FK on FgsTenant)
INSERT INTO dbo."GloSetupTenantStatus" ("Id", "Name", "Description", "IsActive", "CreatedOn")
OVERRIDING SYSTEM VALUE
SELECT v."Id", v."Name", v."Description", v."IsActive", timezone('utc', now())
FROM (
    VALUES
        (1::smallint, 'Pending',    'Tenant registration initiated',     true),
        (2::smallint, 'Provisioning',  'Infrastructure provisioning in progress',   true),
        (3::smallint, 'Active', 'Tenant is active and operational', true),
        (4::smallint, 'ProvisioningFailed', 'Infrastructure provisioning failed', true),
        (5::smallint, 'Suspended', 'Tenant access temporarily suspended', true),
        (6::smallint, 'Cancelled', 'Tenant subscription cancelled',     true)
) AS v("Id", "Name", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1 FROM dbo."GloSetupTenantStatus" t WHERE t."Id" = v."Id"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloSetupTenantStatus"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSetupTenantStatus"), 1),
    true);

-- GloTrade
INSERT INTO dbo."GloTrade"
(
    "BusinessTypeId",
    "TradeCode",
    "TradeName",
    "Description",
    "IsActive",
    "CreatedOn"
)
SELECT
    bt."Id",
    v."TradeCode",
    v."TradeName",
    v."Description",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('PESTCONTROL',   'PESTCONTROL',   'Pest Control',   'General pest control services'),
        ('GARAGEDOOR',    'GARAGEDOOR',    'Garage Door',    'Garage door installation and repair'),
        ('LAWNCARE',      'LAWNCARE',      'Lawn Care',      'General lawn maintenance'),
        ('LAWNCARE',      'IRRIGATION',    'Irrigation',     'Sprinkler and irrigation systems'),
        ('LAWNCARE',      'LANDSCAPING',   'Landscaping',    'Landscape design and maintenance'),
        ('HOUSECLEANING', 'HOUSECLEANING', 'House Cleaning', 'Residential and commercial cleaning'),
        ('TRASHPICKUP',   'TRASHREMOVAL',  'Trash Removal',  'General trash pickup services'),
        ('TRASHPICKUP',   'JUNKREMOVAL',   'Junk Removal',   'Bulk junk and debris removal'),
        ('ELECTRICAL',    'ELECTRICAL',    'Electrical',     'Electrical installation and repair'),
        ('PLUMBING',      'PLUMBING',      'Plumbing',       'Residential and commercial plumbing'),
        ('HVAC',          'HVAC',          'HVAC',           'Heating and air conditioning services'),
        ('PAINTING',      'PAINTING',      'Painting',       'Interior and exterior painting')
) AS v("BusinessTypeCode", "TradeCode", "TradeName", "Description")
INNER JOIN dbo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloTrade" t
    WHERE t."TradeCode" = v."TradeCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloTrade"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloTrade"), 1),
    true);

-- GloSkill (HVAC, Plumbing, Electrical)
INSERT INTO dbo."GloSkill"
(
    "BusinessTypeId",
    "TradeId",
    "SkillCode",
    "SkillName",
    "Description",
    "RequiresCertification",
    "IsActive",
    "CreatedOn"
)
SELECT
    bt."Id",
    tr."Id",
    v."SkillCode",
    v."SkillName",
    v."Description",
    v."RequiresCertification",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('HVAC',       'HVAC',       'HVACEXPERT',       'HVAC Expert',       'Experienced HVAC technician', false),
        ('HVAC',       'HVAC',       'HVACHELPER',       'HVAC Helper',       'HVAC helper and assistant technician', false),
        ('PLUMBING',   'PLUMBING',   'PLUMBINGEXPERT',   'Plumbing Expert',   'Experienced plumbing technician', false),
        ('PLUMBING',   'PLUMBING',   'PLUMBINGHELPER',   'Plumbing Helper',   'Plumbing helper and assistant technician', false),
        ('ELECTRICAL', 'ELECTRICAL', 'ELECTRICALEXPERT', 'Electrical Expert', 'Experienced electrical technician', false),
        ('ELECTRICAL', 'ELECTRICAL', 'ELECTRICALHELPER', 'Electrical Helper', 'Electrical helper and assistant technician', false)
) AS v("BusinessTypeCode", "TradeCode", "SkillCode", "SkillName", "Description", "RequiresCertification")
INNER JOIN dbo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
INNER JOIN dbo."GloTrade" tr ON tr."TradeCode" = v."TradeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSkill" s
    WHERE s."SkillCode" = v."SkillCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloSkill"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSkill"), 1),
    true);

-- GloZone
INSERT INTO dbo."GloZone"
(
    "Code",
    "Name",
    "Description",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."Code",
    v."Name",
    v."Description",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ('ALL', 'All', 'Default zone covering all service areas', true)
) AS v("Code", "Name", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloZone" z
    WHERE z."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloZone"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloZone"), 1),
    true);

-- GloSubCategory
INSERT INTO dbo."GloSubCategory"
(
    "Code",
    "Name",
    "Description",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."Code",
    v."Name",
    v."Description",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ('INSTALL',      'Install',      'Installation service', true),
        ('REPAIR',       'Repair',       'Repair service', true),
        ('SERVICE',      'Service',      'General maintenance service', true),
        ('REPLACE',      'Replace',      'Replacement service', true),
        ('INSPECT',      'Inspect',      'Inspection service', true),
        ('MAINTENANCE',  'Maintenance',  'Preventive maintenance service', true),
        ('TROUBLESHOOT', 'Troubleshoot', 'Diagnostic and troubleshooting service', true),
        ('CLEANING',     'Cleaning',     'Cleaning service', true),
        ('TUNEUP',       'Tune-Up',      'System tune-up service', true),
        ('UPGRADE',      'Upgrade',      'Upgrade existing equipment or system', true)
) AS v("Code", "Name", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSubCategory" sc
    WHERE sc."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloSubCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSubCategory"), 1),
    true);

-- GloCategory
INSERT INTO dbo."GloCategory"
(
    "BusinessTypeId",
    "Code",
    "Name",
    "Description",
    "IsActive",
    "CreatedOn"
)
SELECT
    bt."Id",
    v."Code",
    v."Name",
    v."Description",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('HVAC',       'AC',          'Air Conditioning',     'Air conditioning systems'),
        ('HVAC',       'FURNACE',     'Furnace',              'Heating furnace systems'),
        ('HVAC',       'THERMOSTAT',  'Thermostat',           'Thermostat systems and controls'),
        ('PLUMBING',   'TOILET',      'Toilet',               'Toilet systems'),
        ('PLUMBING',   'FAUCET',      'Faucet',               'Faucet systems'),
        ('PLUMBING',   'WATERHEATER', 'Water Heater',         'Water heater systems'),
        ('ELECTRICAL', 'PANEL',       'Electrical Panel',     'Electrical panel systems'),
        ('ELECTRICAL', 'LIGHTING',    'Lighting',             'Lighting systems'),
        ('ELECTRICAL', 'OUTLET',      'Outlet',               'Electrical outlet systems')
) AS v("BusinessTypeCode", "Code", "Name", "Description")
INNER JOIN dbo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloCategory" c
    WHERE c."BusinessTypeId" = bt."Id"
      AND c."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloCategory"), 1),
    true);

-- GloCategorySubCategory
INSERT INTO dbo."GloCategorySubCategory"
(
    "BusinessTypeId",
    "CategoryId",
    "SubCategoryId",
    "CreatedOn"
)
SELECT
    bt."Id",
    c."Id",
    sc."Id",
    timezone('utc', now())
FROM (
    VALUES
        ('HVAC',       'AC',          'INSTALL'),
        ('HVAC',       'AC',          'REPAIR'),
        ('HVAC',       'AC',          'SERVICE'),
        ('HVAC',       'FURNACE',     'SERVICE'),
        ('PLUMBING',   'TOILET',      'REPAIR'),
        ('PLUMBING',   'TOILET',      'REPLACE'),
        ('PLUMBING',   'FAUCET',      'INSTALL'),
        ('PLUMBING',   'FAUCET',      'REPLACE'),
        ('ELECTRICAL', 'PANEL',       'REPLACE'),
        ('ELECTRICAL', 'LIGHTING',    'INSTALL'),
        ('ELECTRICAL', 'OUTLET',      'REPAIR')
) AS v("BusinessTypeCode", "CategoryCode", "SubCategoryCode")
INNER JOIN dbo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
INNER JOIN dbo."GloCategory" c
    ON c."BusinessTypeId" = bt."Id"
   AND c."Code" = v."CategoryCode"
INNER JOIN dbo."GloSubCategory" sc ON sc."Code" = v."SubCategoryCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloCategorySubCategory" m
    WHERE m."BusinessTypeId" = bt."Id"
      AND m."CategoryId" = c."Id"
      AND m."SubCategoryId" = sc."Id"
);

-- GloLeadSource (global lead acquisition sources for tenant provisioning seed)
INSERT INTO dbo."GloLeadSource"
(
    "SourceCode",
    "SourceName",
    "Description",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    v."SourceCode",
    v."SourceName",
    v."Description",
    v."IsActive",
    timezone('utc', now()),
    'System'
FROM (
    VALUES
        ('REFERRAL',  'Referral',        'Customer or partner referral',           true),
        ('WEBSITE',   'Website',         'Company website inquiry',                true),
        ('GOOGLE',    'Google',          'Google search or ads',                   true),
        ('FACEBOOK',  'Facebook',        'Facebook or social media',               true),
        ('YELP',      'Yelp',            'Yelp or online review platform',         true),
        ('PHONE',     'Phone Call',      'Inbound phone call',                     true),
        ('DIRECT',    'Direct Mail',     'Direct mail campaign',                   true),
        ('OTHER',     'Other',           'Other or unknown lead source',           true)
) AS v("SourceCode", "SourceName", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloLeadSource" ls
    WHERE ls."SourceCode" = v."SourceCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloLeadSource"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloLeadSource"), 1),
    true);

-- GloUnitOfMeasure (global units of measure)
INSERT INTO dbo."GloUnitOfMeasure"
(
    "UnitCode",
    "Name",
    "Abbreviation",
    "Description",
    "UnitType",
    "DecimalPlaces",
    "DisplayOrder",
    "IsSystem",
    "IsActive"
)
SELECT
    v."UnitCode",
    v."Name",
    v."Abbreviation",
    v."Description",
    v."UnitType",
    v."DecimalPlaces",
    v."DisplayOrder",
    v."IsSystem",
    v."IsActive"
FROM (
    VALUES
        ('EACH',  'Each',   'EA',   'Individual item',        'COUNT',   0,  1, true, true),
        ('BOX',   'Box',    'BOX',  'Box quantity',           'PACKAGE', 0,  2, true, true),
        ('CASE',  'Case',   'CS',   'Case quantity',          'PACKAGE', 0,  3, true, true),
        ('FOOT',  'Foot',   'FT',   'Linear feet',            'LENGTH',  2,  4, true, true),
        ('INCH',  'Inch',   'IN',   'Inches',                 'LENGTH',  2,  5, true, true),
        ('POUND', 'Pound',  'LB',   'Weight in pounds',       'WEIGHT',  2,  6, true, true),
        ('GALLON','Gallon', 'GAL',  'Liquid gallon',          'VOLUME',  2,  7, true, true),
        ('HOUR',  'Hour',   'HR',   'Labor hour',             'TIME',    2,  8, true, true),
        ('DAY',   'Day',    'DAY',  'Daily unit',             'TIME',    0,  9, true, true),
        ('ROLL',  'Roll',   'ROLL', 'Roll quantity',          'PACKAGE', 0, 10, true, true)
) AS v("UnitCode", "Name", "Abbreviation", "Description", "UnitType", "DecimalPlaces", "DisplayOrder", "IsSystem", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloUnitOfMeasure" u
    WHERE u."UnitCode" = v."UnitCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloUnitOfMeasure"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloUnitOfMeasure"), 1),
    true);

-- GloTag (global system tags)
INSERT INTO dbo."GloTag"
(
    "TagCode",
    "Name",
    "NormalizedName",
    "Description",
    "BackgroundColor",
    "TextColor",
    "DisplayOrder",
    "IsSystemGenerated",
    "IsActive"
)
SELECT
    v."TagCode",
    v."Name",
    v."NormalizedName",
    v."Description",
    v."BackgroundColor",
    v."TextColor",
    v."DisplayOrder",
    v."IsSystemGenerated",
    v."IsActive"
FROM (
    VALUES
        ('URGENT',     'Urgent',               'urgent',               'Requires immediate attention',     '#EF4444', '#FFFFFF', 1, true, true),
        ('VIP',        'VIP',                  'vip',                  'Important customer',               '#F59E0B', '#000000', 2, true, true),
        ('WARRANTY',   'Warranty',             'warranty',             'Under warranty',                   '#10B981', '#FFFFFF', 3, true, true),
        ('FOLLOWUP',   'Needs Follow-Up',      'needs follow-up',      'Additional follow-up required',    '#EAB308', '#000000', 4, true, true),
        ('COMMERCIAL', 'Commercial',           'commercial',           'Commercial customer or property',  '#3B82F6', '#FFFFFF', 5, true, true),
        ('INSPECTION', 'Inspection Required',  'inspection required',  'Inspection is required',           '#06B6D4', '#FFFFFF', 6, true, true)
) AS v("TagCode", "Name", "NormalizedName", "Description", "BackgroundColor", "TextColor", "DisplayOrder", "IsSystemGenerated", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloTag" t
    WHERE t."TagCode" = v."TagCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloTag"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloTag"), 1),
    true);

-- GloTitleOfCourtesy (global courtesy titles)
INSERT INTO dbo."GloTitleOfCourtesy"
(
    "Code",
    "DisplayName",
    "SortOrder",
    "IsActive"
)
SELECT
    v."Code",
    v."DisplayName",
    v."SortOrder",
    v."IsActive"
FROM (
    VALUES
        ('MR',   'Mr.',   1, true),
        ('MRS',  'Mrs.',  2, true),
        ('MS',   'Ms.',   3, true),
        ('MISS', 'Miss',  4, true),
        ('DR',   'Dr.',   5, true),
        ('PROF', 'Prof.', 6, true),
        ('REV',  'Rev.',  7, true)
) AS v("Code", "DisplayName", "SortOrder", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloTitleOfCourtesy" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloTitleOfCourtesy"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloTitleOfCourtesy"), 1),
    true);

-- =============================================================================
-- GloSeedTableMapping / GloSeedTableColumnMapping
-- Tenant provisioning: global (Glo*) -> tenant/company (Fgs*) catalog copies
-- Idempotent: matched by SeedCode (table) and TargetColumnName (column)
-- =============================================================================

INSERT INTO dbo."GloSeedTableMapping"
(
    "SeedCode",
    "SourceSchemaName",
    "SourceTableName",
    "TargetSchemaName",
    "TargetTableName",
    "SeedOrder",
    "Description",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."SeedCode",
    v."SourceSchemaName",
    v."SourceTableName",
    v."TargetSchemaName",
    v."TargetTableName",
    v."SeedOrder",
    v."Description",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               'dbo', 'GloZone',       'dbo', 'FgsSetupZone',           10, 'Copy global service zones into tenant setup',                    true),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        'dbo', 'GloTrade',      'dbo', 'FgsSetupTechTrade',      20, 'Copy global technician trades into tenant setup',                true),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  'dbo', 'GloSkill',      'dbo', 'FgsSetupTechSkillLevel', 30, 'Copy global technician skill levels into tenant setup',          true),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       'dbo', 'GloLeadSource', 'dbo', 'FgsLeadSource',          40, 'Copy global lead sources into tenant/company lead source catalog', true),
        ('GLO_ROLE_TO_FGS_ROLE',                     'dbo', 'GloRole',       'dbo', 'FgsRole',                50, 'Copy global roles into tenant/company role catalog',             true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', 'dbo', 'GloTitleOfCourtesy', 'dbo', 'FgsSetupTitleOfCourtesy', 60, 'Copy global courtesy titles into tenant setup',                  true),
        ('GLO_TAG_TO_FGS_TAG',                       'dbo', 'GloTag',        'dbo', 'FgsTag',                 70, 'Copy global system tags into tenant/company tag catalog',        true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'dbo', 'GloUnitOfMeasure', 'dbo', 'FgsUnitOfMeasure',  80, 'Copy global units of measure into tenant/company catalog',       true)
) AS v("SeedCode", "SourceSchemaName", "SourceTableName", "TargetSchemaName", "TargetTableName", "SeedOrder", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSeedTableMapping" m
    WHERE m."SeedCode" = v."SeedCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloSeedTableMapping"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSeedTableMapping"), 1),
    true);

INSERT INTO dbo."GloSeedTableColumnMapping"
(
    "SeedTableMappingId",
    "SourceColumnName",
    "TargetColumnName",
    "TransformationType",
    "StaticValue",
    "ColumnOrder",
    "IsRequired",
    "IsActive",
    "CreatedOn"
)
SELECT
    m."Id",
    c."SourceColumnName",
    c."TargetColumnName",
    c."TransformationType",
    c."StaticValue",
    c."ColumnOrder",
    c."IsRequired",
    true,
    timezone('utc', now())
FROM dbo."GloSeedTableMapping" m
INNER JOIN (
    VALUES
        -- GloZone -> FgsSetupZone
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               NULL,           'TenantId',    'TENANT_ID',         NULL,      1, true),
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               NULL,           'CompanyId',   'COMPANY_ID',        NULL,      2, true),
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               'Code',         'Code',        NULL,                NULL,      3, true),
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               'Name',         'Name',        NULL,                NULL,      4, true),
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               'Description',  'Description', NULL,                NULL,      5, false),
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               'IsActive',     'IsActive',    NULL,                NULL,      6, true),
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               NULL,           'CreatedOn',   'CURRENT_TIMESTAMP', NULL,      7, true),
        ('GLO_ZONE_TO_FGS_SETUP_ZONE',               NULL,           'CreatedBy',   'STATIC',            'System',  8, false),

        -- GloTrade -> FgsSetupTechTrade
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        NULL,           'TenantId',    'TENANT_ID',         NULL,      1, true),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        NULL,           'CompanyId',   'COMPANY_ID',        NULL,      2, true),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        'TradeCode',    'TradeCode',   NULL,                NULL,      3, true),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        'TradeName',    'Name',        NULL,                NULL,      4, true),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        'Description',  'Description', NULL,                NULL,      5, false),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        'Id',           'SortOrder',   NULL,                NULL,      6, false),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        'IsActive',     'IsActive',    NULL,                NULL,      7, true),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        NULL,           'CreatedOn',   'CURRENT_TIMESTAMP', NULL,      8, true),
        ('GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',        NULL,           'CreatedBy',   'STATIC',            'System',  9, false),

        -- GloSkill -> FgsSetupTechSkillLevel
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  NULL,           'TenantId',    'TENANT_ID',         NULL,      1, true),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  NULL,           'CompanyId',   'COMPANY_ID',        NULL,      2, true),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  'SkillCode',    'Code',        NULL,                NULL,      3, true),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  'SkillName',    'Name',        NULL,                NULL,      4, true),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  'Description',  'Description', NULL,                NULL,      5, false),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  'Id',           'SortOrder',   NULL,                NULL,      6, false),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  'IsActive',     'IsActive',    NULL,                NULL,      7, true),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  NULL,           'CreatedOn',   'CURRENT_TIMESTAMP', NULL,      8, true),
        ('GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',  NULL,           'CreatedBy',   'STATIC',            'System',  9, false),

        -- GloLeadSource -> FgsLeadSource
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       NULL,           'TenantId',    'TENANT_ID',         NULL,      1, true),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       NULL,           'CompanyId',   'COMPANY_ID',        NULL,      2, true),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       'SourceCode',   'SourceCode',  NULL,                NULL,      3, true),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       'SourceName',   'SourceName',  NULL,                NULL,      4, true),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       'Description',  'Description', NULL,                NULL,      5, false),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       'IsActive',     'IsActive',    NULL,                NULL,      6, true),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       NULL,           'CreatedOn',   'CURRENT_TIMESTAMP', NULL,      7, true),
        ('GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',       NULL,           'CreatedBy',   'STATIC',            'System',  8, false),

        -- GloRole -> FgsRole
        ('GLO_ROLE_TO_FGS_ROLE',                     NULL,           'TenantId',    'TENANT_ID',         NULL,      1, true),
        ('GLO_ROLE_TO_FGS_ROLE',                     NULL,           'CompanyId',   'COMPANY_ID',        NULL,      2, true),
        ('GLO_ROLE_TO_FGS_ROLE',                     'RoleCode',     'RoleCode',    NULL,                NULL,      3, true),
        ('GLO_ROLE_TO_FGS_ROLE',                     'Name',         'Name',        NULL,                NULL,      4, true),
        ('GLO_ROLE_TO_FGS_ROLE',                     'Description',  'Description', NULL,                NULL,      5, false),
        ('GLO_ROLE_TO_FGS_ROLE',                     'Id',           'GloRoleId',   NULL,                NULL,      6, true),
        ('GLO_ROLE_TO_FGS_ROLE',                     'IsActive',     'IsActive',    NULL,                NULL,      7, true),
        ('GLO_ROLE_TO_FGS_ROLE',                     NULL,           'CreatedOn',   'CURRENT_TIMESTAMP', NULL,      8, true),
        ('GLO_ROLE_TO_FGS_ROLE',                     NULL,           'CreatedBy',   'STATIC',            'System',  9, false),

        -- GloTitleOfCourtesy -> FgsSetupTitleOfCourtesy
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', NULL,           'TenantId',    'TENANT_ID',         NULL,      1, true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', NULL,           'CompanyId',   'COMPANY_ID',        NULL,      2, true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', 'Code',         'Code',        NULL,                NULL,      3, true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', 'DisplayName',  'DisplayName', NULL,                NULL,      4, true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', 'SortOrder',    'SortOrder',   NULL,                NULL,      5, true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', 'IsActive',     'IsActive',    NULL,                NULL,      6, true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', NULL,           'CreatedOn',   'CURRENT_TIMESTAMP', NULL,      7, true),
        ('GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY', NULL,           'CreatedBy',   'STATIC',            'System',  8, false),

        -- GloTag -> FgsTag
        ('GLO_TAG_TO_FGS_TAG',                       NULL,           'TenantId',         'TENANT_ID',         NULL,      1, true),
        ('GLO_TAG_TO_FGS_TAG',                       NULL,           'CompanyId',        'COMPANY_ID',        NULL,      2, true),
        ('GLO_TAG_TO_FGS_TAG',                       'TagCode',      'TagCode',          NULL,                NULL,      3, true),
        ('GLO_TAG_TO_FGS_TAG',                       'Name',         'Name',             NULL,                NULL,      4, true),
        ('GLO_TAG_TO_FGS_TAG',                       'NormalizedName', 'NormalizedName', NULL,                NULL,      5, true),
        ('GLO_TAG_TO_FGS_TAG',                       'Description',  'Description',      NULL,                NULL,      6, false),
        ('GLO_TAG_TO_FGS_TAG',                       'BackgroundColor', 'BackgroundColor', NULL,             NULL,      7, false),
        ('GLO_TAG_TO_FGS_TAG',                       'TextColor',    'TextColor',        NULL,                NULL,      8, false),
        ('GLO_TAG_TO_FGS_TAG',                       'IconFileId',   'IconFileId',       NULL,                NULL,      9, false),
        ('GLO_TAG_TO_FGS_TAG',                       'IsSystemGenerated', 'IsSystemGenerated', NULL,         NULL,     10, true),
        ('GLO_TAG_TO_FGS_TAG',                       'IsActive',     'IsActive',         NULL,                NULL,     11, true),
        ('GLO_TAG_TO_FGS_TAG',                       NULL,           'CreatedOn',        'CURRENT_TIMESTAMP', NULL,     12, true),

        -- GloUnitOfMeasure -> FgsUnitOfMeasure
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', NULL,           'TenantId',      'TENANT_ID',         NULL,      1, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', NULL,           'CompanyId',     'COMPANY_ID',        NULL,      2, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'UnitCode',     'UnitCode',      NULL,                NULL,      3, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'Name',         'Name',          NULL,                NULL,      4, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'Abbreviation', 'Abbreviation',  NULL,                NULL,      5, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'Description',  'Description',   NULL,                NULL,      6, false),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'UnitType',     'UnitType',      NULL,                NULL,      7, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'DecimalPlaces', 'DecimalPlaces', NULL,               NULL,      8, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'DisplayOrder', 'DisplayOrder', NULL,               NULL,      9, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'IsSystem',     'IsSystem',      NULL,                NULL,     10, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', 'IsActive',     'IsActive',      NULL,                NULL,     11, true),
        ('GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE', NULL,           'CreatedOn',     'CURRENT_TIMESTAMP', NULL,     12, true)
) AS c("SeedCode", "SourceColumnName", "TargetColumnName", "TransformationType", "StaticValue", "ColumnOrder", "IsRequired")
    ON c."SeedCode" = m."SeedCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSeedTableColumnMapping" existing
    WHERE existing."SeedTableMappingId" = m."Id"
      AND existing."TargetColumnName" = c."TargetColumnName"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloSeedTableColumnMapping"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSeedTableColumnMapping"), 1),
    true);

COMMIT;
