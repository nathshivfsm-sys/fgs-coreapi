START TRANSACTION;
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
                {"key":"FgsUserReadOnly","label":"User Service Read-Only (FgsUserReadOnly)","type":"password","required":false,"sensitive":true},
                {"key":"FgsSetup","label":"Setup Service (FgsSetup)","type":"password","required":false,"sensitive":true},
                {"key":"FgsSetupReadOnly","label":"Setup Service Read-Only (FgsSetupReadOnly)","type":"password","required":false,"sensitive":true},
                {"key":"FgsFile","label":"File Service (FgsFile)","type":"password","required":false,"sensitive":true},
                {"key":"FgsNotification","label":"Notification Service (FgsNotification)","type":"password","required":false,"sensitive":true},
                {"key":"FgsConsumer","label":"Consumer Service (FgsConsumer)","type":"password","required":false,"sensitive":true},
                {"key":"FgsAudit","label":"Audit Service (FgsAudit)","type":"password","required":false,"sensitive":true},
                {"key":"FgsAsset","label":"Asset Service (FgsAsset)","type":"password","required":false,"sensitive":true},
                {"key":"FgsBilling","label":"Billing Service (FgsBilling)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCommunication","label":"Communication Service (FgsCommunication)","type":"password","required":false,"sensitive":true},
                {"key":"FgsServiceAgreement","label":"Service Agreement Service (FgsServiceAgreement)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCrm","label":"CRM Service (FgsCrm)","type":"password","required":false,"sensitive":true},
                {"key":"FgsDispatch","label":"Scheduling Service (FgsDispatch)","type":"password","required":false,"sensitive":true},
                {"key":"FgsIntegration","label":"Integration Service (FgsIntegration)","type":"password","required":false,"sensitive":true},
                {"key":"FgsInventory","label":"Inventory Service (FgsInventory)","type":"password","required":false,"sensitive":true},
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
            'REDIS',
            'Redis Cache',
            '[
                {"key":"Enabled","label":"Enabled","type":"boolean","required":true},
                {"key":"ConnectionString","label":"Connection String","type":"text","required":true},
                {"key":"InstanceName","label":"Instance Name Prefix","type":"text","required":false},
                {"key":"DefaultAbsoluteExpirationMinutes","label":"Default Cache TTL (minutes)","type":"number","required":false}
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
                {"key":"FgsUserReadOnly","label":"User Service Read-Only (FgsUserReadOnly)","type":"password","required":false,"sensitive":true},
                {"key":"FgsSetup","label":"Setup Service (FgsSetup)","type":"password","required":false,"sensitive":true},
                {"key":"FgsSetupReadOnly","label":"Setup Service Read-Only (FgsSetupReadOnly)","type":"password","required":false,"sensitive":true},
                {"key":"FgsFile","label":"File Service (FgsFile)","type":"password","required":false,"sensitive":true},
                {"key":"FgsNotification","label":"Notification Service (FgsNotification)","type":"password","required":false,"sensitive":true},
                {"key":"FgsConsumer","label":"Consumer Service (FgsConsumer)","type":"password","required":false,"sensitive":true},
                {"key":"FgsAudit","label":"Audit Service (FgsAudit)","type":"password","required":false,"sensitive":true},
                {"key":"FgsAsset","label":"Asset Service (FgsAsset)","type":"password","required":false,"sensitive":true},
                {"key":"FgsBilling","label":"Billing Service (FgsBilling)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCommunication","label":"Communication Service (FgsCommunication)","type":"password","required":false,"sensitive":true},
                {"key":"FgsServiceAgreement","label":"Service Agreement Service (FgsServiceAgreement)","type":"password","required":false,"sensitive":true},
                {"key":"FgsCrm","label":"CRM Service (FgsCrm)","type":"password","required":false,"sensitive":true},
                {"key":"FgsDispatch","label":"Scheduling Service (FgsDispatch)","type":"password","required":false,"sensitive":true},
                {"key":"FgsIntegration","label":"Integration Service (FgsIntegration)","type":"password","required":false,"sensitive":true},
                {"key":"FgsInventory","label":"Inventory Service (FgsInventory)","type":"password","required":false,"sensitive":true},
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
            'REDIS',
            'Redis Cache',
            '[
                {"key":"Enabled","label":"Enabled","type":"boolean","required":true},
                {"key":"ConnectionString","label":"Connection String","type":"text","required":true},
                {"key":"InstanceName","label":"Instance Name Prefix","type":"text","required":false},
                {"key":"DefaultAbsoluteExpirationMinutes","label":"Default Cache TTL (minutes)","type":"number","required":false}
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


INSERT INTO setup."GloCredentialProviderTypeCache"
(
    "ProviderTypeId",
    "ProviderCode",
    "ProviderName",
    "ConfigurationSchema",
    "IsActive",
    "UpdatedOn"
)
SELECT
    src."Id",
    src."ProviderCode",
    src."ProviderName",
    src."ConfigurationSchema",
    src."IsActive",
    timezone('utc', now())
FROM glo."GloCredentialProviderType" src
WHERE NOT EXISTS (
    SELECT 1
    FROM setup."GloCredentialProviderTypeCache" c
    WHERE c."ProviderTypeId" = src."Id");
COMMIT;

