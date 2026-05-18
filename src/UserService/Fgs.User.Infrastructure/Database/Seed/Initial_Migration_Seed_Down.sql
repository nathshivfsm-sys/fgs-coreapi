-- =============================================================================
-- Revert: CleanUpTables global reference data seed
-- Pair with: Initial_Migration_Seed.sql
-- =============================================================================

START TRANSACTION;

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
    'PAINTING'
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

COMMIT;
