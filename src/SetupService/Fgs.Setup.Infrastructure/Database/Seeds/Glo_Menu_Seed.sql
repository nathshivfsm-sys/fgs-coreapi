-- Glo_Menu_Seed.sql
-- FieldPro navigation catalog for glo.GloMenu.
-- Run with the other Setup seeds after glo.GloMenu exists.
-- Does not change ALL_GloMenu provisioning mappings (those stay in Initial_Migration_Seed.sql).
-- Replaces every row in glo.GloMenu. Top Menu is only Universal Search, Chat,
-- Notifications, Call, and Profile. Every other item, including Create New, is Left Menu.
-- Explicit Ids stay stable for identity.FgsTenantMenu.MenuId.

DROP TABLE IF EXISTS "GloMenuSeed";

CREATE TEMP TABLE "GloMenuSeed"
(
    "Id" integer NOT NULL,
    "MenuCode" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(255),
    "ParentMenuCode" character varying(50),
    "MenuType" character varying(20) NOT NULL,
    "Route" character varying(255),
    "Icon" character varying(100),
    "SortOrder" smallint NOT NULL,
    CONSTRAINT "PK_GloMenuSeed" PRIMARY KEY ("Id"),
    CONSTRAINT "UX_GloMenuSeed_MenuCode" UNIQUE ("MenuCode"),
    CONSTRAINT "CK_GloMenuSeed_MenuCode_NotEmpty" CHECK (length(trim("MenuCode")) > 0),
    CONSTRAINT "CK_GloMenuSeed_Name_NotEmpty" CHECK (length(trim("Name")) > 0),
    CONSTRAINT "CK_GloMenuSeed_MenuType" CHECK ("MenuType" IN ('Top Menu', 'Left Menu'))
);

INSERT INTO "GloMenuSeed"
(
    "Id",
    "MenuCode",
    "Name",
    "Description",
    "ParentMenuCode",
    "MenuType",
    "Route",
    "Icon",
    "SortOrder"
)
SELECT
    v."Id",
    v."MenuCode",
    v."Name",
    v."Description",
    v."ParentMenuCode",
    v."MenuType",
    v."Route",
    v."Icon",
    v."SortOrder"
FROM (
    VALUES
        -- Top Menu: search, chat, reminder notifications, call, and profile only
        (1,  'TOP_SEARCH',        'Universal Search', 'Opens universal search from the top bar.', NULL, 'Top Menu', NULL, 'search',         1::smallint),
        (2,  'TOP_CHAT',          'Chat',             'Opens chat from the top bar.',             NULL, 'Top Menu', NULL, 'message-circle', 2::smallint),
        (3,  'TOP_NOTIFICATIONS', 'Notifications',    'Bell icon for reminder notifications.',    NULL, 'Top Menu', NULL, 'bell',           3::smallint),
        (4,  'TOP_CALL',          'Call',             'Call icon in the top bar.',                NULL, 'Top Menu', NULL, 'phone',          4::smallint),
        (5,  'TOP_PROFILE',       'Profile',          'Opens the user profile from the top bar.', NULL, 'Top Menu', NULL, 'user',           5::smallint),

        -- Left Menu roots. Create New is the sidebar action above Today.
        (6,  'TOP_CREATE',        'Create New',        'Opens shortcuts for creating new records.', NULL, 'Left Menu', NULL,                 'plus',             1::smallint),
        (7,  'TODAY',             'Today',             'Opens the Today workspace.',                NULL, 'Left Menu', '/today',             'home',             2::smallint),
        (8,  'DASHBOARD',         'Dashboard',         'Opens the dashboard.',                      NULL, 'Left Menu', '/dashboard',         'layout-dashboard', 3::smallint),
        (9,  'WORK_ORDERS',       'Work Orders',       'Opens the work order list.',                NULL, 'Left Menu', '/work-orders',       'clipboard',        4::smallint),
        (10, 'DISPATCH_BOARD',    'Dispatch Board',    'Opens the dispatch board.',                 NULL, 'Left Menu', '/dispatch-board',    'map',              5::smallint),
        (11, 'CUSTOMERS',         'Customers',         'Opens the customer list.',                  NULL, 'Left Menu', '/customers',         'users',            6::smallint),
        (12, 'SERVICE_LOCATIONS', 'Service Locations', 'Opens service locations.',                  NULL, 'Left Menu', '/service-locations', 'map-pin',          7::smallint),
        (13, 'SALES',             'Sales',             'Sales section folder.',                     NULL, 'Left Menu', NULL,                 'handshake',        8::smallint),
        (14, 'WORK',              'Work',              'Work section folder.',                      NULL, 'Left Menu', NULL,                 'calendar',         9::smallint),
        (15, 'PAYMENTS',          'Payments',          'Payments section folder.',                  NULL, 'Left Menu', NULL,                 'credit-card',      10::smallint),
        (16, 'PURCHASE',          'Purchase',          'Purchase section folder.',                  NULL, 'Left Menu', NULL,                 'shopping-cart',    11::smallint),
        (17, 'REPORTS',           'Reports',           'Reports section folder.',                   NULL, 'Left Menu', NULL,                 'bar-chart',        12::smallint),
        (18, 'SETTINGS',          'Settings',          'Opens settings.',                           NULL, 'Left Menu', NULL,                 'settings',         13::smallint),

        -- Left Menu / Create New children
        (19, 'CREATE_WORK_ORDER', 'Work Order', 'Creates a new work order.', 'TOP_CREATE', 'Left Menu', '/work-orders/new', 'clipboard',  1::smallint),
        (20, 'CREATE_LEAD',       'Lead',       'Creates a new lead.',        'TOP_CREATE', 'Left Menu', '/leads/new',       'handshake',  2::smallint),
        (21, 'CREATE_CUSTOMER',   'Customer',   'Creates a new customer.',    'TOP_CREATE', 'Left Menu', '/customers/new',   'users',      3::smallint),
        (22, 'CREATE_ESTIMATE',   'Estimate',   'Creates a new estimate.',    'TOP_CREATE', 'Left Menu', '/estimates/new',   'calculator', 4::smallint),

        -- Left Menu / Sales
        (23, 'LEAD',                'Lead',                'Opens the lead list.',        'SALES', 'Left Menu', '/leads',               'handshake', 1::smallint),
        (24, 'INVOICE',             'Invoice',             'Opens the invoice list.',     'SALES', 'Left Menu', '/invoices',            'file-text', 2::smallint),
        (25, 'ESTIMATE',            'Estimate',            'Opens the estimate list.',    'SALES', 'Left Menu', '/estimates',           'calculator', 3::smallint),
        (26, 'SERVICE_AGREEMENTS',  'Service Agreements',  'Opens service agreements.',   'SALES', 'Left Menu', '/service-agreements',  'calendar',  4::smallint),

        -- Left Menu / Work
        (27, 'CAPACITY_PLANNING', 'Capacity Planning', 'Opens capacity planning.', 'WORK', 'Left Menu', '/capacity-planning', 'calendar-clock', 1::smallint),
        (28, 'PROJECTS',          'Projects',          'Opens projects.',           'WORK', 'Left Menu', '/projects',          'folder',         2::smallint),

        -- Left Menu / Payments
        (29, 'PAYMENT_TRANSACTIONS', 'Payments',         'Opens payment transactions.', 'PAYMENTS', 'Left Menu', '/payments',          'credit-card',    1::smallint),
        (30, 'SCHEDULE_PAYMENT',     'Schedule Payment', 'Opens scheduled payments.',   'PAYMENTS', 'Left Menu', '/schedule-payments', 'calendar-clock', 2::smallint),
        (31, 'REFUNDS',              'Refunds',          'Opens refunds.',              'PAYMENTS', 'Left Menu', '/refunds',           'undo',           3::smallint),

        -- Left Menu / Purchase
        (32, 'PURCHASE_ORDERS', 'Purchase Orders', 'Opens purchase orders.',  'PURCHASE', 'Left Menu', '/purchase-orders', 'shopping-cart', 1::smallint),
        (33, 'RETURNS',         'Returns',         'Opens purchase returns.', 'PURCHASE', 'Left Menu', '/returns',         'rotate-ccw',    2::smallint),
        (34, 'VENDORS',         'Vendors',         'Opens vendors.',          'PURCHASE', 'Left Menu', '/vendors',         'store',         3::smallint),

        -- Left Menu / Reports
        (35, 'REPORTS_LIST', 'Reports', 'Opens the report list.', 'REPORTS', 'Left Menu', '/reports', 'bar-chart', 1::smallint),

        -- Left Menu / Settings sections
        (36, 'SETTINGS_COMPANY',         'Company Setup',       'Company setup folder.',       'SETTINGS', 'Left Menu', NULL,                  NULL, 1::smallint),
        (37, 'SETTINGS_CUSTOMER',        'Customer Setup',      'Customer setup folder.',      'SETTINGS', 'Left Menu', NULL,                  NULL, 2::smallint),
        (38, 'SETTINGS_GEOGRAPHICAL',    'Geographical Setup',  'Geographical setup folder.',  'SETTINGS', 'Left Menu', NULL,                  NULL, 3::smallint),
        (39, 'SETTINGS_OPERATIONS',      'Operations',          'Operations setup folder.',    'SETTINGS', 'Left Menu', NULL,                  NULL, 4::smallint),
        (40, 'SETTINGS_CONFIGURATION',   'Configuration',       'Configuration folder.',       'SETTINGS', 'Left Menu', NULL,                  NULL, 5::smallint),
        (41, 'SETTINGS_USER_MANAGEMENT', 'User Management',     'User management folder.',     'SETTINGS', 'Left Menu', NULL,                  NULL, 6::smallint),
        (42, 'SETTINGS_BILLING',         'Billing',             'Opens billing settings.',     'SETTINGS', 'Left Menu', '/settings/billing',    NULL, 7::smallint),
        (43, 'SETTINGS_ASSETS',          'Assets',              'Asset setup folder.',         'SETTINGS', 'Left Menu', NULL,                  NULL, 8::smallint),
        (44, 'SETTINGS_INVENTORY',       'Inventory',           'Inventory setup folder.',     'SETTINGS', 'Left Menu', NULL,                  NULL, 9::smallint),
        (45, 'SETTINGS_PRICING',         'Pricing',             'Pricing setup folder.',       'SETTINGS', 'Left Menu', NULL,                  NULL, 10::smallint),
        (46, 'SETTINGS_NOTIFICATION',    'Notification',        'Notification setup folder.',  'SETTINGS', 'Left Menu', NULL,                  NULL, 11::smallint),
        (47, 'SETTINGS_INTEGRATIONS',    'Integrations',        'Integrations folder.',        'SETTINGS', 'Left Menu', NULL,                  NULL, 12::smallint),
        (48, 'SETTINGS_AUTOMATION',      'Automation',          'Opens automation settings.',  'SETTINGS', 'Left Menu', '/settings/automation', NULL, 13::smallint),

        -- Settings / Company Setup
        (49, 'SETTINGS_COMPANY_GENERAL',    'General Info',        'Opens company general information.', 'SETTINGS_COMPANY', 'Left Menu', '/settings/company/general',            NULL, 1::smallint),
        (50, 'SETTINGS_TRADE',              'Trade',               'Opens company trades.',              'SETTINGS_COMPANY', 'Left Menu', '/settings/company/trades',             NULL, 2::smallint),
        (51, 'SETTINGS_BUSINESS_UNIT',      'Business Unit',       'Opens business units.',              'SETTINGS_COMPANY', 'Left Menu', '/settings/company/business-units',    NULL, 3::smallint),
        (52, 'SETTINGS_TITLE_OF_COURTESY',  'Title Of Courtesy',   'Opens titles of courtesy.',          'SETTINGS_COMPANY', 'Left Menu', '/settings/company/titles-of-courtesy', NULL, 4::smallint),

        -- Settings / Customer Setup
        (53, 'SETTINGS_CONTENT_PORTAL', 'Content Portal', 'Opens the content portal.',    'SETTINGS_CUSTOMER', 'Left Menu', '/settings/customer/content-portal', NULL, 1::smallint),
        (54, 'SETTINGS_TAX',            'Tax',            'Opens customer tax settings.', 'SETTINGS_CUSTOMER', 'Left Menu', '/settings/customer/tax',            NULL, 2::smallint),

        -- Settings / Geographical Setup
        (55, 'SETTINGS_ZONE',     'Zone',     'Opens zones.',     'SETTINGS_GEOGRAPHICAL', 'Left Menu', '/settings/geographical/zones',     NULL, 1::smallint),
        (56, 'SETTINGS_ZIP_CODE', 'Zip Code', 'Opens zip codes.', 'SETTINGS_GEOGRAPHICAL', 'Left Menu', '/settings/geographical/zip-codes', NULL, 2::smallint),

        -- Settings / Operations
        (57, 'SETTINGS_SKILLS',                'Skills',                'Opens skills.',                 'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/skills',                 NULL, 1::smallint),
        (58, 'SETTINGS_TIME_TRACKING',         'Time Tracking',         'Opens time tracking.',          'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/time-tracking',         NULL, 2::smallint),
        (59, 'SETTINGS_TIME_SLOTS',            'Time Slots',            'Opens time slots.',             'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/time-slots',            NULL, 3::smallint),
        (60, 'SETTINGS_STANDARD_DESCRIPTION',  'Standard Description',  'Opens standard descriptions.',  'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/standard-descriptions', NULL, 4::smallint),
        (61, 'SETTINGS_JOB_TYPE',              'Job Type',              'Opens job types.',              'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/job-types',             NULL, 5::smallint),
        (62, 'SETTINGS_SERVICE_CATEGORY',      'Service Category',      'Opens service categories.',     'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/service-categories',    NULL, 6::smallint),
        (63, 'SETTINGS_RESOLUTION_TYPE',       'Resolution Type',       'Opens resolution types.',       'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/resolution-types',      NULL, 7::smallint),
        (64, 'SETTINGS_PAYMENT_METHOD',        'Payment Method',        'Opens payment methods.',        'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/payment-methods',       NULL, 8::smallint),
        (65, 'SETTINGS_PAYMENT_TERMS',         'Payment Terms',         'Opens payment terms.',          'SETTINGS_OPERATIONS', 'Left Menu', '/settings/operations/payment-terms',        NULL, 9::smallint),

        -- Settings / Configuration
        (66, 'SETTINGS_WORK_ORDER',     'Work Order Setup',     'Opens work order setup.',     'SETTINGS_CONFIGURATION', 'Left Menu', '/settings/configuration/work-orders',     NULL, 1::smallint),
        (67, 'SETTINGS_INVOICE',        'Invoice Setup',        'Opens invoice setup.',        'SETTINGS_CONFIGURATION', 'Left Menu', '/settings/configuration/invoices',       NULL, 2::smallint),
        (68, 'SETTINGS_QUOTE',          'Quote Setup',          'Opens quote setup.',          'SETTINGS_CONFIGURATION', 'Left Menu', '/settings/configuration/quotes',         NULL, 3::smallint),
        (69, 'SETTINGS_CONTRACT',       'Contract Setup',       'Opens contract setup.',       'SETTINGS_CONFIGURATION', 'Left Menu', '/settings/configuration/contracts',      NULL, 4::smallint),
        (70, 'SETTINGS_PURCHASE_ORDER', 'Purchase Order Setup', 'Opens purchase order setup.', 'SETTINGS_CONFIGURATION', 'Left Menu', '/settings/configuration/purchase-orders', NULL, 5::smallint),
        (71, 'SETTINGS_FORMS',          'Forms',                'Opens forms.',                'SETTINGS_CONFIGURATION', 'Left Menu', '/settings/configuration/forms',          NULL, 6::smallint),
        (72, 'SETTINGS_TAGS',           'Tags',                 'Opens tags.',                 'SETTINGS_CONFIGURATION', 'Left Menu', '/settings/configuration/tags',           NULL, 7::smallint),

        -- Settings / User Management
        (73, 'SETTINGS_USERS', 'Users',                  'Opens users.',                  'SETTINGS_USER_MANAGEMENT', 'Left Menu', '/settings/users', NULL, 1::smallint),
        (74, 'SETTINGS_ROLES', 'Roles and Permissions',  'Opens roles and permissions.',  'SETTINGS_USER_MANAGEMENT', 'Left Menu', '/settings/roles', NULL, 2::smallint),

        -- Settings / Assets
        (75, 'SETTINGS_EQUIPMENT_CATEGORY', 'Equipment Category', 'Opens equipment categories.', 'SETTINGS_ASSETS', 'Left Menu', '/settings/assets/equipment-categories', NULL, 1::smallint),
        (76, 'SETTINGS_MANUFACTURER',       'Manufacturer',       'Opens manufacturers.',        'SETTINGS_ASSETS', 'Left Menu', '/settings/assets/manufacturers',         NULL, 2::smallint),
        (77, 'SETTINGS_MODEL',              'Model',              'Opens equipment models.',     'SETTINGS_ASSETS', 'Left Menu', '/settings/assets/models',                NULL, 3::smallint),

        -- Settings / Inventory
        (78, 'SETTINGS_TRUCK_WAREHOUSE', 'Truck and Warehouse', 'Opens trucks and warehouses.',       'SETTINGS_INVENTORY', 'Left Menu', '/settings/inventory/trucks-and-warehouses', NULL, 1::smallint),
        (79, 'SETTINGS_MATERIAL',        'Material Management', 'Opens material management.',         'SETTINGS_INVENTORY', 'Left Menu', '/settings/inventory/materials',              NULL, 2::smallint),
        (80, 'SETTINGS_TRANSACTION_LOG', 'Transaction Log',     'Opens the inventory transaction log.', 'SETTINGS_INVENTORY', 'Left Menu', '/settings/inventory/transaction-log',       NULL, 3::smallint),

        -- Settings / Pricing
        (81, 'SETTINGS_PRICE_BOOK',       'Price Book',             'Opens the price book.',            'SETTINGS_PRICING', 'Left Menu', '/settings/pricing/price-book',       NULL, 1::smallint),
        (82, 'SETTINGS_SERVICE_BUSINESS', 'Service Business',       'Opens service business pricing.',  'SETTINGS_PRICING', 'Left Menu', '/settings/pricing/service-business', NULL, 2::smallint),
        (83, 'SETTINGS_PRICING_MATRIX',   'Pricing Matrix',         'Opens the pricing matrix.',        'SETTINGS_PRICING', 'Left Menu', '/settings/pricing/pricing-matrix',   NULL, 3::smallint),
        (84, 'SETTINGS_PAYROLL',          'Payroll Configuration',  'Opens payroll configuration.',     'SETTINGS_PRICING', 'Left Menu', '/settings/pricing/payroll',          NULL, 4::smallint),

        -- Settings / Notification
        (85, 'SETTINGS_NOTIFICATION_PREFERENCE', 'Notification Preference', 'Opens notification preferences.', 'SETTINGS_NOTIFICATION', 'Left Menu', '/settings/notifications/preferences', NULL, 1::smallint),
        (86, 'SETTINGS_NOTIFICATION_TEMPLATES',  'Notification Templates',  'Opens notification templates.',   'SETTINGS_NOTIFICATION', 'Left Menu', '/settings/notifications/templates',   NULL, 2::smallint),

        -- Settings / Integrations
        (87, 'SETTINGS_QBO', 'QBO Integration', 'Opens QuickBooks Online integration.', 'SETTINGS_INTEGRATIONS', 'Left Menu', '/settings/integrations/qbo', NULL, 1::smallint)
) AS v("Id", "MenuCode", "Name", "Description", "ParentMenuCode", "MenuType", "Route", "Icon", "SortOrder");

DO $seed$
DECLARE
    inserted_count integer := 1;
    pass_count integer := 0;
BEGIN
    IF EXISTS (
        SELECT 1
        FROM "GloMenuSeed" child
        WHERE child."ParentMenuCode" IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM "GloMenuSeed" parent
              WHERE parent."MenuCode" = child."ParentMenuCode"
          )
    ) THEN
        RAISE EXCEPTION 'Glo menu seed references a parent MenuCode that is not in the seed.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM "GloMenuSeed"
        GROUP BY "ParentMenuCode", "MenuType", "SortOrder"
        HAVING COUNT(*) > 1
    ) THEN
        RAISE EXCEPTION 'Glo menu seed has a duplicate SortOrder among siblings.';
    END IF;

    -- Drop every current menu so the catalog below is the whole table.
    -- Parent FK is RESTRICT, so detach children before the delete.
    -- glo.GloRoleMenu rows are removed by FK_GloRoleMenu_Menu ON DELETE CASCADE.
    UPDATE glo."GloMenu"
    SET "ParentMenuId" = NULL
    WHERE "ParentMenuId" IS NOT NULL;

    DELETE FROM glo."GloMenu";

    -- Parents before children. Each pass inserts rows whose parent MenuCode is already in glo.GloMenu.
    WHILE inserted_count > 0 AND pass_count < 8 LOOP
        INSERT INTO glo."GloMenu"
        (
            "Id",
            "MenuCode",
            "Name",
            "Description",
            "ParentMenuId",
            "MenuType",
            "Route",
            "Icon",
            "SortOrder",
            "IsActive",
            "CreatedOn"
        )
        OVERRIDING SYSTEM VALUE
        SELECT
            v."Id",
            v."MenuCode",
            v."Name",
            v."Description",
            (
                SELECT parent."Id"
                FROM glo."GloMenu" parent
                WHERE parent."MenuCode" = v."ParentMenuCode"
            ),
            v."MenuType",
            v."Route",
            v."Icon",
            v."SortOrder",
            true,
            timezone('utc', now())
        FROM "GloMenuSeed" v
        WHERE NOT EXISTS (
            SELECT 1
            FROM glo."GloMenu" existing
            WHERE existing."MenuCode" = v."MenuCode"
        )
          AND (
              v."ParentMenuCode" IS NULL
              OR EXISTS (
                  SELECT 1
                  FROM glo."GloMenu" parent
                  WHERE parent."MenuCode" = v."ParentMenuCode"
              )
          );

        GET DIAGNOSTICS inserted_count = ROW_COUNT;
        pass_count := pass_count + 1;
    END LOOP;

    IF EXISTS (
        SELECT 1
        FROM "GloMenuSeed" v
        WHERE NOT EXISTS (
            SELECT 1
            FROM glo."GloMenu" existing
            WHERE existing."MenuCode" = v."MenuCode"
        )
    ) THEN
        RAISE EXCEPTION 'Glo menu seed did not insert every MenuCode. An existing row may already use one of the explicit Ids.';
    END IF;
END
$seed$;

-- Refresh labels and parent links for the rows just inserted. CreatedOn is set only on insert.
UPDATE glo."GloMenu" AS target
SET
    "Name" = v."Name",
    "Description" = v."Description",
    "ParentMenuId" = (
        SELECT parent."Id"
        FROM glo."GloMenu" parent
        WHERE parent."MenuCode" = v."ParentMenuCode"
    ),
    "MenuType" = v."MenuType",
    "Route" = v."Route",
    "Icon" = v."Icon",
    "SortOrder" = v."SortOrder",
    "IsActive" = true
FROM "GloMenuSeed" v
WHERE target."MenuCode" = v."MenuCode";

DROP TABLE IF EXISTS "GloMenuSeed";

SELECT setval(pg_get_serial_sequence('glo."GloMenu"', 'Id'), COALESCE((SELECT MAX("Id") FROM glo."GloMenu"), 1), true);
