-- =============================================================================
-- Seed: CleanUpTables global reference data (User Service)
-- Run manually after migrations (including 20260601115438_AddGloCommunicationTemplateAndSchemaComments).
-- Not part of EF migration / Up / Down scripts.
--
-- Idempotent: each insert skips rows that already exist (matched by natural key).
-- Defaults where applicable:
--   CreatedOn = UTC now
--   CreatedBy = 'System' (varchar(100) audit columns)
-- =============================================================================

START TRANSACTION;

-- GloLocationType (Code = Name)
INSERT INTO glo."GloLocationType"
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
    FROM glo."GloLocationType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloLocationType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloLocationType"), 1),
    true);

-- GloMasterEntityType
INSERT INTO glo."GloMasterEntityType"
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
        ('INVOICE',         true, true, 11),
        ('Warehouse',       true, true, 12),
        ('Vehicle',         true, true, 13),
        ('VehicleMaintenance', true, true, 14)
) AS v("Code", "IsDocumentAllowed", "IsActive", "SortOrder")
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloMasterEntityType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloMasterEntityType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloMasterEntityType"), 1),
    true);

-- GloVehicleMaintenanceType
INSERT INTO glo."GloVehicleMaintenanceType"
(
    "MaintenanceTypeCode",
    "Name",
    "Description",
    "DisplayOrder"
)
SELECT
    v."MaintenanceTypeCode",
    v."Name",
    v."Description",
    v."DisplayOrder"
FROM (
    VALUES
        ('OIL_CHANGE',           'Oil Change',           'Engine oil and filter replacement.',                              1::smallint),
        ('TIRE_ROTATION',        'Tire Rotation',        'Rotation of vehicle tires to promote even wear.',                 2::smallint),
        ('TIRE_REPLACEMENT',     'Tire Replacement',     'Replacement of one or more vehicle tires.',                       3::smallint),
        ('BRAKE_SERVICE',        'Brake Service',        'Inspection, repair, or replacement of brake components.',           4::smallint),
        ('INSPECTION',           'Inspection',           'General vehicle inspection and safety check.',                    5::smallint),
        ('BATTERY_REPLACEMENT',  'Battery Replacement',  'Replacement of vehicle battery.',                                 6::smallint),
        ('TRANSMISSION_SERVICE', 'Transmission Service', 'Maintenance or repair of transmission system.',                   7::smallint),
        ('REGISTRATION_RENEWAL', 'Registration Renewal', 'Vehicle registration renewal.',                                   8::smallint),
        ('REPAIR',               'Repair',               'General repair work not covered by a specific maintenance type.', 9::smallint),
        ('OTHER',                'Other',                'Other maintenance activity.',                                     99::smallint)
) AS v("MaintenanceTypeCode", "Name", "Description", "DisplayOrder")
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloVehicleMaintenanceType" t
    WHERE t."MaintenanceTypeCode" = v."MaintenanceTypeCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloVehicleMaintenanceType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloVehicleMaintenanceType"), 1),
    true);

-- GloLanguage
INSERT INTO glo."GloLanguage"
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
    FROM glo."GloLanguage" t
    WHERE t."LanguageCode" = v."LanguageCode"
);

-- GloAccountingIntegrationType
INSERT INTO glo."GloAccountingIntegrationType"
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
    FROM glo."GloAccountingIntegrationType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloAccountingIntegrationType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloAccountingIntegrationType"), 1),
    true);

-- GloTimeCardOption
INSERT INTO glo."GloTimeCardOption"
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
    FROM glo."GloTimeCardOption" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloTimeCardOption"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloTimeCardOption"), 1),
    true);

-- GloBusinessType (explicit Id: sequential 1..n; OTHER last)
INSERT INTO glo."GloBusinessType"
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
    FROM glo."GloBusinessType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloBusinessType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloBusinessType"), 1),
    true);

-- GloBillingCategory (no CreatedOn/CreatedBy columns)
INSERT INTO glo."GloBillingCategory"
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
    FROM glo."GloBillingCategory" t
    WHERE t."BillingCategoryType" = v."BillingCategoryType"
);

UPDATE glo."GloBillingCategory" AS t
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
INSERT INTO glo."GloCountry"
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
    FROM glo."GloCountry" t
    WHERE t."CountryCode" = v."CountryCode"
);

-- GloCredentialProviderType
INSERT INTO glo."GloCredentialProviderType"
(
    "ProviderCode",
    "ProviderName",
    "ConfigurationSchema",
    "IsActive",
    "CreatedOn",
    "CreatedBy"
)
SELECT
    v."ProviderCode",
    v."ProviderName",
    v."ConfigurationSchema"::jsonb,
    v."IsActive",
    timezone('utc', now()),
    'SYSTEM'
FROM (
    VALUES
        (
            'DATABASE',
            'Database Connections',
            '[
                {"key":"FgsUser","label":"User Service (FgsUser)","type":"password","required":false,"sensitive":true},
                {"key":"FgsSetup","label":"Setup Service (FgsSetup)","type":"password","required":false,"sensitive":true},
                {"key":"FgsFile","label":"File Service (FgsFile)","type":"password","required":false,"sensitive":true},
                {"key":"FgsNotification","label":"Notification Service (FgsNotification)","type":"password","required":false,"sensitive":true},
                {"key":"FgsConsumer","label":"Consumer Service (FgsConsumer)","type":"password","required":false,"sensitive":true},
                {"key":"FgsAudit","label":"Audit Service (FgsAudit)","type":"password","required":false,"sensitive":true},
                {"key":"FgsBilling","label":"Billing Service (FgsBilling)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCommunication","label":"Communication Service (FgsCommunication)","type":"password","required":false,"sensitive":true},
                {"key":"FgsContract","label":"Contract Service (FgsContract)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCrm","label":"CRM Service (FgsCrm)","type":"password","required":false,"sensitive":true},
                {"key":"FgsDispatch","label":"Dispatch Service (FgsDispatch)","type":"password","required":false,"sensitive":true},
                {"key":"FgsIntegration","label":"Integration Service (FgsIntegration)","type":"password","required":false,"sensitive":true},
                {"key":"FgsInventory","label":"Inventory Service (FgsInventory)","type":"password","required":false,"sensitive":true},
                {"key":"FgsJob","label":"Job Service (FgsJob)","type":"password","required":false,"sensitive":true},
                {"key":"FgsReporting","label":"Reporting Service (FgsReporting)","type":"password","required":false,"sensitive":true},
                {"key":"ConnectionStringName","label":"Single connection name (legacy)","type":"text","required":false},
                {"key":"ConnectionString","label":"Single connection string (legacy)","type":"password","required":false,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'RABBITMQ',
            'RabbitMQ',
            '[
                {"key":"Username","label":"Username","type":"text","required":true},
                {"key":"Password","label":"Password","type":"password","required":true,"sensitive":true},
                {"key":"ConnectionUri","label":"Connection URI","type":"text","required":false,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'AWS',
            'Amazon Web Services',
            '[
                {"key":"AccessKeyId","label":"Access Key ID","type":"text","required":true},
                {"key":"SecretAccessKey","label":"Secret Access Key","type":"password","required":true,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'ENTRA_EXTERNAL_ID',
            'Microsoft Entra External ID',
            '[
                {"key":"TenantId","label":"Entra Tenant ID","type":"text","required":true},
                {"key":"ClientId","label":"Client ID","type":"text","required":true},
                {"key":"ClientSecret","label":"Client Secret","type":"password","required":true,"sensitive":true},
                {"key":"Authority","label":"Authority","type":"text","required":true},
                {"key":"RedirectUri","label":"Redirect URI","type":"text","required":true},
                {"key":"Scopes","label":"Scopes","type":"text","required":true},
                {"key":"UserFlow","label":"User Flow","type":"text","required":false},
                {"key":"AuthorizeEndpoint","label":"Authorize Endpoint","type":"text","required":false},
                {"key":"TokenEndpoint","label":"Token Endpoint","type":"text","required":false}
            ]',
            TRUE
        ),
        (
            'SENDGRID',
            'SendGrid',
            '[
                {"key":"ApiKey","label":"API Key","type":"password","required":true,"sensitive":true},
                {"key":"FromAddress","label":"From Address","type":"text","required":true},
                {"key":"FromName","label":"From Name","type":"text","required":true}
            ]',
            TRUE
        ),
        (
            'TWILIO',
            'Twilio',
            '[
                {"key":"AccountSid","label":"Account SID","type":"text","required":true},
                {"key":"AuthToken","label":"Auth Token","type":"password","required":true,"sensitive":true},
                {"key":"FromNumber","label":"From Number","type":"text","required":false}
            ]',
            TRUE
        ),
        (
            'FIREBASE',
            'Firebase',
            '[
                {"key":"ServiceAccountJson","label":"Service Account JSON","type":"password","required":true,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'STRIPE',
            'Stripe',
            '[
                {"key":"SecretKey","label":"Secret Key","type":"password","required":true,"sensitive":true},
                {"key":"WebhookSecret","label":"Webhook Secret","type":"password","required":false,"sensitive":true},
                {"key":"PublishableKey","label":"Publishable Key","type":"text","required":false}
            ]',
            FALSE
        ),
        (
            'SMTP',
            'SMTP',
            '[
                {"key":"Host","label":"Host","type":"text","required":true},
                {"key":"Port","label":"Port","type":"number","required":true},
                {"key":"Username","label":"Username","type":"text","required":false},
                {"key":"Password","label":"Password","type":"password","required":false,"sensitive":true},
                {"key":"EnableSsl","label":"Enable SSL","type":"boolean","required":false}
            ]',
            FALSE
        ),
        (
            'JWT',
            'JWT Signing',
            '[
                {"key":"SigningKey","label":"Signing Key","type":"password","required":true,"sensitive":true},
                {"key":"Issuer","label":"Issuer","type":"text","required":false},
                {"key":"Audience","label":"Audience","type":"text","required":false}
            ]',
            FALSE
        ),
        (
            'WEBHOOK',
            'Webhook',
            '[
                {"key":"Secret","label":"Secret","type":"password","required":true,"sensitive":true},
                {"key":"SigningKey","label":"Signing Key","type":"password","required":false,"sensitive":true}
            ]',
            FALSE
        )
) AS v("ProviderCode", "ProviderName", "ConfigurationSchema", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloCredentialProviderType" t
    WHERE t."ProviderCode" = v."ProviderCode"
);

-- Sync corrected schemas and IsActive flags for existing provider rows
UPDATE glo."GloCredentialProviderType" AS t SET
    "ProviderName" = v."ProviderName",
    "ConfigurationSchema" = v."ConfigurationSchema"::jsonb,
    "IsActive" = v."IsActive",
    "UpdatedOn" = timezone('utc', now()),
    "UpdatedBy" = 'SYSTEM'
FROM (
    VALUES
        (
            'DATABASE',
            'Database Connections',
            '[
                {"key":"FgsUser","label":"User Service (FgsUser)","type":"password","required":false,"sensitive":true},
                {"key":"FgsSetup","label":"Setup Service (FgsSetup)","type":"password","required":false,"sensitive":true},
                {"key":"FgsFile","label":"File Service (FgsFile)","type":"password","required":false,"sensitive":true},
                {"key":"FgsNotification","label":"Notification Service (FgsNotification)","type":"password","required":false,"sensitive":true},
                {"key":"FgsConsumer","label":"Consumer Service (FgsConsumer)","type":"password","required":false,"sensitive":true},
                {"key":"FgsAudit","label":"Audit Service (FgsAudit)","type":"password","required":false,"sensitive":true},
                {"key":"FgsBilling","label":"Billing Service (FgsBilling)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCommunication","label":"Communication Service (FgsCommunication)","type":"password","required":false,"sensitive":true},
                {"key":"FgsContract","label":"Contract Service (FgsContract)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCrm","label":"CRM Service (FgsCrm)","type":"password","required":false,"sensitive":true},
                {"key":"FgsDispatch","label":"Dispatch Service (FgsDispatch)","type":"password","required":false,"sensitive":true},
                {"key":"FgsIntegration","label":"Integration Service (FgsIntegration)","type":"password","required":false,"sensitive":true},
                {"key":"FgsInventory","label":"Inventory Service (FgsInventory)","type":"password","required":false,"sensitive":true},
                {"key":"FgsJob","label":"Job Service (FgsJob)","type":"password","required":false,"sensitive":true},
                {"key":"FgsReporting","label":"Reporting Service (FgsReporting)","type":"password","required":false,"sensitive":true},
                {"key":"ConnectionStringName","label":"Single connection name (legacy)","type":"text","required":false},
                {"key":"ConnectionString","label":"Single connection string (legacy)","type":"password","required":false,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'RABBITMQ',
            'RabbitMQ',
            '[
                {"key":"Username","label":"Username","type":"text","required":true},
                {"key":"Password","label":"Password","type":"password","required":true,"sensitive":true},
                {"key":"ConnectionUri","label":"Connection URI","type":"text","required":false,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'AWS',
            'Amazon Web Services',
            '[
                {"key":"AccessKeyId","label":"Access Key ID","type":"text","required":true},
                {"key":"SecretAccessKey","label":"Secret Access Key","type":"password","required":true,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'ENTRA_EXTERNAL_ID',
            'Microsoft Entra External ID',
            '[
                {"key":"TenantId","label":"Entra Tenant ID","type":"text","required":true},
                {"key":"ClientId","label":"Client ID","type":"text","required":true},
                {"key":"ClientSecret","label":"Client Secret","type":"password","required":true,"sensitive":true},
                {"key":"Authority","label":"Authority","type":"text","required":true},
                {"key":"RedirectUri","label":"Redirect URI","type":"text","required":true},
                {"key":"Scopes","label":"Scopes","type":"text","required":true},
                {"key":"UserFlow","label":"User Flow","type":"text","required":false},
                {"key":"AuthorizeEndpoint","label":"Authorize Endpoint","type":"text","required":false},
                {"key":"TokenEndpoint","label":"Token Endpoint","type":"text","required":false}
            ]',
            TRUE
        ),
        (
            'SENDGRID',
            'SendGrid',
            '[
                {"key":"ApiKey","label":"API Key","type":"password","required":true,"sensitive":true},
                {"key":"FromAddress","label":"From Address","type":"text","required":true},
                {"key":"FromName","label":"From Name","type":"text","required":true}
            ]',
            TRUE
        ),
        (
            'TWILIO',
            'Twilio',
            '[
                {"key":"AccountSid","label":"Account SID","type":"text","required":true},
                {"key":"AuthToken","label":"Auth Token","type":"password","required":true,"sensitive":true},
                {"key":"FromNumber","label":"From Number","type":"text","required":false}
            ]',
            TRUE
        ),
        (
            'FIREBASE',
            'Firebase',
            '[
                {"key":"ServiceAccountJson","label":"Service Account JSON","type":"password","required":true,"sensitive":true}
            ]',
            TRUE
        ),
        (
            'STRIPE',
            'Stripe',
            '[
                {"key":"SecretKey","label":"Secret Key","type":"password","required":true,"sensitive":true},
                {"key":"WebhookSecret","label":"Webhook Secret","type":"password","required":false,"sensitive":true},
                {"key":"PublishableKey","label":"Publishable Key","type":"text","required":false}
            ]',
            FALSE
        ),
        (
            'SMTP',
            'SMTP',
            '[
                {"key":"Host","label":"Host","type":"text","required":true},
                {"key":"Port","label":"Port","type":"number","required":true},
                {"key":"Username","label":"Username","type":"text","required":false},
                {"key":"Password","label":"Password","type":"password","required":false,"sensitive":true},
                {"key":"EnableSsl","label":"Enable SSL","type":"boolean","required":false}
            ]',
            FALSE
        ),
        (
            'JWT',
            'JWT Signing',
            '[
                {"key":"SigningKey","label":"Signing Key","type":"password","required":true,"sensitive":true},
                {"key":"Issuer","label":"Issuer","type":"text","required":false},
                {"key":"Audience","label":"Audience","type":"text","required":false}
            ]',
            FALSE
        ),
        (
            'WEBHOOK',
            'Webhook',
            '[
                {"key":"Secret","label":"Secret","type":"password","required":true,"sensitive":true},
                {"key":"SigningKey","label":"Signing Key","type":"password","required":false,"sensitive":true}
            ]',
            FALSE
        )
) AS v("ProviderCode", "ProviderName", "ConfigurationSchema", "IsActive")
WHERE t."ProviderCode" = v."ProviderCode";

-- Refresh setup provider type cache when present
UPDATE setup."GloCredentialProviderTypeCache" AS c SET
    "ProviderName" = src."ProviderName",
    "ConfigurationSchema" = src."ConfigurationSchema",
    "IsActive" = src."IsActive",
    "UpdatedOn" = timezone('utc', now())
FROM glo."GloCredentialProviderType" AS src
WHERE c."ProviderTypeId" = src."Id";

SELECT setval(
    pg_get_serial_sequence('glo."GloCredentialProviderType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloCredentialProviderType"), 1),
    true);

-- GloStateProvince (requires GloCountry; no CreatedOn/CreatedBy columns)
INSERT INTO glo."GloStateProvince"
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
    FROM glo."GloStateProvince" t
    WHERE t."CountryCode" = v."CountryCode"
      AND t."StateProvinceCode" = v."StateProvinceCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloStateProvince"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloStateProvince"), 1),
    true);

-- GloPaymentMethodType (no CreatedOn/CreatedBy columns)
INSERT INTO glo."GloPaymentMethodType"
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
    FROM glo."GloPaymentMethodType" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloPaymentMethodType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloPaymentMethodType"), 1),
    true);

-- GloResolutionType
INSERT INTO glo."GloResolutionType"
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
    FROM glo."GloResolutionType" t
    WHERE t."ResolutionTypeCode" = v."ResolutionTypeCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloResolutionType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloResolutionType"), 1),
    true);

-- GloRole (global system roles)
INSERT INTO glo."GloRole"
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
    FROM glo."GloRole" t
    WHERE t."RoleCode" = v."RoleCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloRole"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloRole"), 1),
    true);

-- GloCommunicationTemplate (FSM-provided communication templates)
INSERT INTO glo."GloCommunicationTemplate"
(
    "TemplateScope",
    "CommunicationChannel",
    "TemplateCode",
    "Name",
    "Subject",
    "Body",
    "IsMobileVisible",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."TemplateScope",
    v."CommunicationChannel",
    v."TemplateCode",
    v."Name",
    v."Subject",
    v."Body",
    v."IsMobileVisible",
    v."DisplayOrder",
    v."IsActive",
    timezone('utc', now())
FROM (
    VALUES
        ('System', 'Email', 'COMPANY_ADMIN_INVITATION', 'Company Admin Invitation Email', 'Welcome to {{PlatformName}} – Activate Your Admin Account', E'Hello {{Name}},\n\nWelcome to {{PlatformName}}.\n\nYour company account has been created, and you have been assigned as the administrator for your organization.\n\nTo complete your setup and activate your administrator account, please click the link below:\n{{InviteLink}}\n\nDuring setup, you will be asked to:\n• Create or sign in to your account\n• Verify your email address\n\nThe invite link will expire in {{ExpirationHours}} hours.\n\nIf you did not expect this invitation or believe you received it in error, please ignore this email or contact support.\n\nThank you,\n{{CompanyName}}\n{{SupportEmail}}', false, 5::smallint, true),
        ('System', 'Email', 'USER_INVITATION', 'User Invitation', 'You have been invited to {{CompanyName}}', E'Hello {{UserName}},\n\nYou have been invited to join {{CompanyName}}.\n\nClick the link below to activate your account:\n\n{{ActivationLink}}', false, 10::smallint, true),
        ('System', 'Email', 'PASSWORD_RESET', 'Password Reset', 'Password Reset Request', E'Hello {{UserName}},\n\nA password reset was requested for your account.\n\nReset your password using the link below:\n\n{{ResetLink}}', false, 20::smallint, true),
        ('System', 'Email', 'EMAIL_VERIFICATION', 'Email Verification', 'Verify Your Email Address', E'Hello {{UserName}},\n\nPlease verify your email address using the link below:\n\n{{VerificationLink}}', false, 30::smallint, true),
        ('System', 'SystemNotification', 'ACCOUNT_LOCKED', 'Account Locked', NULL::text, 'Your account has been locked due to multiple failed login attempts.', false, 40::smallint, true),
        ('System', 'SystemNotification', 'MFA_CODE', 'Multi-Factor Authentication Code', NULL::text, 'Your verification code is {{VerificationCode}}.', false, 50::smallint, true),
        ('Tenant', 'Email', 'CUSTOMER_WELCOME', 'Customer Welcome', 'Welcome to {{CompanyName}}', E'Hello {{CustomerName}},\n\nThank you for choosing {{CompanyName}}.', false, 100::smallint, true),
        ('Tenant', 'Email', 'ESTIMATE_SENT', 'Estimate Sent', 'Estimate {{EstimateNumber}}', E'Hello {{CustomerName}},\n\nYour estimate {{EstimateNumber}} is ready.\n\nAmount: {{EstimateAmount}}\n\n{{EstimateLink}}', false, 110::smallint, true),
        ('Tenant', 'Email', 'ESTIMATE_APPROVED', 'Estimate Approved', 'Estimate Approved', 'Estimate {{EstimateNumber}} has been approved.', false, 120::smallint, true),
        ('Tenant', 'Email', 'INVOICE_SENT', 'Invoice Sent', 'Invoice {{InvoiceNumber}}', E'Hello {{CustomerName}},\n\nInvoice {{InvoiceNumber}} is ready.\n\nAmount Due: {{InvoiceAmount}}\n\n{{PaymentLink}}', false, 130::smallint, true),
        ('Tenant', 'Email', 'PAYMENT_RECEIVED', 'Payment Received', 'Payment Receipt', E'Payment of {{PaymentAmount}} has been received.\n\nInvoice: {{InvoiceNumber}}\n\nThank you.', false, 140::smallint, true),
        ('Tenant', 'Email', 'PAST_DUE_NOTICE', 'Past Due Notice', 'Past Due Invoice {{InvoiceNumber}}', E'Invoice {{InvoiceNumber}} is now past due.\n\nBalance Due: {{BalanceDue}}', false, 150::smallint, true),
        ('Tenant', 'Email', 'WORKORDER_CREATED', 'Work Order Created', 'Work Order {{WorkOrderNumber}} Created', 'Your work order has been scheduled for {{ScheduledDate}}.', true, 160::smallint, true),
        ('Tenant', 'Email', 'WORKORDER_COMPLETED', 'Work Order Completed', 'Work Order Completed', 'Your work order {{WorkOrderNumber}} has been completed.', true, 170::smallint, true),
        ('Tenant', 'Email', 'APPOINTMENT_REMINDER', 'Appointment Reminder', 'Upcoming Appointment Reminder', 'Reminder: Your appointment is scheduled for {{AppointmentDate}} at {{AppointmentTime}}.', true, 180::smallint, true),
        ('Tenant', 'SMS', 'APPOINTMENT_REMINDER', 'Appointment Reminder SMS', NULL::text, 'Reminder: Appointment on {{AppointmentDate}} at {{AppointmentTime}}.', true, 200::smallint, true),
        ('Tenant', 'SMS', 'TECHNICIAN_EN_ROUTE', 'Technician En Route', NULL::text, '{{TechnicianName}} is on the way for work order {{WorkOrderNumber}}.', true, 210::smallint, true),
        ('Tenant', 'SMS', 'TECHNICIAN_ARRIVED', 'Technician Arrived', NULL::text, '{{TechnicianName}} has arrived.', true, 220::smallint, true),
        ('Tenant', 'SMS', 'INVOICE_SENT', 'Invoice Sent SMS', NULL::text, 'Invoice {{InvoiceNumber}} for {{InvoiceAmount}} is ready. {{PaymentLink}}', true, 230::smallint, true),
        ('Tenant', 'SMS', 'PAYMENT_RECEIVED', 'Payment Received SMS', NULL::text, 'Payment of {{PaymentAmount}} received. Thank you.', true, 240::smallint, true),
        ('Tenant', 'PushNotification', 'WORKORDER_ASSIGNED', 'Work Order Assigned', NULL::text, 'You have been assigned work order {{WorkOrderNumber}}.', true, 300::smallint, true),
        ('Tenant', 'PushNotification', 'WORKORDER_COMPLETED', 'Work Order Completed', NULL::text, 'Work order {{WorkOrderNumber}} completed.', true, 310::smallint, true),
        ('Tenant', 'PushNotification', 'APPOINTMENT_REMINDER', 'Appointment Reminder', NULL::text, 'Upcoming appointment at {{AppointmentTime}}.', true, 320::smallint, true),
        ('Tenant', 'SystemNotification', 'ESTIMATE_APPROVED', 'Estimate Approved', NULL::text, 'Estimate {{EstimateNumber}} approved.', true, 400::smallint, true),
        ('Tenant', 'SystemNotification', 'PAYMENT_RECEIVED', 'Payment Received', NULL::text, 'Payment of {{PaymentAmount}} received.', true, 410::smallint, true),
        ('Tenant', 'SystemNotification', 'WORKORDER_COMPLETED', 'Work Order Completed', NULL::text, 'Work order {{WorkOrderNumber}} completed.', true, 420::smallint, true)
) AS v("TemplateScope", "CommunicationChannel", "TemplateCode", "Name", "Subject", "Body", "IsMobileVisible", "DisplayOrder", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloCommunicationTemplate" t
    WHERE t."CommunicationChannel" = v."CommunicationChannel"
      AND t."TemplateCode" = v."TemplateCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloCommunicationTemplate"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloCommunicationTemplate"), 1),
    true);

-- GloSetupDescriptionType
INSERT INTO glo."GloSetupDescriptionType"
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
    FROM glo."GloSetupDescriptionType" t
    WHERE t."Code" = v."Code"
);

-- GloSetupLaborRateType
INSERT INTO glo."GloSetupLaborRateType"
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
    FROM glo."GloSetupLaborRateType" t
    WHERE t."Name" = v."Name"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloSetupLaborRateType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloSetupLaborRateType"), 1),
    true);

-- GloSetupPaymentTerm
INSERT INTO glo."GloSetupPaymentTerm"
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
    FROM glo."GloSetupPaymentTerm" t
    WHERE t."Name" = v."Name"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloSetupPaymentTerm"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloSetupPaymentTerm"), 1),
    true);

-- GloSetupTenantStatus (Id 1 = default FK on FgsTenant)
INSERT INTO glo."GloSetupTenantStatus" ("Id", "Name", "Description", "IsActive", "CreatedOn")
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
    SELECT 1 FROM glo."GloSetupTenantStatus" t WHERE t."Id" = v."Id"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloSetupTenantStatus"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloSetupTenantStatus"), 1),
    true);

-- GloTrade
INSERT INTO glo."GloTrade"
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
INNER JOIN glo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloTrade" t
    WHERE t."TradeCode" = v."TradeCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloTrade"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloTrade"), 1),
    true);

-- GloSkill (HVAC, Plumbing, Electrical)
INSERT INTO glo."GloSkill"
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
INNER JOIN glo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
INNER JOIN glo."GloTrade" tr ON tr."TradeCode" = v."TradeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloSkill" s
    WHERE s."SkillCode" = v."SkillCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloSkill"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloSkill"), 1),
    true);

-- GloZone
INSERT INTO glo."GloZone"
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
    FROM glo."GloZone" z
    WHERE z."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloZone"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloZone"), 1),
    true);

-- GloJobTypeSubCategory
INSERT INTO glo."GloJobTypeSubCategory"
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
    FROM glo."GloJobTypeSubCategory" sc
    WHERE sc."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloJobTypeSubCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloJobTypeSubCategory"), 1),
    true);

-- GloJobTypeCategory
INSERT INTO glo."GloJobTypeCategory"
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
INNER JOIN glo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloJobTypeCategory" c
    WHERE c."BusinessTypeId" = bt."Id"
      AND c."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloJobTypeCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloJobTypeCategory"), 1),
    true);

-- GloInventoryItemType
INSERT INTO glo."GloInventoryItemType"
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
    FROM glo."GloInventoryItemType" t
    WHERE t."ItemTypeCode" = v."ItemTypeCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloInventoryItemType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloInventoryItemType"), 1),
    true);

-- GloInventoryCategory
INSERT INTO glo."GloInventoryCategory"
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
INNER JOIN glo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloInventoryCategory" c
    WHERE c."BusinessTypeId" = bt."Id"
      AND c."CategoryCode" = v."CategoryCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloInventoryCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloInventoryCategory"), 1),
    true);

-- GloInventorySubCategory
INSERT INTO glo."GloInventorySubCategory"
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
INNER JOIN glo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
INNER JOIN glo."GloInventoryCategory" c
    ON c."BusinessTypeId" = bt."Id"
   AND c."CategoryCode" = v."CategoryCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloInventorySubCategory" sc
    WHERE sc."InventoryCategoryId" = c."Id"
      AND sc."SubCategoryCode" = v."SubCategoryCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloInventorySubCategory"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloInventorySubCategory"), 1),
    true);

-- GloLeadSource (global lead acquisition sources for tenant provisioning seed)
INSERT INTO glo."GloLeadSource"
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
    FROM glo."GloLeadSource" ls
    WHERE ls."SourceCode" = v."SourceCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloLeadSource"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloLeadSource"), 1),
    true);

-- GloLeadStatus (global lead pipeline statuses for tenant provisioning seed)
INSERT INTO glo."GloLeadStatus"
(
    "StatusCode",
    "StatusName",
    "Description",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."StatusCode",
    v."StatusName",
    v."Description",
    v."DisplayOrder",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('NEW',          'New',          'Lead has been created.', 1::smallint),
        ('CONTACTED',    'Contacted',    'Initial contact has been made.', 2::smallint),
        ('QUALIFIED',    'Qualified',    'Lead meets qualification criteria.', 3::smallint),
        ('CONVERTED',    'Converted',    'Lead converted to customer and/or opportunity.', 4::smallint),
        ('DISQUALIFIED', 'Disqualified', 'Lead is not a viable sales opportunity.', 5::smallint)
) AS v("StatusCode", "StatusName", "Description", "DisplayOrder")
WHERE NOT EXISTS (
    SELECT 1 FROM glo."GloLeadStatus" ls WHERE ls."StatusCode" = v."StatusCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloLeadStatus"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloLeadStatus"), 1),
    true);

-- GloLeadDisqualificationReason (global disqualification reasons for tenant provisioning seed)
INSERT INTO glo."GloLeadDisqualificationReason"
(
    "ReasonCode",
    "ReasonName",
    "Description",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."ReasonCode",
    v."ReasonName",
    v."Description",
    v."DisplayOrder",
    true,
    timezone('utc', now())
FROM (
    VALUES
        ('OUTSIDE_SERVICE_AREA', 'Outside Service Area',           'Service location is outside the company service area.', 1::smallint),
        ('DUPLICATE',            'Duplicate Lead',                 'Lead already exists in the system.', 2::smallint),
        ('NO_RESPONSE',          'Unable To Contact',              'Multiple contact attempts were unsuccessful.', 3::smallint),
        ('NO_BUDGET',            'No Budget',                      'Prospect does not have budget available for the requested service.', 4::smallint),
        ('COMPETITOR',           'Competitor Selected',            'Prospect selected a competing provider.', 5::smallint),
        ('NOT_INTERESTED',       'Not Interested',                 'Prospect is no longer interested in the service.', 6::smallint),
        ('INVALID_CONTACT',      'Invalid Contact Information',    'Provided phone number, email, or contact information is invalid.', 7::smallint),
        ('PROJECT_CANCELLED',    'Project Cancelled',              'Prospect cancelled or postponed the project indefinitely.', 8::smallint),
        ('NO_DECISION_MAKER',    'No Decision Maker',              'Unable to reach or engage the decision maker.', 9::smallint),
        ('OTHER',                'Other',                          'Other disqualification reason not covered by existing categories.', 10::smallint)
) AS v("ReasonCode", "ReasonName", "Description", "DisplayOrder")
WHERE NOT EXISTS (
    SELECT 1 FROM glo."GloLeadDisqualificationReason" r WHERE r."ReasonCode" = v."ReasonCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloLeadDisqualificationReason"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloLeadDisqualificationReason"), 1),
    true);

-- GloUnitOfMeasure (global units of measure)
INSERT INTO glo."GloUnitOfMeasure"
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
    FROM glo."GloUnitOfMeasure" u
    WHERE u."UnitCode" = v."UnitCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloUnitOfMeasure"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloUnitOfMeasure"), 1),
    true);

-- GloTag (global system tags)
INSERT INTO glo."GloTag"
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
    FROM glo."GloTag" t
    WHERE t."TagCode" = v."TagCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloTag"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloTag"), 1),
    true);

-- GloTitleOfCourtesy (global courtesy titles)
INSERT INTO glo."GloTitleOfCourtesy"
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
    FROM glo."GloTitleOfCourtesy" t
    WHERE t."Code" = v."Code"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloTitleOfCourtesy"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloTitleOfCourtesy"), 1),
    true);

-- =============================================================================
-- GloSeedTableMapping / GloSeedTableColumnMapping
-- Tenant provisioning: global (Glo*) -> tenant/company (Fgs*) catalog copies
-- Idempotent: matched by SeedCode (table) and TargetColumnName (column)
-- Transformation types: TENANT_ID, COMPANY_ID, STATIC, CURRENT_TIMESTAMP, SEED_CREATED_BY
-- =============================================================================

INSERT INTO glo."GloSeedTableMapping"
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
    'System'
FROM (
    VALUES
        ('TENANT_FgsTenantCompany_setup_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'setup', 'FgsTenantCompanyCache', 1, 'Tenant company cache (setup)', true),
        ('TENANT_FgsTenantCompany_identity_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'identity', 'FgsTenantCompanyCache', 2, 'Tenant company cache (identity)', true),
        ('TENANT_FgsTenantCompany_billing_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'billing', 'FgsTenantCompanyCache', 3, 'Tenant company cache (billing)', true),
        ('TENANT_FgsTenantCompany_crm_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'crm', 'FgsTenantCompanyCache', 4, 'Tenant company cache (crm)', true),
        ('TENANT_FgsTenantCompany_dispatch_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'dispatch', 'FgsTenantCompanyCache', 5, 'Tenant company cache (dispatch)', true),
        ('TENANT_FgsTenantCompany_inventory_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'inventory', 'FgsTenantCompanyCache', 6, 'Tenant company cache (inventory)', true),
        ('TENANT_FgsTenantCompany_notification_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'notification', 'FgsTenantCompanyCache', 7, 'Tenant company cache (notification)', true),
        ('TENANT_FgsTenantCompany_reporting_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'reporting', 'FgsTenantCompanyCache', 8, 'Tenant company cache (reporting)', true),
        ('TENANT_FgsTenantCompany_integration_cache', 'fgs_dev_db', 'tenant', 'FgsTenantCompany', 'fgs_dev_db', 'integration', 'FgsTenantCompanyCache', 9, 'Tenant company cache (integration)', true),
        ('ALL_GloBillingCategory', 'fgs_dev_db', 'glo', 'GloBillingCategory', 'fgs_dev_db', 'setup', 'FgsBillingCategory', 100, 'Billing Category', true),
        ('ALL_GloJobTypeCategory', 'fgs_dev_db', 'glo', 'GloJobTypeCategory', 'fgs_dev_db', 'setup', 'FgsJobTypeCategory', 130, 'JobType Categories', true),
        ('ALL_GloJobTypeSubCategory', 'fgs_dev_db', 'glo', 'GloJobTypeSubCategory', 'fgs_dev_db', 'setup', 'FgsJobTypeSubCategory', 160, 'JobType Sub Categories', true),
        ('ALL_GloLeadSource', 'fgs_dev_db', 'glo', 'GloLeadSource', 'fgs_dev_db', 'setup', 'FgsLeadSource', 190, 'Lead Source', true),
        ('ALL_GloLeadStatus', 'fgs_dev_db', 'glo', 'GloLeadStatus', 'fgs_dev_db', 'setup', 'FgsLeadStatus', 195, 'Lead Status', true),
        ('ALL_GloLeadDisqualificationReason', 'fgs_dev_db', 'glo', 'GloLeadDisqualificationReason', 'fgs_dev_db', 'setup', 'FgsLeadDisqualificationReason', 198, 'Lead Disqualification Reason', true),
        ('ALL_GloPaymentMethodType', 'fgs_dev_db', 'glo', 'GloPaymentMethodType', 'fgs_dev_db', 'setup', 'FgsSetupPaymentMethod', 220, 'Payment Method', true),
        ('ALL_GloResolutionType', 'fgs_dev_db', 'glo', 'GloResolutionType', 'fgs_dev_db', 'setup', 'FgsResolutionCode', 250, 'Resolution Code', true),
        ('ALL_GloSetupLaborRateType', 'fgs_dev_db', 'glo', 'GloSetupLaborRateType', 'fgs_dev_db', 'setup', 'FgsSetupLaborRateType', 280, 'Labor Rate Type', true),
        ('GloSkill', 'fgs_dev_db', 'glo', 'GloSkill', 'fgs_dev_db', 'setup', 'FgsSetupTechSkillLevel', 310, 'Technician Skill', true),
        ('ALL_GloTag', 'fgs_dev_db', 'glo', 'GloTag', 'fgs_dev_db', 'setup', 'FgsTag', 340, 'Tags', true),
        ('GloTrade', 'fgs_dev_db', 'glo', 'GloTrade', 'fgs_dev_db', 'setup', 'FgsSetupTechTrade', 410, 'Technician Trade', true),
        ('ALL_GloTitleOfCourtesy', 'fgs_dev_db', 'glo', 'GloTitleOfCourtesy', 'fgs_dev_db', 'setup', 'FgsSetupTitleOfCourtesy', 440, 'Title Of Courtesy', true),
        ('ALL_GloZone', 'fgs_dev_db', 'glo', 'GloZone', 'fgs_dev_db', 'setup', 'FgsSetupZone', 470, 'Zone', true),
        ('ALL_GloSetupPaymentTerm', 'fgs_dev_db', 'glo', 'GloSetupPaymentTerm', 'fgs_dev_db', 'setup', 'FgsSetupPaymentTerm', 500, 'Payment Term', true)
) AS v("SeedCode", "SourceDatabaseName", "SourceSchemaName", "SourceTableName", "TargetDatabaseName", "TargetSchemaName", "TargetTableName", "SeedOrder", "Description", "IsActive")
WHERE NOT EXISTS (
    SELECT 1 FROM glo."GloSeedTableMapping" m WHERE m."SeedCode" = v."SeedCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloSeedTableMapping"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloSeedTableMapping"), 1),
    true);

UPDATE glo."GloSeedTableMapping"
SET "TargetSchemaName" = 'setup'
WHERE "SeedCode" = 'ALL_GloTag'
  AND "TargetSchemaName" = 'shared';

INSERT INTO glo."GloSeedTableColumnMapping"
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
    'System'
FROM glo."GloSeedTableMapping" m
INNER JOIN (
    VALUES
        -- TENANT_FgsTenantCompany -> setup.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_setup_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_setup_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_setup_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_setup_cache', 'Code', 'Code', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_setup_cache', 'Name', 'Name', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_setup_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_setup_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> identity.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_identity_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_identity_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_identity_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_identity_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_identity_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_identity_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_identity_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> billing.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_billing_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_billing_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_billing_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_billing_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_billing_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_billing_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_billing_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> crm.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_crm_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_crm_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_crm_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_crm_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_crm_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_crm_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_crm_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> dispatch.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_dispatch_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_dispatch_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_dispatch_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_dispatch_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_dispatch_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_dispatch_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_dispatch_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> inventory.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_inventory_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_inventory_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_inventory_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_inventory_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_inventory_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_inventory_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_inventory_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> notification.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_notification_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_notification_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_notification_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_notification_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_notification_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_notification_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_notification_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> reporting.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_reporting_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_reporting_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_reporting_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_reporting_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_reporting_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_reporting_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_reporting_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

        -- TENANT_FgsTenantCompany -> integration.FgsTenantCompanyCache
        ('TENANT_FgsTenantCompany_integration_cache', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('TENANT_FgsTenantCompany_integration_cache', 'CompanyNumber', 'CompanyId', NULL, NULL, 2, true, true),
        ('TENANT_FgsTenantCompany_integration_cache', 'CompanyGuid', 'CompanyGuid', NULL, NULL, 3, true, true),
        ('TENANT_FgsTenantCompany_integration_cache', 'Code', 'CompanyCode', NULL, NULL, 4, true, true),
        ('TENANT_FgsTenantCompany_integration_cache', 'Name', 'CompanyName', NULL, NULL, 5, true, true),
        ('TENANT_FgsTenantCompany_integration_cache', 'IsActive', 'IsActive', NULL, NULL, 6, true, true),
        ('TENANT_FgsTenantCompany_integration_cache', NULL, 'UpdatedOn', 'CURRENT_TIMESTAMP', NULL, 7, true, true),

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

        -- ALL_GloLeadStatus -> FgsLeadStatus
        ('ALL_GloLeadStatus', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloLeadStatus', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloLeadStatus', 'StatusCode', 'StatusCode', NULL, NULL, 3, true, true),
        ('ALL_GloLeadStatus', 'StatusName', 'StatusName', NULL, NULL, 4, true, true),
        ('ALL_GloLeadStatus', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('ALL_GloLeadStatus', 'DisplayOrder', 'DisplayOrder', NULL, NULL, 6, true, true),
        ('ALL_GloLeadStatus', NULL, 'IsSystem', 'STATIC', 'true', 7, true, true),
        ('ALL_GloLeadStatus', 'IsActive', 'IsActive', NULL, NULL, 8, true, true),
        ('ALL_GloLeadStatus', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 9, true, true),
        ('ALL_GloLeadStatus', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 10, false, true),

        -- ALL_GloLeadDisqualificationReason -> FgsLeadDisqualificationReason
        ('ALL_GloLeadDisqualificationReason', NULL, 'TenantId', 'TENANT_ID', NULL, 1, true, true),
        ('ALL_GloLeadDisqualificationReason', NULL, 'CompanyId', 'COMPANY_ID', NULL, 2, true, true),
        ('ALL_GloLeadDisqualificationReason', 'ReasonCode', 'ReasonCode', NULL, NULL, 3, true, true),
        ('ALL_GloLeadDisqualificationReason', 'ReasonName', 'ReasonName', NULL, NULL, 4, true, true),
        ('ALL_GloLeadDisqualificationReason', 'Description', 'Description', NULL, NULL, 5, false, true),
        ('ALL_GloLeadDisqualificationReason', 'DisplayOrder', 'DisplayOrder', NULL, NULL, 6, true, true),
        ('ALL_GloLeadDisqualificationReason', NULL, 'IsSystem', 'STATIC', 'true', 7, true, true),
        ('ALL_GloLeadDisqualificationReason', 'IsActive', 'IsActive', NULL, NULL, 8, true, true),
        ('ALL_GloLeadDisqualificationReason', NULL, 'CreatedOn', 'CURRENT_TIMESTAMP', NULL, 9, true, true),
        ('ALL_GloLeadDisqualificationReason', NULL, 'CreatedBy', 'SEED_CREATED_BY', NULL, 10, false, true),

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
    FROM glo."GloSeedTableColumnMapping" existing
    WHERE existing."SeedTableMappingId" = m."Id"
      AND existing."TargetColumnName" = c."TargetColumnName"
);

UPDATE glo."GloSeedTableColumnMapping" AS existing
SET
    "SourceColumnName" = 'ShowToFieldTech',
    "TransformationType" = NULL,
    "StaticValue" = NULL,
    "ColumnOrder" = 8,
    "IsRequired" = true,
    "IsActive" = true
FROM glo."GloSeedTableMapping" m
WHERE existing."SeedTableMappingId" = m."Id"
  AND m."SeedCode" = 'ALL_GloBillingCategory'
  AND existing."TargetColumnName" = 'ShowToFieldTech'
  AND existing."TransformationType" = 'STATIC';

SELECT setval(
    pg_get_serial_sequence('glo."GloSeedTableColumnMapping"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloSeedTableColumnMapping"), 1),
    true);

COMMIT;
