START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup') THEN
    DROP INDEX identity."IX_FgsUser_TenantId_CompanyId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup') THEN
    DROP INDEX identity."IX_FgsUser_TenantId_Email";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup') THEN
    CREATE TABLE tenant."FgsTenantServiceAccountsSetup" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "BankAccountId" bigint,
        "AccountsReceivableAccountId" bigint,
        "RevenueAccountId" bigint,
        "DiscountAccountId" bigint,
        "SalesTaxPayableAccountId" bigint,
        "InventoryAccountId" bigint,
        "COGSAccountId" bigint,
        "UndepositedFundsAccountId" bigint,
        "ProcessingFeeAccountId" bigint,
        "AccountsPayableAccountId" bigint,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_FgsTenantServiceAccountsSetup" PRIMARY KEY ("TenantId", "CompanyId"),
        CONSTRAINT "FK_FgsTenantServiceAccountsSetup_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES tenant."FgsTenantCompany" ("TenantId", "CompanyNumber") ON DELETE CASCADE
    );
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."BankAccountId" IS 'Default bank account used for customer payments, deposits, and cash transactions.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."AccountsReceivableAccountId" IS 'General ledger account used to record customer accounts receivable.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."RevenueAccountId" IS 'Default revenue or income account used when posting invoices and completed work orders.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."DiscountAccountId" IS 'General ledger account used to record customer discounts and promotional adjustments.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."SalesTaxPayableAccountId" IS 'Liability account used to record collected sales taxes owed to tax authorities.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."InventoryAccountId" IS 'Asset account used to record the value of inventory on hand.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."COGSAccountId" IS 'Cost of Goods Sold account used when inventory is consumed or sold.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."UndepositedFundsAccountId" IS 'Holding account used for customer payments received but not yet deposited into a bank account.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."ProcessingFeeAccountId" IS 'Expense account used to record merchant, credit card, and payment processing fees.';
    COMMENT ON COLUMN tenant."FgsTenantServiceAccountsSetup"."AccountsPayableAccountId" IS 'General ledger account used to record amounts owed to vendors and suppliers.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup') THEN
    CREATE UNIQUE INDEX "IX_FgsUser_TenantId_CompanyId_Email" ON identity."FgsUser" ("TenantId", "CompanyId", "Email") WHERE "IsDeleted" = false;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260803193459_AddFgsTenantServiceAccountsSetup') THEN
    INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260803193459_AddFgsTenantServiceAccountsSetup', '10.0.8');
    END IF;
END $EF$;
COMMIT;

