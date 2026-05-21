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

-- GloBusinessType
INSERT INTO dbo."GloBusinessType"
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
        ('HVAC',          'HVAC',           true),
        ('PLUMBING',      'Plumbing',       true),
        ('ELECTRICAL',    'Electrical',     true),
        ('PESTCONTROL',   'Pest Control',   true),
        ('LAWNCARE',      'Lawn Care',      true),
        ('TRASHPICKUP',   'Trash Pickup',   true),
        ('GARAGEDOOR',    'Garage Door',    true),
        ('HOUSECLEANING', 'House Cleaning', true),
        ('PAINTING',      'Painting',       true)
) AS v("Code", "Name", "IsActive")
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

COMMIT;
