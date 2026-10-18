-- Glo_Permission_Seed.sql
-- Idempotent glo catalog for missing roles, permissions, role permissions, and role menus.
-- Run with the other Setup seeds after glo.GloRole, glo.GloPermission, glo.GloMenu,
-- glo.GloRolePermission, and glo.GloRoleMenu exist.
-- Does not change existing GloRole rows and does not write identity.FgsPermission.
-- Missing MenuCode values are skipped. A child menu also receives every ancestor.

-- Roles that are not already in Initial_Migration_Seed.sql
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
        ('TENANT_USER',        'Tenant User',        'Read-only access to all admin permissions',                         'TENANT',  false, false, 11::smallint, true),
        ('SALES',              'Sales',              'Create leads, estimates, and opportunities',                        'COMPANY', true,  false, 26::smallint, true),
        ('INVENTORY_MANAGER',  'Inventory Manager',  'Manages trucks, warehouses, purchase orders, and part assignments', 'COMPANY', true,  false, 27::smallint, true),
        ('API_CLIENT',         'API Client',         'User-defined role for external systems',                            'COMPANY', true,  false, 28::smallint, true)
) AS v("RoleCode", "Name", "Description", "RoleLevel", "IsAssignable", "IsSystemRole", "SortOrder", "IsActive")
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloRole" existing
    WHERE existing."RoleCode" = v."RoleCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloRole"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloRole"), 1),
    true);

DROP TABLE IF EXISTS "GloPermissionSeed";

CREATE TEMP TABLE "GloPermissionSeed"
(
    "DisplayOrder" smallint NOT NULL,
    "PermissionCode" character varying(100) NOT NULL,
    "Name" character varying(150) NOT NULL,
    "Description" character varying(255) NOT NULL,
    CONSTRAINT "PK_GloPermissionSeed" PRIMARY KEY ("PermissionCode")
);

INSERT INTO "GloPermissionSeed"
(
    "DisplayOrder",
    "PermissionCode",
    "Name",
    "Description"
)
VALUES
    (1::smallint,   'ROLE.CREATE',                              'Add New Role',                         'Allows creating a new role.'),
    (2::smallint,   'ROLE.LIST',                                'Role Grid',                            'Allows viewing the role grid.'),
    (3::smallint,   'ROLE.EDIT',                                'Edit Role',                            'Allows editing a role.'),
    (4::smallint,   'ROLE.PERMISSIONS',                         'Role Permissions',                     'Allows managing permissions assigned to a role.'),
    (5::smallint,   'ROLE.CLONE',                               'Clone Role',                           'Allows cloning a role.'),
    (10::smallint,  'DASHBOARD.VIEW',                           'Dashboard',                            'Allows viewing the dashboard.'),
    (20::smallint,  'LEAD.LIST',                                'Lead Listing',                         'Allows viewing the lead listing.'),
    (21::smallint,  'LEAD.VIEW',                                'Lead View',                            'Allows viewing a lead.'),
    (22::smallint,  'LEAD.CREATE_EDIT',                         'Lead Create/Edit',                     'Allows creating and editing leads.'),
    (30::smallint,  'WORKORDER.LIST',                           'Work Order Listing',                   'Allows viewing the work order listing.'),
    (31::smallint,  'WORKORDER.VIEW',                           'Work Order View',                      'Allows viewing a work order.'),
    (32::smallint,  'WORKORDER.CREATE_EDIT',                    'Work Order Create/Edit',               'Allows creating and editing work orders.'),
    (40::smallint,  'QUOTE.LIST',                               'Quote Listing',                        'Allows viewing the quote listing.'),
    (41::smallint,  'QUOTE.VIEW',                               'Quote View',                           'Allows viewing a quote.'),
    (42::smallint,  'QUOTE.CREATE_EDIT',                        'Quote Create/Edit',                    'Allows creating and editing quotes.'),
    (50::smallint,  'INVOICE.LIST',                             'Invoice Listing',                      'Allows viewing the invoice listing.'),
    (51::smallint,  'INVOICE.VIEW',                             'Invoice View',                         'Allows viewing an invoice.'),
    (52::smallint,  'INVOICE.CREATE_EDIT',                      'Invoice Create/Edit',                  'Allows creating and editing invoices.'),
    (60::smallint,  'CUSTOMER.LIST',                            'Customer Listing',                     'Allows viewing the customer listing.'),
    (61::smallint,  'CUSTOMER.VIEW',                            'Customer View',                        'Allows viewing a customer.'),
    (62::smallint,  'CUSTOMER.CREATE_EDIT',                     'Customer Create/Edit',                 'Allows creating and editing customers.'),
    (70::smallint,  'DISPATCH.VIEW',                            'Dispatch',                             'Allows viewing dispatch.'),
    (80::smallint,  'CONTRACT.LIST',                            'Contract Listing',                     'Allows viewing the contract listing.'),
    (81::smallint,  'CONTRACT.VIEW',                            'Contract View',                        'Allows viewing a contract.'),
    (82::smallint,  'CONTRACT.CREATE_EDIT',                     'Contract Create/Edit',                 'Allows creating and editing contracts.'),
    (90::smallint,  'PURCHASEORDER.LIST',                       'Purchase Order Listing',               'Allows viewing the purchase order listing.'),
    (91::smallint,  'PURCHASEORDER.VIEW',                       'Purchase Order View',                  'Allows viewing a purchase order.'),
    (92::smallint,  'PURCHASEORDER.CREATE_EDIT',                'Purchase Order Create/Edit',           'Allows creating and editing purchase orders.'),
    (100::smallint, 'SETTINGS.PAYROLL_SETTING',                 'Payroll Setting',                      'Allows managing payroll settings.'),
    (101::smallint, 'SETTINGS.COMPANY.GENERAL_INFO',            'Company Setup — General Info',         'Allows managing company general information.'),
    (102::smallint, 'SETTINGS.COMPANY.SERVICES',                'Company Setup — Services',             'Allows managing company services.'),
    (103::smallint, 'SETTINGS.WORK_ORDER_CONFIGURATION',        'Work Order Configuration',             'Allows managing work order configuration.'),
    (104::smallint, 'SETTINGS.CUSTOM_FIELDS',                   'Custom Fields',                        'Allows managing custom fields.'),
    (105::smallint, 'SETTINGS.QUOTE_CONFIGURATION',             'Quote Configuration',                  'Allows managing quote configuration.'),
    (106::smallint, 'SETTINGS.SETUP',                           'Setup',                                'Allows managing setup settings.'),
    (107::smallint, 'SETTINGS.CONTRACT_CONFIGURATION',          'Contract Configuration',               'Allows managing contract configuration.'),
    (108::smallint, 'SETTINGS.PURCHASE_ORDER_CONFIGURATION',    'Purchase Order Configuration',         'Allows managing purchase order configuration.'),
    (109::smallint, 'SETTINGS.SCHEDULE',                        'Schedule Setup',                       'Allows managing schedule setup.'),
    (110::smallint, 'SETTINGS.INVOICE_CONFIGURATION',           'Invoice Configuration',                'Allows managing invoice configuration.'),
    (111::smallint, 'SETTINGS.TEMPLATE_LISTING',                'Template Listing',                     'Allows viewing the template listing.'),
    (112::smallint, 'SETTINGS.PAYROLL_CONFIGURATION',           'Payroll Configuration',                'Allows managing payroll configuration.'),
    (113::smallint, 'SETTINGS.TAX',                             'Tax Setting',                          'Allows managing tax settings.'),
    (114::smallint, 'SETTINGS.ZIP_CODE',                        'Zip Code',                             'Allows managing zip codes.'),
    (115::smallint, 'SETTINGS.JOB_TYPE',                        'Job Type',                             'Allows managing job types.'),
    (116::smallint, 'SETTINGS.PRICE_BOOK',                      'Price Book',                           'Allows managing the price book.'),
    (117::smallint, 'SETTINGS.INVENTORY',                       'Inventory Management',                 'Allows managing inventory.'),
    (118::smallint, 'SETTINGS.TRUCK_WAREHOUSE',                 'Truck and Warehouse Management',       'Allows managing trucks and warehouses.'),
    (119::smallint, 'SETTINGS.PART_ASSIGNMENT',                 'Part Assignment',                      'Allows managing part assignments.'),
    (120::smallint, 'SETTINGS.NOTIFICATION',                    'Notification',                         'Allows managing notification settings.'),
    (200::smallint, 'MOBILE.TEAM',                              'Team',                                 'Allows using the mobile team feature.'),
    (201::smallint, 'MOBILE.FIELD_MANAGER',                     'Is Field Manager',                     'Allows acting as a field manager on mobile.'),
    (202::smallint, 'MOBILE.VIEW_FUTURE_APPOINTMENTS',          'Can View Future Appointments',         'Allows viewing future appointments on mobile.'),
    (203::smallint, 'MOBILE.VIEW_PAST_APPOINTMENTS',            'Can View Past Appointments',           'Allows viewing past appointments on mobile.');

INSERT INTO glo."GloPermission"
(
    "PermissionCode",
    "Name",
    "Description",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    seed."PermissionCode",
    seed."Name",
    seed."Description",
    seed."DisplayOrder",
    true,
    timezone('utc', now())
FROM "GloPermissionSeed" seed
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloPermission" existing
    WHERE existing."PermissionCode" = seed."PermissionCode"
);

SELECT setval(
    pg_get_serial_sequence('glo."GloPermission"', 'Id'),
    COALESCE((SELECT MAX("Id") FROM glo."GloPermission"), 1),
    true);

DROP TABLE IF EXISTS "GloRolePermissionSeed";

CREATE TEMP TABLE "GloRolePermissionSeed"
(
    "RoleCode" character varying(50) NOT NULL,
    "PermissionCode" character varying(100) NOT NULL,
    CONSTRAINT "PK_GloRolePermissionSeed" PRIMARY KEY ("RoleCode", "PermissionCode")
);

-- Tenant and company administrators receive every permission in this seed.
INSERT INTO "GloRolePermissionSeed" ("RoleCode", "PermissionCode")
SELECT admin."RoleCode", seed."PermissionCode"
FROM (
    VALUES
        ('TENANT_ADMIN'),
        ('COMPANY_ADMIN')
) AS admin("RoleCode")
CROSS JOIN "GloPermissionSeed" seed
ON CONFLICT DO NOTHING;

-- Read-only roles: *.LIST and *.VIEW only. No create/edit, role maintenance, settings, or mobile.
INSERT INTO "GloRolePermissionSeed" ("RoleCode", "PermissionCode")
SELECT reader."RoleCode", seed."PermissionCode"
FROM (
    VALUES
        ('TENANT_USER'),
        ('READONLY_AUDITOR')
) AS reader("RoleCode")
CROSS JOIN "GloPermissionSeed" seed
WHERE (
        seed."PermissionCode" LIKE '%.LIST'
        OR seed."PermissionCode" LIKE '%.VIEW'
    )
  AND seed."PermissionCode" NOT LIKE 'SETTINGS.%'
  AND seed."PermissionCode" NOT LIKE 'MOBILE.%'
  AND seed."PermissionCode" NOT IN (
        'ROLE.CREATE',
        'ROLE.EDIT',
        'ROLE.PERMISSIONS',
        'ROLE.CLONE'
    )
ON CONFLICT DO NOTHING;

INSERT INTO "GloRolePermissionSeed" ("RoleCode", "PermissionCode")
SELECT paired."RoleCode", paired."PermissionCode"
FROM (
    VALUES
        ('DISPATCHER', 'DASHBOARD.VIEW'),
        ('DISPATCHER', 'WORKORDER.LIST'),
        ('DISPATCHER', 'WORKORDER.VIEW'),
        ('DISPATCHER', 'WORKORDER.CREATE_EDIT'),
        ('DISPATCHER', 'DISPATCH.VIEW'),
        ('DISPATCHER', 'CUSTOMER.LIST'),
        ('DISPATCHER', 'CUSTOMER.VIEW'),

        ('BILLING', 'INVOICE.LIST'),
        ('BILLING', 'INVOICE.VIEW'),
        ('BILLING', 'INVOICE.CREATE_EDIT'),
        ('BILLING', 'QUOTE.LIST'),
        ('BILLING', 'QUOTE.VIEW'),
        ('BILLING', 'PURCHASEORDER.LIST'),
        ('BILLING', 'PURCHASEORDER.VIEW'),
        ('BILLING', 'PURCHASEORDER.CREATE_EDIT'),

        ('CSR', 'CUSTOMER.LIST'),
        ('CSR', 'CUSTOMER.VIEW'),
        ('CSR', 'CUSTOMER.CREATE_EDIT'),
        ('CSR', 'LEAD.LIST'),
        ('CSR', 'LEAD.VIEW'),

        ('SALES', 'LEAD.LIST'),
        ('SALES', 'LEAD.VIEW'),
        ('SALES', 'LEAD.CREATE_EDIT'),
        ('SALES', 'QUOTE.LIST'),
        ('SALES', 'QUOTE.VIEW'),
        ('SALES', 'QUOTE.CREATE_EDIT'),
        ('SALES', 'CUSTOMER.LIST'),
        ('SALES', 'CUSTOMER.VIEW'),

        ('FIELD_SUPERVISOR', 'WORKORDER.LIST'),
        ('FIELD_SUPERVISOR', 'WORKORDER.VIEW'),
        ('FIELD_SUPERVISOR', 'DISPATCH.VIEW'),
        ('FIELD_SUPERVISOR', 'QUOTE.VIEW'),
        ('FIELD_SUPERVISOR', 'CUSTOMER.VIEW'),
        ('FIELD_SUPERVISOR', 'MOBILE.TEAM'),
        ('FIELD_SUPERVISOR', 'MOBILE.VIEW_FUTURE_APPOINTMENTS'),
        ('FIELD_SUPERVISOR', 'MOBILE.VIEW_PAST_APPOINTMENTS'),

        ('FIELD_TECH', 'WORKORDER.VIEW'),
        ('FIELD_TECH', 'CUSTOMER.VIEW'),
        ('FIELD_TECH', 'MOBILE.VIEW_PAST_APPOINTMENTS'),

        ('INVENTORY_MANAGER', 'PURCHASEORDER.LIST'),
        ('INVENTORY_MANAGER', 'PURCHASEORDER.VIEW'),
        ('INVENTORY_MANAGER', 'PURCHASEORDER.CREATE_EDIT'),
        ('INVENTORY_MANAGER', 'SETTINGS.INVENTORY'),
        ('INVENTORY_MANAGER', 'SETTINGS.TRUCK_WAREHOUSE'),
        ('INVENTORY_MANAGER', 'SETTINGS.PART_ASSIGNMENT')
) AS paired("RoleCode", "PermissionCode")
INNER JOIN "GloPermissionSeed" seed ON seed."PermissionCode" = paired."PermissionCode"
ON CONFLICT DO NOTHING;

-- Service manager receives every lead, work order, and quote permission in this seed.
INSERT INTO "GloRolePermissionSeed" ("RoleCode", "PermissionCode")
SELECT 'SERVICE_MANAGER', seed."PermissionCode"
FROM "GloPermissionSeed" seed
WHERE seed."PermissionCode" LIKE 'LEAD.%'
   OR seed."PermissionCode" LIKE 'WORKORDER.%'
   OR seed."PermissionCode" LIKE 'QUOTE.%'
   OR seed."PermissionCode" IN (
        'DISPATCH.VIEW',
        'CUSTOMER.LIST',
        'CUSTOMER.VIEW',
        'MOBILE.TEAM',
        'MOBILE.FIELD_MANAGER',
        'MOBILE.VIEW_FUTURE_APPOINTMENTS',
        'MOBILE.VIEW_PAST_APPOINTMENTS'
    )
ON CONFLICT DO NOTHING;

INSERT INTO glo."GloRolePermission"
(
    "RoleId",
    "PermissionId",
    "IsActive",
    "CreatedOn"
)
SELECT
    role."Id",
    permission."Id",
    true,
    timezone('utc', now())
FROM "GloRolePermissionSeed" seed
INNER JOIN glo."GloRole" role ON role."RoleCode" = seed."RoleCode"
INNER JOIN glo."GloPermission" permission ON permission."PermissionCode" = seed."PermissionCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloRolePermission" existing
    WHERE existing."RoleId" = role."Id"
      AND existing."PermissionId" = permission."Id"
);

DROP TABLE IF EXISTS "GloRoleMenuSeed";

CREATE TEMP TABLE "GloRoleMenuSeed"
(
    "RoleCode" character varying(50) NOT NULL,
    "MenuCode" character varying(50) NOT NULL,
    CONSTRAINT "PK_GloRoleMenuSeed" PRIMARY KEY ("RoleCode", "MenuCode")
);

-- Administrators receive every global menu. API_CLIENT receives none.
INSERT INTO "GloRoleMenuSeed" ("RoleCode", "MenuCode")
SELECT admin."RoleCode", menu."MenuCode"
FROM (
    VALUES
        ('TENANT_ADMIN'),
        ('COMPANY_ADMIN')
) AS admin("RoleCode")
CROSS JOIN glo."GloMenu" menu
ON CONFLICT DO NOTHING;

-- Read-only roles receive every menu except Create New and its descendants.
WITH RECURSIVE excluded AS (
    SELECT create_root."Id"
    FROM glo."GloMenu" create_root
    WHERE create_root."MenuCode" = 'TOP_CREATE'
    UNION ALL
    SELECT child."Id"
    FROM glo."GloMenu" child
    INNER JOIN excluded parent ON child."ParentMenuId" = parent."Id"
)
INSERT INTO "GloRoleMenuSeed" ("RoleCode", "MenuCode")
SELECT reader."RoleCode", menu."MenuCode"
FROM (
    VALUES
        ('TENANT_USER'),
        ('READONLY_AUDITOR')
) AS reader("RoleCode")
CROSS JOIN glo."GloMenu" menu
WHERE menu."Id" NOT IN (SELECT excluded."Id" FROM excluded)
ON CONFLICT DO NOTHING;

INSERT INTO "GloRoleMenuSeed" ("RoleCode", "MenuCode")
SELECT paired."RoleCode", paired."MenuCode"
FROM (
    VALUES
        ('DISPATCHER', 'TODAY'),
        ('DISPATCHER', 'DASHBOARD'),
        ('DISPATCHER', 'WORK_ORDERS'),
        ('DISPATCHER', 'CREATE_WORK_ORDER'),
        ('DISPATCHER', 'DISPATCH_BOARD'),
        ('DISPATCHER', 'CUSTOMERS'),
        ('DISPATCHER', 'WORK'),
        ('DISPATCHER', 'CAPACITY_PLANNING'),

        ('BILLING', 'SALES'),
        ('BILLING', 'INVOICE'),
        ('BILLING', 'ESTIMATE'),
        ('BILLING', 'PAYMENTS'),
        ('BILLING', 'PAYMENT_TRANSACTIONS'),
        ('BILLING', 'SCHEDULE_PAYMENT'),
        ('BILLING', 'PURCHASE'),
        ('BILLING', 'PURCHASE_ORDERS'),
        ('BILLING', 'RETURNS'),

        ('CSR', 'TODAY'),
        ('CSR', 'CUSTOMERS'),
        ('CSR', 'CREATE_CUSTOMER'),
        ('CSR', 'SERVICE_LOCATIONS'),
        ('CSR', 'SALES'),
        ('CSR', 'LEAD'),
        ('CSR', 'TOP_CALL'),
        ('CSR', 'TOP_CHAT'),

        ('SALES', 'SALES'),
        ('SALES', 'LEAD'),
        ('SALES', 'ESTIMATE'),
        ('SALES', 'CREATE_LEAD'),
        ('SALES', 'CREATE_ESTIMATE'),
        ('SALES', 'CUSTOMERS'),
        ('SALES', 'TOP_NOTIFICATIONS'),

        ('SERVICE_MANAGER', 'TODAY'),
        ('SERVICE_MANAGER', 'DASHBOARD'),
        ('SERVICE_MANAGER', 'WORK_ORDERS'),
        ('SERVICE_MANAGER', 'CREATE_WORK_ORDER'),
        ('SERVICE_MANAGER', 'DISPATCH_BOARD'),
        ('SERVICE_MANAGER', 'CUSTOMERS'),
        ('SERVICE_MANAGER', 'SALES'),
        ('SERVICE_MANAGER', 'LEAD'),
        ('SERVICE_MANAGER', 'ESTIMATE'),
        ('SERVICE_MANAGER', 'CREATE_LEAD'),
        ('SERVICE_MANAGER', 'CREATE_ESTIMATE'),

        ('FIELD_SUPERVISOR', 'TODAY'),
        ('FIELD_SUPERVISOR', 'WORK_ORDERS'),
        ('FIELD_SUPERVISOR', 'DISPATCH_BOARD'),
        ('FIELD_SUPERVISOR', 'CUSTOMERS'),
        ('FIELD_SUPERVISOR', 'ESTIMATE'),

        ('FIELD_TECH', 'TODAY'),
        ('FIELD_TECH', 'WORK_ORDERS'),
        ('FIELD_TECH', 'CUSTOMERS'),

        ('INVENTORY_MANAGER', 'PURCHASE'),
        ('INVENTORY_MANAGER', 'PURCHASE_ORDERS'),
        ('INVENTORY_MANAGER', 'RETURNS'),
        ('INVENTORY_MANAGER', 'VENDORS'),
        ('INVENTORY_MANAGER', 'SETTINGS'),
        ('INVENTORY_MANAGER', 'SETTINGS_INVENTORY'),
        ('INVENTORY_MANAGER', 'SETTINGS_TRUCK_WAREHOUSE'),
        ('INVENTORY_MANAGER', 'SETTINGS_MATERIAL'),
        ('INVENTORY_MANAGER', 'SETTINGS_TRANSACTION_LOG')
) AS paired("RoleCode", "MenuCode")
ON CONFLICT DO NOTHING;

-- Assign every ancestor of a selected menu (folder parents and Create New).
WITH RECURSIVE walk AS (
    SELECT
        seed."RoleCode",
        menu."Id" AS "MenuId",
        menu."ParentMenuId"
    FROM "GloRoleMenuSeed" seed
    INNER JOIN glo."GloMenu" menu ON menu."MenuCode" = seed."MenuCode"
    UNION
    SELECT
        walk."RoleCode",
        parent."Id",
        parent."ParentMenuId"
    FROM walk
    INNER JOIN glo."GloMenu" parent ON parent."Id" = walk."ParentMenuId"
)
INSERT INTO "GloRoleMenuSeed" ("RoleCode", "MenuCode")
SELECT walk."RoleCode", menu."MenuCode"
FROM walk
INNER JOIN glo."GloMenu" menu ON menu."Id" = walk."MenuId"
ON CONFLICT DO NOTHING;

INSERT INTO glo."GloRoleMenu"
(
    "RoleId",
    "MenuId",
    "SortOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    role."Id",
    menu."Id",
    menu."SortOrder",
    true,
    timezone('utc', now())
FROM "GloRoleMenuSeed" seed
INNER JOIN glo."GloRole" role ON role."RoleCode" = seed."RoleCode"
INNER JOIN glo."GloMenu" menu ON menu."MenuCode" = seed."MenuCode"
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloRoleMenu" existing
    WHERE existing."RoleId" = role."Id"
      AND existing."MenuId" = menu."Id"
);

DROP TABLE IF EXISTS "GloRolePermissionSeed";
DROP TABLE IF EXISTS "GloRoleMenuSeed";
DROP TABLE IF EXISTS "GloPermissionSeed";
