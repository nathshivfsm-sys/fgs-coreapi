-- =============================================================================
-- Revert: CleanUpTables global reference data seed
-- Pair with: Initial_Migration_Seed.sql
-- =============================================================================

START TRANSACTION;

DELETE FROM glo."GloStateProvince"
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

DELETE FROM glo."GloCredentialProviderType"
WHERE "ProviderCode" IN ('RABBITMQ', 'AWS', 'ENTRA_EXTERNAL_ID', 'SENDGRID');

DELETE FROM glo."GloCountry"
WHERE "CountryCode" IN (
    'US',
    'CA'
);

DELETE FROM glo."GloBillingCategory"
WHERE "BillingCategoryType" IN (
    'DS',
    'IN',
    'LB',
    'NI',
    'OT',
    'SB',
    'SF',
    'SH',
    'TX'
);

DELETE FROM glo."GloSetupLaborRateType"
WHERE "Name" IN (
    'Regular',
    'Overtime',
    'Double-Time',
    'Holiday',
    'Weekend'
);

DELETE FROM glo."GloSetupPaymentTerm"
WHERE "Name" IN (
    'Net 15',
    'Net 30',
    'Net 45',
    'End Of Month',
    'COD'
);

DELETE FROM glo."GloSetupDescriptionType"
WHERE "Code" IN (
    'ReasonForCall',
    'Recommendations',
    'WorkSummary',
    'AgreementDescription'
);

DELETE FROM glo."GloCommunicationTemplate"
WHERE ("CommunicationChannel", "TemplateCode") IN (
    ('Email', 'COMPANY_ADMIN_INVITATION'),
    ('Email', 'USER_INVITATION'),
    ('Email', 'PASSWORD_RESET'),
    ('Email', 'EMAIL_VERIFICATION'),
    ('SystemNotification', 'ACCOUNT_LOCKED'),
    ('SystemNotification', 'MFA_CODE'),
    ('Email', 'CUSTOMER_WELCOME'),
    ('Email', 'ESTIMATE_SENT'),
    ('Email', 'ESTIMATE_APPROVED'),
    ('Email', 'INVOICE_SENT'),
    ('Email', 'PAYMENT_RECEIVED'),
    ('Email', 'PAST_DUE_NOTICE'),
    ('Email', 'WORKORDER_CREATED'),
    ('Email', 'WORKORDER_COMPLETED'),
    ('Email', 'APPOINTMENT_REMINDER'),
    ('SMS', 'APPOINTMENT_REMINDER'),
    ('SMS', 'TECHNICIAN_EN_ROUTE'),
    ('SMS', 'TECHNICIAN_ARRIVED'),
    ('SMS', 'INVOICE_SENT'),
    ('SMS', 'PAYMENT_RECEIVED'),
    ('PushNotification', 'WORKORDER_ASSIGNED'),
    ('PushNotification', 'WORKORDER_COMPLETED'),
    ('PushNotification', 'APPOINTMENT_REMINDER'),
    ('SystemNotification', 'ESTIMATE_APPROVED'),
    ('SystemNotification', 'PAYMENT_RECEIVED'),
    ('SystemNotification', 'WORKORDER_COMPLETED')
);

DELETE FROM glo."GloRole"
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

DELETE FROM glo."GloResolutionType"
WHERE "ResolutionTypeCode" IN (
    'COMPLETED',
    'INCOMPLETE',
    'PART_REQUIRED',
    'PARTS_ARRIVED',
    'CANCELLED'
);

DELETE FROM glo."GloPaymentMethodType"
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

DELETE FROM glo."GloSkill"
WHERE "SkillCode" IN (
    'HVACEXPERT',
    'HVACHELPER',
    'PLUMBINGEXPERT',
    'PLUMBINGHELPER',
    'ELECTRICALEXPERT',
    'ELECTRICALHELPER'
);

DELETE FROM glo."GloJobTypeCategory"
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
    INNER JOIN glo."GloBusinessType" bt ON bt."Code" = v."BusinessTypeCode"
);

DELETE FROM glo."GloJobTypeSubCategory"
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

DELETE FROM glo."GloTrade"
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

DELETE FROM glo."GloZone"
WHERE "Code" IN ('ALL');

DELETE FROM glo."GloBusinessType"
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

DELETE FROM glo."GloTimeCardOption"
WHERE "Code" IN (
    'NONE',
    'DISPATCHARRIVECOMPLETE',
    'CHECKINCHECKOUT'
);

DELETE FROM glo."GloAccountingIntegrationType"
WHERE "Code" IN (
    'NONE',
    'QUICKBOOKSONLINE',
    'SAGEINTACCT'
);

DELETE FROM glo."GloLanguage"
WHERE "LanguageCode" IN (
    'EN',
    'ES',
    'FR'
);

DELETE FROM glo."GloMasterEntityType"
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
    'Warehouse',
    'Vehicle',
    'VehicleMaintenance',
    'Warehouse',
    'Vehicle',
    'VehicleMaintenance',
    -- legacy codes from prior seed (safe if never inserted)
    'TENANT_COMPANY',
    'WORK_ORDER',
    'EMPLOYEE',
    'PURCHASE_ORDER',
    'SUB_CONTRACTOR',
    'BILL_TO',
    'SERVICE_LOCATION'
);

DELETE FROM glo."GloVehicleMaintenanceType"
WHERE "MaintenanceTypeCode" IN (
    'OIL_CHANGE',
    'TIRE_ROTATION',
    'TIRE_REPLACEMENT',
    'BRAKE_SERVICE',
    'INSPECTION',
    'BATTERY_REPLACEMENT',
    'TRANSMISSION_SERVICE',
    'REGISTRATION_RENEWAL',
    'REPAIR',
    'OTHER'
);

DELETE FROM glo."GloLocationType"
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

DELETE FROM glo."GloSeedTableColumnMapping"
WHERE "SeedTableMappingId" IN (
    SELECT "Id"
    FROM glo."GloSeedTableMapping"
    WHERE "SeedCode" IN (
        'ALL_GloBillingCategory',
        'ALL_GloJobTypeCategory',
        'ALL_GloJobTypeSubCategory',
        'ALL_GloLeadSource',
        'ALL_GloEstimateFlavor',
        'ALL_GloEstimateStatus',
        'ALL_GloPaymentMethodType',
        'ALL_GloResolutionType',
        'ALL_GloSetupLaborRateType',
        'GloSkill',
        'ALL_GloTag',
        'GloTrade',
        'ALL_GloTitleOfCourtesy',
        'ALL_GloZone',
        'ALL_GloSetupPaymentTerm',
        'GLO_ZONE_TO_FGS_SETUP_ZONE',
        'GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',
        'GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',
        'GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',
        'GLO_ROLE_TO_FGS_ROLE',
        'GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY',
        'GLO_TAG_TO_FGS_TAG',
        'GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE',
        'GLO_INVENTORY_ITEM_TYPE_TO_FGS_INVENTORY_ITEM_TYPE',
        'GLO_INVENTORY_CATEGORY_TO_FGS_INVENTORY_CATEGORY',
        'GLO_BILLING_CATEGORY_TO_FGS_BILLING_CATEGORY',
        'GLO_BUSINESS_TYPE_TO_FGS_BUSINESS_TYPE',
        'GLO_SETUP_LABOR_RATE_TYPE_TO_FGS_SETUP_LABOR_RATE_TYPE',
        'GLO_SETUP_PAYMENT_TERM_TO_FGS_SETUP_PAYMENT_TERM'
    )
);

DELETE FROM glo."GloSeedTableMapping"
WHERE "SeedCode" IN (
    'ALL_GloBillingCategory',
    'ALL_GloJobTypeCategory',
    'ALL_GloJobTypeSubCategory',
    'ALL_GloLeadSource',
    'ALL_GloEstimateFlavor',
    'ALL_GloEstimateStatus',
    'ALL_GloPaymentMethodType',
    'ALL_GloResolutionType',
    'ALL_GloSetupLaborRateType',
    'GloSkill',
    'ALL_GloTag',
    'GloTrade',
    'ALL_GloTitleOfCourtesy',
    'ALL_GloZone',
    'ALL_GloSetupPaymentTerm',
    'GLO_ZONE_TO_FGS_SETUP_ZONE',
    'GLO_TRADE_TO_FGS_SETUP_TECH_TRADE',
    'GLO_SKILL_TO_FGS_SETUP_TECH_SKILL_LEVEL',
    'GLO_LEAD_SOURCE_TO_FGS_LEAD_SOURCE',
    'GLO_ROLE_TO_FGS_ROLE',
    'GLO_TITLE_OF_COURTESY_TO_FGS_SETUP_TITLE_OF_COURTESY',
    'GLO_TAG_TO_FGS_TAG',
    'GLO_UNIT_OF_MEASURE_TO_FGS_UNIT_OF_MEASURE',
    'GLO_INVENTORY_ITEM_TYPE_TO_FGS_INVENTORY_ITEM_TYPE',
    'GLO_INVENTORY_CATEGORY_TO_FGS_INVENTORY_CATEGORY',
    'GLO_BILLING_CATEGORY_TO_FGS_BILLING_CATEGORY',
    'GLO_BUSINESS_TYPE_TO_FGS_BUSINESS_TYPE',
    'GLO_SETUP_LABOR_RATE_TYPE_TO_FGS_SETUP_LABOR_RATE_TYPE',
    'GLO_SETUP_PAYMENT_TERM_TO_FGS_SETUP_PAYMENT_TERM'
);

DELETE FROM glo."GloInventorySubCategory";

DELETE FROM glo."GloInventoryCategory";

DELETE FROM glo."GloInventoryItemType"
WHERE "ItemTypeCode" IN (
    'INVENTORY',
    'NONINVENTORY',
    'SERVICE',
    'KIT',
    'TOOL'
);

DELETE FROM glo."GloLeadSource"
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

DELETE FROM glo."GloEstimateStatus"
WHERE "StatusCode" IN (
    'DRAFT',
    'SENT',
    'VIEWED',
    'FOLLOWUP',
    'SOLD',
    'DECLINED',
    'EXPIRED',
    'BOOKED',
    'CANCELLED'
);

DELETE FROM glo."GloTitleOfCourtesy"
WHERE "Code" IN (
    'MR',
    'MRS',
    'MS',
    'MISS',
    'DR',
    'PROF',
    'REV'
);

DELETE FROM glo."GloTag"
WHERE "TagCode" IN (
    'URGENT',
    'VIP',
    'WARRANTY',
    'FOLLOWUP',
    'COMMERCIAL',
    'INSPECTION'
);

DELETE FROM glo."GloUnitOfMeasure"
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
