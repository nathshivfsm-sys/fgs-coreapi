-- =============================================================================
-- Revert: CleanUpTables global reference data seed
-- Pair with: Initial_Migration_Seed.sql
-- =============================================================================

START TRANSACTION;

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

DELETE FROM dbo."GloMasterEntityType"
WHERE "Code" IN (
    'TENANT',
    'TENANT_COMPANY',
    'WORK_ORDER',
    'EMPLOYEE',
    'PURCHASE_ORDER',
    'VENDOR',
    'SUB_CONTRACTOR',
    'BILL_TO',
    'SERVICE_LOCATION',
    'PROPOSAL'
);

COMMIT;
