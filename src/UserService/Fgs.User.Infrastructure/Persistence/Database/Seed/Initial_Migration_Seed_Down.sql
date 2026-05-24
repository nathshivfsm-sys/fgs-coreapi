-- =============================================================================
-- Revert: CleanUpTables global reference data seed
-- Pair with: Initial_Migration_Seed.sql
-- =============================================================================

START TRANSACTION;

DELETE FROM dbo."GloStateProvince"
WHERE ("CountryCode", "StateProvinceCode") IN (
    SELECT v."CountryCode", v."StateProvinceCode"
    FROM (
        VALUES
            ('US', 'AL'), ('US', 'AK'), ('US', 'AZ'), ('US', 'AR'), ('US', 'CA'),
            ('US', 'CO'), ('US', 'CT'), ('US', 'DE'), ('US', 'FL'), ('US', 'GA'),
            ('US', 'HI'), ('US', 'ID'), ('US', 'IL'), ('US', 'IN'), ('US', 'IA'),
            ('US', 'KS'), ('US', 'KY'), ('US', 'LA'), ('US', 'ME'), ('US', 'MD'),
            ('US', 'MA'), ('US', 'MI'), ('US', 'MN'), ('US', 'MS'), ('US', 'MO'),
            ('US', 'MT'), ('US', 'NE'), ('US', 'NV'), ('US', 'NH'), ('US', 'NJ'),
            ('US', 'NM'), ('US', 'NY'), ('US', 'NC'), ('US', 'ND'), ('US', 'OH'),
            ('US', 'OK'), ('US', 'OR'), ('US', 'PA'), ('US', 'RI'), ('US', 'SC'),
            ('US', 'SD'), ('US', 'TN'), ('US', 'TX'), ('US', 'UT'), ('US', 'VT'),
            ('US', 'VA'), ('US', 'WA'), ('US', 'WV'), ('US', 'WI'), ('US', 'WY'),
            ('US', 'DC'),
            ('CA', 'AB'), ('CA', 'BC'), ('CA', 'MB'), ('CA', 'NB'), ('CA', 'NL'),
            ('CA', 'NS'), ('CA', 'ON'), ('CA', 'PE'), ('CA', 'QC'), ('CA', 'SK'),
            ('CA', 'NT'), ('CA', 'NU'), ('CA', 'YT')
    ) AS v("CountryCode", "StateProvinceCode")
);

DELETE FROM dbo."GloCredentialProviderType"
WHERE "Code" IN (
    'AWS',
    'AZURE',
    'TWILIO',
    'STRIPE',
    'PAYPAL',
    'QUICKBOOKS',
    'SHOPIFY',
    'HUBSPOT',
    'MAILCHIMP',
    'SENDGRID',
    'GOOGLE',
    'MICROSOFT',
    'META',
    'DOCUSIGN',
    'CUSTOM',
    'OTHER'
);

DELETE FROM dbo."GloCredentialCategory"
WHERE "Code" IN (
    'API_KEY',
    'OAUTH',
    'DATABASE',
    'SMTP',
    'AWS',
    'AZURE',
    'PAYMENT_GATEWAY',
    'TWILIO',
    'STRIPE',
    'QUICKBOOKS',
    'SERVICE_ACCOUNT',
    'SSH',
    'ENCRYPTION',
    'WEBHOOK'
);

DELETE FROM dbo."GloCountry"
WHERE "CountryCode" IN (
    'US',
    'CA'
);

DELETE FROM dbo."GloBillingCategory"
WHERE "BillingCategoryType" IN (
    'EQ',
    'MT',
    'LB',
    'SB',
    'SF',
    'SH',
    'TX',
    'DS',
    'OT'
);

DELETE FROM dbo."GloSetupLaborRateType"
WHERE "Name" IN (
    'Regular',
    'Overtime',
    'Double-Time',
    'Holiday',
    'Weekend'
);

DELETE FROM dbo."GloSetupDescriptionType"
WHERE "Code" IN (
    'ReasonForCall',
    'Recommendations',
    'WorkSummary',
    'AgreementDescription'
);

DELETE FROM dbo."GloRole"
WHERE "RoleCode" IN (
    'SYSTEM_ADMIN',
    'IMPLEMENTATION_SPECIALIST',
    'SUPPORT_AGENT',
    'BILLING_ADMIN',
    'SALES_ADMIN',
    'READONLY_AUDITOR',
    'TENANT_ADMIN',
    'COMPANY_ADMIN',
    'OPERATIONS_MANAGER',
    'DISPATCHER',
    'BILLING',
    'CSR',
    'OFFICE_USER',
    'SERVICE_MANAGER',
    'FIELD_SUPERVISOR',
    'FIELD_TECH'
);

DELETE FROM dbo."GloResolutionType"
WHERE "ResolutionTypeCode" IN (
    'COMPLETED',
    'INCOMPLETE',
    'PART_REQUIRED',
    'PARTS_ARRIVED',
    'CANCELLED'
);

DELETE FROM dbo."GloPaymentMethodType"
WHERE "Code" IN (
    'CASH',
    'CHECK',
    'CREDIT_CARD',
    'DEBIT_CARD',
    'ACH',
    'APPLE_PAY',
    'GOOGLE_PAY',
    'ZELLE'
);

DELETE FROM dbo."GloJobTypeCategorySubCategory"
WHERE "BusinessTypeId" IN (
    SELECT "Id" FROM dbo."GloBusinessType"
    WHERE "Code" IN ('HVAC', 'PLUMBING', 'ELECTRICAL')
);

DELETE FROM dbo."GloSkill"
WHERE "SkillCode" IN (
    'HVACEXPERT',
    'HVACHELPER',
    'PLUMBINGEXPERT',
    'PLUMBINGHELPER',
    'ELECTRICALEXPERT',
    'ELECTRICALHELPER'
);

DELETE FROM dbo."GloJobTypeCategory"
WHERE ("BusinessTypeId", "Code") IN (
    SELECT bt."Id", v."Code"
    FROM (
        VALUES
            ('HVAC',       'AC'),
            ('HVAC',       'FURNACE'),
            ('HVAC',       'THERMOSTAT'),
            ('PLUMBING',   'TOILET'),
            ('PLUMBING',   'FAUCET'),
            ('PLUMBING',   'WATERHEATER'),
            ('ELECTRICAL', 'PANEL'),
            ('ELECTRICAL', 'LIGHTING'),
            ('ELECTRICAL', 'OUTLET')
    ) AS v("BusinessTypeCode", "Code")
    INNER JOIN dbo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
);

DELETE FROM dbo."GloJobTypeSubCategory"
WHERE "Code" IN (
    'INSTALL',
    'REPAIR',
    'SERVICE',
    'REPLACE',
    'INSPECT',
    'MAINTENANCE',
    'TROUBLESHOOT',
    'CLEANING',
    'TUNEUP',
    'UPGRADE'
);

DELETE FROM dbo."GloTrade"
WHERE "TradeCode" IN (
    'PESTCONTROL',
    'GARAGEDOOR',
    'LAWNCARE',
    'IRRIGATION',
    'LANDSCAPING',
    'HOUSECLEANING',
    'TRASHREMOVAL',
    'JUNKREMOVAL',
    'ELECTRICAL',
    'PLUMBING',
    'HVAC',
    'PAINTING'
);

DELETE FROM dbo."GloZone"
WHERE "Code" IN ('ALL');

DELETE FROM dbo."GloBusinessType"
WHERE "Code" IN (
    'HVAC',
    'PLUMBING',
    'ELECTRICAL',
    'PESTCONTROL',
    'LAWNCARE',
    'TRASHPICKUP',
    'GARAGEDOOR',
    'HOUSECLEANING',
    'PAINTING',
    'CARPETCLEANING',
    'WINDOWCLEANING',
    'HOLIDAYLIGHTING',
    'OTHER'
);

DELETE FROM dbo."GloTimeCardOption"
WHERE "Code" IN (
    'NONE',
    'DISPATCHARRIVECOMPLETE',
    'CHECKINCHECKOUT'
);

DELETE FROM dbo."GloAccountingIntegrationType"
WHERE "Code" IN (
    'NONE',
    'QUICKBOOKSONLINE',
    'SAGEINTACCT'
);

DELETE FROM dbo."GloLanguage"
WHERE "LanguageCode" IN (
    'EN',
    'ES',
    'FR'
);

DELETE FROM dbo."GloMasterEntityType"
WHERE "Code" IN (
    'TENANT',
    'COMPANY',
    'SERVICELOCATION',
    'BILLTO',
    'VENDOR',
    'SUBCONTRACTOR',
    'LEAD',
    'PROPOSAL',
    'CUSTOMER',
    'WORKORDER',
    'INVOICE',
    -- legacy codes from prior seed (safe if never inserted)
    'TENANT_COMPANY',
    'WORK_ORDER',
    'EMPLOYEE',
    'PURCHASE_ORDER',
    'SUB_CONTRACTOR',
    'BILL_TO',
    'SERVICE_LOCATION'
);

DELETE FROM dbo."GloLocationType"
WHERE "Code" IN (
    'BILLING',
    'SHIPPING',
    'PHYSICAL',
    'SERVICE',
    'MAILING',
    'HQ',
    'REMITTO',
    'JOBSITE'
);

DELETE FROM dbo."GloSeedTableColumnMapping"
WHERE "SeedTableMappingId" IN (
    SELECT "Id"
    FROM dbo."GloSeedTableMapping"
    WHERE "SeedCode" IN (
        'GLO_ZONE_TO_FGS_SETUP_ZONE',
        'GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',
        'GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',
        'GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',
        'GLO_ROLE_TO_FGS_ROLE',
        'GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY',
        'GLO_TAG_TO_FGS_TAG',
        'GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE'
    )
);

DELETE FROM dbo."GloSeedTableMapping"
WHERE "SeedCode" IN (
    'GLO_ZONE_TO_FGS_SETUP_ZONE',
    'GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',
    'GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',
    'GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',
    'GLO_ROLE_TO_FGS_ROLE',
    'GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY',
    'GLO_TAG_TO_FGS_TAG',
    'GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE'
);

DELETE FROM dbo."GloLeadSource"
WHERE "SourceCode" IN (
    'REFERRAL',
    'WEBSITE',
    'GOOGLE',
    'FACEBOOK',
    'YELP',
    'PHONE',
    'DIRECT',
    'OTHER'
);

DELETE FROM dbo."GloTitleOfCourtesy"
WHERE "Code" IN (
    'MR',
    'MRS',
    'MS',
    'MISS',
    'DR',
    'PROF',
    'REV'
);

DELETE FROM dbo."GloTag"
WHERE "TagCode" IN (
    'URGENT',
    'VIP',
    'WARRANTY',
    'FOLLOWUP',
    'COMMERCIAL',
    'INSPECTION'
);

DELETE FROM dbo."GloUnitOfMeasure"
WHERE "UnitCode" IN (
    'EACH',
    'BOX',
    'CASE',
    'FOOT',
    'INCH',
    'POUND',
    'GALLON',
    'HOUR',
    'DAY',
    'ROLL'
);

COMMIT;
