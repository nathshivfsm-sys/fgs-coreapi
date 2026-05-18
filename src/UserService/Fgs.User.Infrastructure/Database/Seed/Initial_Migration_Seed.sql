-- =============================================================================
-- Seed: CleanUpTables global reference data (User Service)
-- Run manually after: 20260518163137_Initial_Migration_Up.sql
-- Not part of EF migration / Up / Down scripts.
--
-- Defaults where applicable:
--   CreatedOn = UTC now
--   CreatedBy = 'System'
-- =============================================================================

START TRANSACTION;

-- GloMasterEntityType
INSERT INTO dbo."GloMasterEntityType"
(
    "Id",
    "Code",
    "IsDocumentAllowed",
    "IsActive",
    "SortOrder",
    "CreatedOn",
    "CreatedBy"
)
VALUES
    (1, 'TENANT', true, true, 1, timezone('utc', now()), 'System'),
    (2, 'TENANT_COMPANY', true, true, 2, timezone('utc', now()), 'System'),
    (3, 'WORK_ORDER', true, true, 3, timezone('utc', now()), 'System'),
    (4, 'EMPLOYEE', true, true, 4, timezone('utc', now()), 'System'),
    (5, 'PURCHASE_ORDER', true, true, 5, timezone('utc', now()), 'System'),
    (6, 'VENDOR', true, true, 6, timezone('utc', now()), 'System'),
    (7, 'SUB_CONTRACTOR', true, true, 7, timezone('utc', now()), 'System'),
    (8, 'BILL_TO', true, true, 8, timezone('utc', now()), 'System'),
    (9, 'SERVICE_LOCATION', true, true, 9, timezone('utc', now()), 'System'),
    (10, 'PROPOSAL', true, true, 10, timezone('utc', now()), 'System')
ON CONFLICT ("Code") DO NOTHING;

SELECT setval(
    pg_get_serial_sequence('dbo."GloMasterEntityType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloMasterEntityType"), 1),
    true);

-- GloPaymentMethodType (no CreatedOn/CreatedBy columns)
INSERT INTO dbo."GloPaymentMethodType"
(
    "Code",
    "DisplayName",
    "SortOrder",
    "IsActive"
)
VALUES
    ('CASH', 'Cash', 1, true),
    ('CHECK', 'Check', 2, true),
    ('CREDIT_CARD', 'Credit Card', 3, true),
    ('DEBIT_CARD', 'Debit Card', 4, true),
    ('ACH', 'ACH Transfer', 5, true),
    ('APPLE_PAY', 'Apple Pay', 6, true),
    ('GOOGLE_PAY', 'Google Pay', 7, true),
    ('ZELLE', 'Zelle', 8, true)
ON CONFLICT ("Code") DO NOTHING;

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
VALUES
    (1, 'COMPLETED', 'Completed Successfully', true, timezone('utc', now()), 'System'),
    (2, 'INCOMPLETE', 'Incomplete Work', true, timezone('utc', now()), 'System'),
    (3, 'PART_REQUIRED', 'Parts Required', true, timezone('utc', now()), 'System'),
    (4, 'PARTS_ARRIVED', 'Parts Arrived', true, timezone('utc', now()), 'System'),
    (5, 'CANCELLED', 'Cancelled', true, timezone('utc', now()), 'System')
ON CONFLICT ("ResolutionTypeCode") DO NOTHING;

SELECT setval(
    pg_get_serial_sequence('dbo."GloResolutionType"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM dbo."GloResolutionType"), 1),
    true);

COMMIT;
