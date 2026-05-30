-- =============================================================================
-- Seed: CleanUpTables global reference data (User Service)
-- Run manually after: 20260518163137_Initial_Migration_Up.sql
-- Not part of EF migration / Up / Down scripts.
--
-- Idempotent: each insert skips rows that already exist (matched by natural key).
-- Defaults where applicable:
--   CreatedOn = UTC now
--   CreatedBy = 'System' (varchar audit columns only; GloSeed* mapping tables use bigint CreatedBy = NULL)
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
    "BillingCategoryName",
    "Description",
    "DisplayOrder",
    "ShowToFieldTech",
    "AllowToPick"
)
SELECT
    v."BillingCategoryType",
    v."BillingCategoryName",
    v."Description",
    v."DisplayOrder",
    v."ShowToFieldTech",
    v."AllowToPick"
FROM (
    VALUES
        ('DS', 'Discount',       'Used for discounts applied to invoices, quotes, or transactions.', 1::smallint, true,  true),
        ('IN', 'Inventory',      'Charges for stocked inventory items.', 2::smallint, true,  true),
        ('LB', 'Labor',          'Labor and technician service charges.', 3::smallint, true,  true),
        ('NI', 'Non-Inventory',  'Charges for non-inventory items or services.', 4::smallint, true,  true),
        ('OT', 'Other',          'Miscellaneous charges not covered by other categories.', 5::smallint, true,  true),
        ('SB', 'Sub Contractor', 'Charges related to subcontractor work or outsourced services.', 6::smallint, true,  true),
        ('SF', 'Service Fee',    'General service-related fees.', 7::smallint, true,  true),
        ('SH', 'Shipping',       'Shipping, freight, and delivery charges.', 8::smallint, true,  true),
        ('TX', 'Tax',            'Sales tax or other applicable tax charges.', 9::smallint, true,  false)
) AS v("BillingCategoryType", "BillingCategoryName", "Description", "DisplayOrder", "ShowToFieldTech", "AllowToPick")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloBillingCategory" t
    WHERE t."BillingCategoryType" = v."BillingCategoryType"
);

UPDATE dbo."GloBillingCategory" AS t
SET
    "BillingCategoryName" = v."BillingCategoryName",
    "Description" = v."Description",
    "DisplayOrder" = v."DisplayOrder",
    "ShowToFieldTech" = v."ShowToFieldTech",
    "AllowToPick" = v."AllowToPick"
FROM (
    VALUES
        ('DS', 'Discount',       'Used for discounts applied to invoices, quotes, or transactions.', 1::smallint, true,  true),
        ('IN', 'Inventory',      'Charges for stocked inventory items.', 2::smallint, true,  true),
        ('LB', 'Labor',          'Labor and technician service charges.', 3::smallint, true,  true),
        ('NI', 'Non-Inventory',  'Charges for non-inventory items or services.', 4::smallint, true,  true),
        ('OT', 'Other',          'Miscellaneous charges not covered by other categories.', 5::smallint, true,  true),
        ('SB', 'Sub Contractor', 'Charges related to subcontractor work or outsourced services.', 6::smallint, true,  true),
        ('SF', 'Service Fee',    'General service-related fees.', 7::smallint, true,  true),
        ('SH', 'Shipping',       'Shipping, freight, and delivery charges.', 8::smallint, true,  true),
        ('TX', 'Tax',            'Sales tax or other applicable tax charges.', 9::smallint, true,  false)
) AS v("BillingCategoryType", "BillingCategoryName", "Description", "DisplayOrder", "ShowToFieldTech", "AllowToPick")
WHERE t."BillingCategoryType" = v."BillingCategoryType";

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

-- GloSetupPaymentTerm
INSERT INTO dbo."GloSetupPaymentTerm"
(
    "Name",
    "DueDateMethod",
    "NumberOfDays",
    "IsAccountsReceivable",
    "IsAccountsPayable",
    "IsMobileVisible",
    "IsActive"
)
SELECT
    v."Name",
    v."DueDateMethod",
    v."NumberOfDays",
    v."IsAccountsReceivable",
    v."IsAccountsPayable",
    v."IsMobileVisible",
    v."IsActive"
FROM (
    VALUES
        ('Net 15',       'NetDays',      15,  true, true, true, true),
        ('Net 30',       'NetDays',      30,  true, true, true, true),
        ('Net 45',       'NetDays',      45,  true, true, true, true),
        ('End Of Month', 'EndOfMonth',   NULL::integer, true, true, true, true),
        ('COD',          'DueOnReceipt', 0,   true, true, true, true)
) AS v("Name", "DueDateMethod", "NumberOfDays", "IsAccountsReceivable", "IsAccountsPayable", "IsMobileVisible", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSetupPaymentTerm" t
    WHERE t."Name" = v."Name"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloSetupPaymentTerm"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSetupPaymentTerm"), 1),
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

-- GloJobTypeSubCategory
INSERT INTO dbo."GloJobTypeSubCategory"
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
    FROM dbo."GloJobTypeSubCategory" sc
    WHERE sc."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloJobTypeSubCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloJobTypeSubCategory"), 1),
    true);

-- GloJobTypeCategory
INSERT INTO dbo."GloJobTypeCategory"
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
    FROM dbo."GloJobTypeCategory" c
    WHERE c."BusinessTypeId" = bt."Id"
      AND c."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloJobTypeCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloJobTypeCategory"), 1),
    true);

-- GloInventoryItemType
INSERT INTO dbo."GloInventoryItemType"
(
    "ItemTypeCode",
    "Name",
    "Description",
    "TracksQuantity",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."ItemTypeCode",
    v."Name",
    v."Description",
    v."TracksQuantity",
    v."DisplayOrder",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ('INVENTORY',    'Inventory',     'Standard inventory item that tracks quantity on hand.',                    true,  1::smallint, true),
        ('NONINVENTORY', 'Non-Inventory', 'Item used for purchasing or selling without quantity tracking.',         false, 2::smallint, true),
        ('SERVICE',      'Service',       'Labor or service item with no inventory tracking.',                      false, 3::smallint, true),
        ('KIT',          'Kit',           'Bundle or grouped item composed of multiple inventory items.',           false, 4::smallint, true),
        ('TOOL',         'Tool',          'Operational tool or equipment item that tracks quantity.',               true,  5::smallint, true)
) AS v("ItemTypeCode", "Name", "Description", "TracksQuantity", "DisplayOrder", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloInventoryItemType" t
    WHERE t."ItemTypeCode" = v."ItemTypeCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloInventoryItemType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloInventoryItemType"), 1),
    true);

-- GloInventoryCategory
INSERT INTO dbo."GloInventoryCategory"
(
    "BusinessTypeId",
    "CategoryCode",
    "Name",
    "Description",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    bt."Id",
    v."CategoryCode",
    v."Name",
    v."Description",
    v."DisplayOrder",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('HVAC',           'THERMOSTATS',      'Thermostats',       'HVAC thermostats and controls',              1::smallint),
        ('HVAC',           'CAPACITORS',       'Capacitors',        'HVAC capacitors',                            2::smallint),
        ('HVAC',           'COMPRESSORS',      'Compressors',       'HVAC compressors',                           3::smallint),
        ('HVAC',           'FILTERS',          'Filters',           'Air filters and filtration products',        4::smallint),
        ('HVAC',           'REFRIGERANT',      'Refrigerant',       'Refrigerant products',                       5::smallint),
        ('HVAC',           'TOOLS',            'Tools',             'HVAC tools and equipment',                   6::smallint),
        ('PLUMBING',       'PIPE',             'Pipe',              'Plumbing pipe and fittings',                 1::smallint),
        ('PLUMBING',       'FAUCETS',          'Faucets',           'Kitchen and bathroom faucets',               2::smallint),
        ('PLUMBING',       'VALVES',           'Valves',            'Plumbing valves',                            3::smallint),
        ('PLUMBING',       'WATERHEATERS',     'Water Heaters',     'Water heater systems',                       4::smallint),
        ('PLUMBING',       'DRAINS',           'Drains',            'Drain and sewer products',                   5::smallint),
        ('PLUMBING',       'TOOLS',            'Tools',             'Plumbing tools and equipment',               6::smallint),
        ('ELECTRICAL',     'BREAKERS',         'Breakers',          'Electrical breakers',                        1::smallint),
        ('ELECTRICAL',     'WIRE',             'Wire',              'Electrical wire and cable',                  2::smallint),
        ('ELECTRICAL',     'PANELS',           'Panels',            'Electrical panels',                          3::smallint),
        ('ELECTRICAL',     'SWITCHES',         'Switches',          'Switches and outlets',                       4::smallint),
        ('ELECTRICAL',     'LIGHTING',         'Lighting',          'Lighting fixtures and accessories',            5::smallint),
        ('ELECTRICAL',     'TOOLS',            'Tools',             'Electrical tools and equipment',             6::smallint),
        ('HOUSECLEANING',  'CHEMICALS',        'Chemicals',         'Cleaning chemicals and supplies',            1::smallint),
        ('HOUSECLEANING',  'MOPS',             'Mops',              'Mops and cleaning tools',                    2::smallint),
        ('HOUSECLEANING',  'VACUUMS',          'Vacuums',           'Vacuum equipment',                           3::smallint),
        ('HOUSECLEANING',  'TRASHBAGS',        'Trash Bags',        'Trash bags and liners',                      4::smallint),
        ('HOUSECLEANING',  'PAPERPRODUCTS',    'Paper Products',    'Paper towels and restroom supplies',         5::smallint),
        ('HOUSECLEANING',  'TOOLS',            'Tools',             'Cleaning tools and equipment',               6::smallint),
        ('WINDOWCLEANING', 'SURFACECLEANERS',  'Surface Cleaners',  'Pressure washing surface cleaners',          1::smallint),
        ('WINDOWCLEANING', 'HOSES',            'Hoses',             'Pressure washing hoses',                     2::smallint),
        ('WINDOWCLEANING', 'WANDS',            'Wands',             'Pressure washing wands and guns',            3::smallint),
        ('WINDOWCLEANING', 'CHEMICALS',        'Chemicals',         'Pressure washing chemicals',                 4::smallint),
        ('WINDOWCLEANING', 'NOZZLES',          'Nozzles',           'Pressure washing nozzles',                   5::smallint),
        ('WINDOWCLEANING', 'TOOLS',            'Tools',             'Pressure washing tools and equipment',       6::smallint),
        ('LAWNCARE',       'MOWERS',           'Mowers',            'Lawn mowers and equipment',                  1::smallint),
        ('LAWNCARE',       'TRIMMERS',         'Trimmers',          'Grass trimmers and edgers',                  2::smallint),
        ('LAWNCARE',       'FERTILIZER',       'Fertilizer',        'Fertilizer and lawn chemicals',              3::smallint),
        ('LAWNCARE',       'IRRIGATION',       'Irrigation',        'Irrigation supplies',                        4::smallint),
        ('LAWNCARE',       'PLANTS',           'Plants',            'Plants and landscaping materials',           5::smallint),
        ('LAWNCARE',       'TOOLS',            'Tools',             'Landscaping tools and equipment',            6::smallint)
) AS v("BusinessTypeCode", "CategoryCode", "Name", "Description", "DisplayOrder")
INNER JOIN dbo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloInventoryCategory" c
    WHERE c."BusinessTypeId" = bt."Id"
      AND c."CategoryCode" = v."CategoryCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloInventoryCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloInventoryCategory"), 1),
    true);

-- GloInventorySubCategory
INSERT INTO dbo."GloInventorySubCategory"
(
    "InventoryCategoryId",
    "SubCategoryCode",
    "Name",
    "Description",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    c."Id",
    v."SubCategoryCode",
    v."Name",
    v."Description",
    v."DisplayOrder",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('HVAC',           'THERMOSTATS',  'SMARTTHERMOSTATS',        'Smart Thermostats',        'WiFi and smart thermostats',              1::smallint),
        ('HVAC',           'THERMOSTATS',  'PROGRAMMABLETHERMOSTATS', 'Programmable Thermostats', 'Programmable thermostats',                2::smallint),
        ('HVAC',           'CAPACITORS',   'RUN_CAPACITORS',          'Run Capacitors',           'HVAC run capacitors',                     1::smallint),
        ('HVAC',           'CAPACITORS',   'START_CAPACITORS',        'Start Capacitors',         'HVAC start capacitors',                   2::smallint),
        ('HVAC',           'COMPRESSORS',  'SCROLLCOMPRESSORS',       'Scroll Compressors',       'Scroll compressors',                      1::smallint),
        ('HVAC',           'COMPRESSORS',  'RECIPROCATINGCOMPRESSORS','Reciprocating Compressors','Reciprocating compressors',               2::smallint),
        ('HVAC',           'FILTERS',      'PLEATEDFILTERS',          'Pleated Filters',          'Pleated air filters',                     1::smallint),
        ('HVAC',           'FILTERS',      'HEPAFILTERS',             'HEPA Filters',             'HEPA filtration products',                2::smallint),
        ('HVAC',           'REFRIGERANT',  'R410A',                   'R-410A',                   'R-410A refrigerant',                      1::smallint),
        ('HVAC',           'REFRIGERANT',  'R32',                     'R-32',                     'R-32 refrigerant',                          2::smallint),
        ('HVAC',           'TOOLS',        'VACUUMPUMPS',             'Vacuum Pumps',             'HVAC vacuum pumps',                       1::smallint),
        ('HVAC',           'TOOLS',        'RECOVERYMACHINES',       'Recovery Machines',        'Refrigerant recovery machines',           2::smallint),
        ('PLUMBING',       'PIPE',         'PVCPIPES',                'PVC Pipes',                'PVC plumbing pipes',                      1::smallint),
        ('PLUMBING',       'PIPE',         'COPPERPIPES',             'Copper Pipes',             'Copper plumbing pipes',                   2::smallint),
        ('PLUMBING',       'FAUCETS',      'KITCHENFAUCETS',          'Kitchen Faucets',          'Kitchen sink faucets',                    1::smallint),
        ('PLUMBING',       'FAUCETS',      'BATHROOMFAUCETS',         'Bathroom Faucets',         'Bathroom sink faucets',                   2::smallint),
        ('PLUMBING',       'VALVES',       'BALLVALVES',              'Ball Valves',              'Ball valves',                             1::smallint),
        ('PLUMBING',       'VALVES',       'GATEVALVES',              'Gate Valves',              'Gate valves',                             2::smallint),
        ('PLUMBING',       'WATERHEATERS', 'TANKWATERHEATERS',        'Tank Water Heaters',       'Traditional tank water heaters',          1::smallint),
        ('PLUMBING',       'WATERHEATERS', 'TANKLESSWATERHEATERS',    'Tankless Water Heaters',   'Tankless water heaters',                  2::smallint),
        ('PLUMBING',       'DRAINS',       'FLOORDRAINS',             'Floor Drains',             'Floor drain products',                    1::smallint),
        ('PLUMBING',       'DRAINS',       'SHOWERDRAINS',            'Shower Drains',            'Shower drain products',                   2::smallint),
        ('PLUMBING',       'TOOLS',        'PIPERENCHES',             'Pipe Wrenches',            'Pipe wrenches',                           1::smallint),
        ('PLUMBING',       'TOOLS',        'DRAINMACHINES',          'Drain Machines',           'Drain cleaning machines',                 2::smallint),
        ('ELECTRICAL',     'BREAKERS',     'SINGLEPOLEBREAKERS',      'Single Pole Breakers',     'Single pole breakers',                    1::smallint),
        ('ELECTRICAL',     'BREAKERS',     'DOUBLEPOLEBREAKERS',      'Double Pole Breakers',     'Double pole breakers',                    2::smallint),
        ('ELECTRICAL',     'WIRE',         'ROMEXWIRE',               'Romex Wire',               'Romex electrical wire',                   1::smallint),
        ('ELECTRICAL',     'WIRE',         'THHNWIRE',                'THHN Wire',                'THHN electrical wire',                    2::smallint),
        ('ELECTRICAL',     'PANELS',       'MAINPANELS',              'Main Panels',              'Main electrical panels',                  1::smallint),
        ('ELECTRICAL',     'PANELS',       'SUBPANELS',               'Sub Panels',               'Sub electrical panels',                   2::smallint),
        ('ELECTRICAL',     'SWITCHES',     'DIMMERSWITCHES',          'Dimmer Switches',          'Dimmer switches',                         1::smallint),
        ('ELECTRICAL',     'SWITCHES',     'GFCIOUTLETS',             'GFCI Outlets',             'GFCI outlets',                            2::smallint),
        ('ELECTRICAL',     'LIGHTING',     'LEDFIXTURES',           'LED Fixtures',             'LED lighting fixtures',                   1::smallint),
        ('ELECTRICAL',     'LIGHTING',     'OUTDOORLIGHTING',         'Outdoor Lighting',         'Outdoor lighting products',               2::smallint),
        ('ELECTRICAL',     'TOOLS',        'MULTIMETERS',             'Multimeters',              'Electrical multimeters',                  1::smallint),
        ('ELECTRICAL',     'TOOLS',        'WIRESTRIPPERS',           'Wire Strippers',           'Wire stripping tools',                    2::smallint)
) AS v("BusinessTypeCode", "CategoryCode", "SubCategoryCode", "Name", "Description", "DisplayOrder")
INNER JOIN dbo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
INNER JOIN dbo."GloInventoryCategory" c
    ON c."BusinessTypeId" = bt."Id"
   AND c."CategoryCode" = v."CategoryCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloInventorySubCategory" sc
    WHERE sc."InventoryCategoryId" = c."Id"
      AND sc."SubCategoryCode" = v."SubCategoryCode"
);

SELECT setval(
    pg_get_serial_sequence('dbo."GloInventorySubCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloInventorySubCategory"), 1),
    true);

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
-- Transformation types: TENANT_ID, COMPANY_ID, STATIC, CURRENT_TIMESTAMP, SEED_CREATED_BY
-- =============================================================================

INSERT INTO dbo."GloSeedTableMapping"
(
    "SeedCode",
    "SourceDatabaseName",
    "SourceSchemaName",
    "SourceTableName",
    "TargetDatabaseName",
    "TargetSchemaName",
    "TargetTableName",
    "SeedOrder",
    "Description",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    v."SeedCode",
    v."SourceDatabaseName",
    v."SourceSchemaName",
    v."SourceTableName",
    v."TargetDatabaseName",
    v."TargetSchemaName",
    v."TargetTableName",
    v."SeedOrder",
    v."Description",
    v."IsActive",
    timezone('utc', now()),
    NULL::bigint
FROM (
    VALUES
        ('ALL_GloBillingCategory', 'fgs_dev_db', 'dbo', 'GloBillingCategory', 'fgs_dev_db', 'dbo', 'FgsBillingCategory', 100, 'Billing Category', true),
        ('ALL_GloJobTypeCategory', 'fgs_dev_db', 'dbo', 'GloJobTypeCategory', 'fgs_dev_db', 'dbo', 'FgsJobTypeCategory', 130, 'JobType Categories', true),
        ('ALL_GloJobTypeSubCategory', 'fgs_dev_db', 'dbo', 'GloJobTypeSubCategory', 'fgs_dev_db', 'dbo', 'FgsJobTypeSubCategory', 160, 'JobType Sub Categories', true),
        ('ALL_GloLeadSource', 'fgs_dev_db', 'dbo', 'GloLeadSource', 'fgs_dev_db', 'dbo', 'FgsLeadSource', 190, 'Lead Source', true),
        ('ALL_GloPaymentMethodType', 'fgs_dev_db', 'dbo', 'GloPaymentMethodType', 'fgs_dev_db', 'dbo', 'FgsSetupPaymentMethod', 220, 'Payment Method', true),
        ('ALL_GloResolutionType', 'fgs_dev_db', 'dbo', 'GloResolutionType', 'fgs_dev_db', 'dbo', 'FgsResolutionCode', 250, 'Resolution Code', true),
        ('ALL_GloSetupLaborRateType', 'fgs_dev_db', 'dbo', 'GloSetupLaborRateType', 'fgs_dev_db', 'dbo', 'FgsSetupLaborRateType', 280, 'Labor Rate Type', true),
        ('GloSkill', 'fgs_dev_db', 'dbo', 'GloSkill', 'fgs_dev_db', 'dbo', 'FgsSetupTechSkillLevel', 310, 'Technician Skill', true),
        ('ALL_GloTag', 'fgs_dev_db', 'dbo', 'GloTag', 'fgs_dev_db', 'dbo', 'FgsTag', 340, 'Tags', true),
        ('GloTrade', 'fgs_dev_db', 'dbo', 'GloTrade', 'fgs_dev_db', 'dbo', 'FgsSetupTechTrade', 410, 'Technician Trade', true),
        ('ALL_GloTitleOfCourtesy', 'fgs_dev_db', 'dbo', 'GloTitleOfCourtesy', 'fgs_dev_db', 'dbo', 'FgsSetupTitleOfCourtesy', 440, 'Title Of Courtesy', true),
        ('ALL_GloZone', 'fgs_dev_db', 'dbo', 'GloZone', 'fgs_dev_db', 'dbo', 'FgsSetupZone', 470, 'Zone', true),
        ('ALL_GloSetupPaymentTerm', 'fgs_dev_db', 'dbo', 'GloSetupPaymentTerm', 'fgs_dev_db', 'dbo', 'FgsSetupPaymentTerm', 500, 'Payment Term', true)
) AS v("SeedCode", "SourceDatabaseName", "SourceSchemaName", "SourceTableName", "TargetDatabaseName", "TargetSchemaName", "TargetTableName", "SeedOrder", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1 FROM dbo."GloSeedTableMapping" m WHERE m."SeedCode" = v."SeedCode"
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
    "CreatedOn",
    "CreatedBy"
)
SELECT
    m."Id",
    c."SourceColumnName",
    c."TargetColumnName",
    c."TransformationType",
    c."StaticValue",
    c."ColumnOrder",
    c."IsRequired",
    c."IsActive",
    timezone('utc', now()),
    NULL::bigint
FROM dbo."GloSeedTableMapping" m
INNER JOIN (
    VALUES
        -- ALL_GloBillingCategory -> FgsBillingCategory
        ('ALL_GloBillingCategory', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloBillingCategory', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloBillingCategory', 'BillingCategoryType', 'BillingCategoryType', NULL, NULL, 3, true, true),
        ('ALL_GloBillingCategory', 'BillingCategoryName', 'BillingCategoryName', NULL, NULL, 4, true, true),
        ('ALL_GloBillingCategory', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('ALL_GloBillingCategory', 'DisplayOrder', 'DisplayOrder', NULL, NULL, 6, true, true),
        ('ALL_GloBillingCategory', NULL, 'IsSystemDefined', 'STATIC', 'true', 7, true, true),
        ('ALL_GloBillingCategory', 'ShowToFieldTech', 'ShowToFieldTech', NULL, NULL, 8, true, true),
        ('ALL_GloBillingCategory', 'AllowToPick', 'AllowToPick', NULL, NULL, 9, true, true),
        ('ALL_GloBillingCategory', NULL, 'IsActive', 'STATIC', 'true', 10, true, true),
        ('ALL_GloBillingCategory', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 11, true, true),
        ('ALL_GloBillingCategory', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 12, false, true),

        -- ALL_GloJobTypeCategory -> FgsJobTypeCategory
        ('ALL_GloJobTypeCategory', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloJobTypeCategory', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloJobTypeCategory', 'Code', 'CategoryCode', NULL, NULL, 3, true, true),
        ('ALL_GloJobTypeCategory', 'Name', 'Name', NULL, NULL, 4, true, true),
        ('ALL_GloJobTypeCategory', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('ALL_GloJobTypeCategory', 'Id', 'DisplayOrder', NULL, NULL, 6, true, true),
        ('ALL_GloJobTypeCategory', 'IsActive', 'IsActive', NULL, NULL, 7, true, true),
        ('ALL_GloJobTypeCategory', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 8, true, true),
        ('ALL_GloJobTypeCategory', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 9, false, true),

        -- ALL_GloJobTypeSubCategory -> FgsJobTypeSubCategory
        ('ALL_GloJobTypeSubCategory', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloJobTypeSubCategory', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloJobTypeSubCategory', 'Code', 'SubCategoryCode', NULL, NULL, 3, true, true),
        ('ALL_GloJobTypeSubCategory', 'Name', 'Name', NULL, NULL, 4, true, true),
        ('ALL_GloJobTypeSubCategory', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('ALL_GloJobTypeSubCategory', 'Id', 'DisplayOrder', NULL, NULL, 6, true, true),
        ('ALL_GloJobTypeSubCategory', 'IsActive', 'IsActive', NULL, NULL, 7, true, true),
        ('ALL_GloJobTypeSubCategory', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 8, true, true),
        ('ALL_GloJobTypeSubCategory', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 9, false, true),

        -- ALL_GloLeadSource -> FgsLeadSource
        ('ALL_GloLeadSource', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloLeadSource', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloLeadSource', 'SourceCode', 'SourceCode', NULL, NULL, 3, true, true),
        ('ALL_GloLeadSource', 'SourceName', 'SourceName', NULL, NULL, 4, true, true),
        ('ALL_GloLeadSource', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('ALL_GloLeadSource', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('ALL_GloLeadSource', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),
        ('ALL_GloLeadSource', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 8, false, true),

        -- ALL_GloPaymentMethodType -> FgsSetupPaymentMethod
        ('ALL_GloPaymentMethodType', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloPaymentMethodType', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloPaymentMethodType', 'DisplayName', 'DisplayName', NULL, NULL, 3, true, true),
        ('ALL_GloPaymentMethodType', 'SortOrder', 'SortOrder', NULL, NULL, 4, true, true),
        ('ALL_GloPaymentMethodType', NULL, 'IsMobileVisible', 'STATIC', 'true', 5, true, true),
        ('ALL_GloPaymentMethodType', NULL, 'IsCustomerPortalVisible', 'STATIC', 'true', 6, true, true),
        ('ALL_GloPaymentMethodType', 'IsActive', 'IsActive', NULL, NULL, 7, true, true),
        ('ALL_GloPaymentMethodType', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 8, true, true),
        ('ALL_GloPaymentMethodType', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 9, false, true),

        -- ALL_GloResolutionType -> FgsResolutionCode
        ('ALL_GloResolutionType', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloResolutionType', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloResolutionType', 'Id', 'GloResolutionTypeId', NULL, NULL, 3, true, true),
        ('ALL_GloResolutionType', 'ResolutionTypeCode', 'ResolutionCode', NULL, NULL, 4, true, true),
        ('ALL_GloResolutionType', 'ResolutionTypeName', 'ResolutionName', NULL, NULL, 5, true, true),
        ('ALL_GloResolutionType', NULL, 'IsMobileVisible', 'STATIC', 'true', 6, true, true),
        ('ALL_GloResolutionType', 'IsActive', 'IsActive', NULL, NULL, 7, true, true),
        ('ALL_GloResolutionType', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 8, true, true),
        ('ALL_GloResolutionType', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 9, false, true),

        -- ALL_GloSetupLaborRateType -> FgsSetupLaborRateType
        ('ALL_GloSetupLaborRateType', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloSetupLaborRateType', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloSetupLaborRateType', 'Name', 'Name', NULL, NULL, 3, true, true),
        ('ALL_GloSetupLaborRateType', 'Description', 'Description', NULL, NULL, 4, false, true),
        ('ALL_GloSetupLaborRateType', 'SortOrder', 'SortOrder', NULL, NULL, 5, true, true),
        ('ALL_GloSetupLaborRateType', 'IsSystem', 'IsSystem', NULL, NULL, 6, true, true),
        ('ALL_GloSetupLaborRateType', 'IsActive', 'IsActive', NULL, NULL, 7, true, true),
        ('ALL_GloSetupLaborRateType', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 8, true, true),
        ('ALL_GloSetupLaborRateType', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 9, false, true),

        -- GloSkill -> FgsSetupTechSkillLevel
        ('GloSkill', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('GloSkill', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('GloSkill', 'SkillCode', 'Code', NULL, NULL, 3, true, true),
        ('GloSkill', 'SkillName', 'Name', NULL, NULL, 4, true, true),
        ('GloSkill', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('GloSkill', 'Id', 'SortOrder', NULL, NULL, 6, false, true),
        ('GloSkill', 'IsActive', 'IsActive', NULL, NULL, 7, true, true),
        ('GloSkill', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 8, true, true),
        ('GloSkill', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 9, false, true),

        -- ALL_GloTag -> FgsTag
        ('ALL_GloTag', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloTag', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloTag', 'TagCode', 'TagCode', NULL, NULL, 3, true, true),
        ('ALL_GloTag', 'Name', 'Name', NULL, NULL, 4, true, true),
        ('ALL_GloTag', 'NormalizedName', 'NormalizedName', NULL, NULL, 5, true, true),
        ('ALL_GloTag', 'Description', 'Description', NULL, NULL, 6, false, true),
        ('ALL_GloTag', 'BackgroundColor', 'BackgroundColor', NULL, NULL, 7, false, true),
        ('ALL_GloTag', 'TextColor', 'TextColor', NULL, NULL, 8, false, true),
        ('ALL_GloTag', 'IconFileId', 'IconFileId', NULL, NULL, 9, false, true),
        ('ALL_GloTag', 'IsSystemGenerated', 'IsSystemGenerated', NULL, NULL, 10, true, true),
        ('ALL_GloTag', 'IsActive', 'IsActive', NULL, NULL, 11, true, true),
        ('ALL_GloTag', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 12, true, true),

        -- GloTrade -> FgsSetupTechTrade
        ('GloTrade', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('GloTrade', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('GloTrade', 'TradeCode', 'TradeCode', NULL, NULL, 3, true, true),
        ('GloTrade', 'TradeName', 'Name', NULL, NULL, 4, true, true),
        ('GloTrade', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('GloTrade', 'Id', 'SortOrder', NULL, NULL, 6, false, true),
        ('GloTrade', 'IsActive', 'IsActive', NULL, NULL, 7, true, true),
        ('GloTrade', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 8, true, true),
        ('GloTrade', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 9, false, true),

        -- ALL_GloTitleOfCourtesy -> FgsSetupTitleOfCourtesy
        ('ALL_GloTitleOfCourtesy', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloTitleOfCourtesy', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloTitleOfCourtesy', 'Code', 'Code', NULL, NULL, 3, true, true),
        ('ALL_GloTitleOfCourtesy', 'DisplayName', 'DisplayName', NULL, NULL, 4, true, true),
        ('ALL_GloTitleOfCourtesy', 'SortOrder', 'SortOrder', NULL, NULL, 5, false, true),
        ('ALL_GloTitleOfCourtesy', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('ALL_GloTitleOfCourtesy', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),
        ('ALL_GloTitleOfCourtesy', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 8, false, true),

        -- ALL_GloZone -> FgsSetupZone
        ('ALL_GloZone', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloZone', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloZone', 'Code', 'Code', NULL, NULL, 3, true, true),
        ('ALL_GloZone', 'Name', 'Name', NULL, NULL, 4, true, true),
        ('ALL_GloZone', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('ALL_GloZone', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('ALL_GloZone', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),
        ('ALL_GloZone', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 8, false, true),

        -- ALL_GloSetupPaymentTerm -> FgsSetupPaymentTerm
        ('ALL_GloSetupPaymentTerm', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloSetupPaymentTerm', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloSetupPaymentTerm', 'Name', 'Name', NULL, NULL, 3, true, true),
        ('ALL_GloSetupPaymentTerm', 'DueDateMethod', 'DueDateMethod', NULL, NULL, 4, true, true),
        ('ALL_GloSetupPaymentTerm', 'NumberOfDays', 'NumberOfDays', NULL, NULL, 5, false, true),
        ('ALL_GloSetupPaymentTerm', 'IsAccountsReceivable', 'IsAccountsReceivable', NULL, NULL, 6, true, true),
        ('ALL_GloSetupPaymentTerm', 'IsAccountsPayable', 'IsAccountsPayable', NULL, NULL, 7, true, true),
        ('ALL_GloSetupPaymentTerm', 'IsMobileVisible', 'IsMobileVisible', NULL, NULL, 8, true, true),
        ('ALL_GloSetupPaymentTerm', 'IsActive', 'IsActive', NULL, NULL, 9, true, true),
        ('ALL_GloSetupPaymentTerm', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 10, true, true),
        ('ALL_GloSetupPaymentTerm', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 11, false, true)
) AS c("SeedCode", "SourceColumnName", "TargetColumnName", "TransformationType", "StaticValue", "ColumnOrder", "IsRequired", "IsActive")
    ON c."SeedCode" = m."SeedCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo."GloSeedTableColumnMapping" existing
    WHERE existing."SeedTableMappingId" = m."Id"
      AND existing."TargetColumnName" = c."TargetColumnName"
);

UPDATE dbo."GloSeedTableColumnMapping" AS existing
SET
    "SourceColumnName" = 'ShowToFieldTech',
    "TransformationType" = NULL,
    "StaticValue" = NULL,
    "ColumnOrder" = 8,
    "IsRequired" = true,
    "IsActive" = true
FROM dbo."GloSeedTableMapping" m
WHERE existing."SeedTableMappingId" = m."Id"
  AND m."SeedCode" = 'ALL_GloBillingCategory'
  AND existing."TargetColumnName" = 'ShowToFieldTech'
  AND existing."TransformationType" = 'STATIC';

SELECT setval(
    pg_get_serial_sequence('dbo."GloSeedTableColumnMapping"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloSeedTableColumnMapping"), 1),
    true);

COMMIT;
