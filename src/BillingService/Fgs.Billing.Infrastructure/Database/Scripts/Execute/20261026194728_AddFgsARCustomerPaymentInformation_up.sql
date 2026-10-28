START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE TABLE billing."FgsARCustomerPaymentInformation" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "ARCustomerId" bigint NOT NULL,
        "PaymentMethodType" character varying(20) NOT NULL,
        "CardType" character varying(20),
        "WalletType" character varying(20),
        "PaymentToken" character varying(500) NOT NULL,
        "CustomerProfileId" character varying(100),
        "CustomerPaymentProfileId" character varying(100),
        "CardHolderName" character varying(150),
        "CardBrand" character varying(50),
        "Last4" character varying(4),
        "ExpirationMonth" smallint,
        "ExpirationYear" smallint,
        "BillingAddressId" bigint,
        "BankName" character varying(100),
        "AccountType" character varying(20),
        "ReceiptEmail" character varying(254),
        "ReceiptPhoneNumber" character varying(30),
        "IsContractBilling" boolean NOT NULL DEFAULT FALSE,
        "IsDefault" boolean NOT NULL DEFAULT FALSE,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamp NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamp,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsARCustomerPaymentInformation" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_FgsARCustomerPaymentInformation_CardType" CHECK ((("PaymentMethodType" = 'CARD' AND "CardType" IN ('CREDIT', 'DEBIT')) OR ("PaymentMethodType" = 'ACH' AND "CardType" IS NULL))),
        CONSTRAINT "CK_FgsARCustomerPaymentInformation_ExpirationMonth" CHECK (("ExpirationMonth" IS NULL OR "ExpirationMonth" BETWEEN 1 AND 12)),
        CONSTRAINT "CK_FgsARCustomerPaymentInformation_Last4" CHECK (("Last4" IS NULL OR "Last4" ~ '^[0-9]{4}$')),
        CONSTRAINT "CK_FgsARCustomerPaymentInformation_PaymentMethodType" CHECK ("PaymentMethodType" IN ('CARD', 'ACH')),
        CONSTRAINT "CK_FgsARCustomerPaymentInformation_WalletType" CHECK ((("WalletType" IS NULL) OR ("PaymentMethodType" = 'CARD' AND "WalletType" IN ('APPLE_PAY', 'GOOGLE_PAY')))),
        CONSTRAINT "FK_FgsARCustomerPaymentInformation_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE billing."FgsARCustomerPaymentInformation" IS 'Stores tokenized payment information associated with an AR customer, including credit cards, debit cards, ACH payment accounts, Apple Pay, and Google Pay payment methods.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."TenantId" IS 'Identifies the tenant that owns the payment information.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CompanyId" IS 'Identifies the company within the tenant that owns the payment information.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."Id" IS 'Unique identifier for the AR customer payment information record.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."ARCustomerId" IS 'Identifies the AR customer associated with the payment information.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."PaymentMethodType" IS 'Identifies the payment method type. Supported values are CARD and ACH.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CardType" IS 'Identifies whether a card payment is a CREDIT or DEBIT card. This column is null for ACH payment methods.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."WalletType" IS 'Identifies the digital wallet used for a card payment. Supported values are APPLE_PAY and GOOGLE_PAY. Null indicates that no digital wallet is used.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."PaymentToken" IS 'Tokenized reference to the payment method provided by the payment processor. Raw card numbers, CVV values, and bank account numbers must not be stored.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CustomerProfileId" IS 'Identifies the customer profile associated with the AR customer at Authorize.Net.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CustomerPaymentProfileId" IS 'Identifies the individual saved payment profile associated with the customer at Authorize.Net.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CardHolderName" IS 'Name of the person or entity shown as the cardholder for the card payment method.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CardBrand" IS 'Card network or brand, such as Visa, Mastercard, American Express, or Discover. Applies to card payment methods.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."Last4" IS 'Last four digits of the card or bank account used for display and identification purposes.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."ExpirationMonth" IS 'Expiration month of the card payment method. This column is null for ACH payment methods.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."ExpirationYear" IS 'Expiration year of the card payment method. This column is null for ACH payment methods.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."BillingAddressId" IS 'Identifies the billing address associated with the payment information. The referenced address contains the street, city, state, county, postal code, and country information.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."BankName" IS 'Name of the financial institution associated with an ACH payment method.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."AccountType" IS 'Type of bank account associated with an ACH payment method, such as CHECKING or SAVINGS.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."ReceiptEmail" IS 'Email address to which payment receipts for charges using this payment information should be sent.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."ReceiptPhoneNumber" IS 'Phone number to which payment receipt notifications for charges using this payment information should be sent.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."IsContractBilling" IS 'Indicates whether the payment information is selected and authorized for contract billing charges.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."IsDefault" IS 'Indicates whether the payment information is the default payment information for the AR customer when no specific payment information is selected.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."IsActive" IS 'Indicates whether the payment information is currently active and available for payment processing.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CreatedOn" IS 'Date and time when the payment information was created.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."CreatedBy" IS 'Identifies the user who created the payment information.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."UpdatedOn" IS 'Date and time when the payment information was last updated.';
    COMMENT ON COLUMN billing."FgsARCustomerPaymentInformation"."UpdatedBy" IS 'Identifies the user who last updated the payment information.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE INDEX "IX_FgsARCustomerPaymentInformation_ARCustomer" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId", "ARCustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE INDEX "IX_FgsARCustomerPaymentInformation_ContractBilling" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId", "ARCustomerId", "IsContractBilling", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE INDEX "IX_FgsARCustomerPaymentInformation_PaymentMethodType" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId", "PaymentMethodType");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE INDEX "IX_FgsARCustomerPaymentInformation_TenantCompany" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE INDEX "IX_FgsARCustomerPaymentInformation_WalletType" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId", "WalletType");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE UNIQUE INDEX "UX_FgsARCustomerPaymentInformation_ContractBilling" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId", "ARCustomerId") WHERE "IsContractBilling" = true AND "IsActive" = true;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE UNIQUE INDEX "UX_FgsARCustomerPaymentInformation_CustomerPaymentProfile" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId", "CustomerPaymentProfileId") WHERE "CustomerPaymentProfileId" IS NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    CREATE UNIQUE INDEX "UX_FgsARCustomerPaymentInformation_Default" ON billing."FgsARCustomerPaymentInformation" ("TenantId", "CompanyId", "ARCustomerId") WHERE "IsDefault" = true AND "IsActive" = true;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20261026194728_AddFgsARCustomerPaymentInformation') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261026194728_AddFgsARCustomerPaymentInformation', '10.0.8');
    END IF;
END $EF$;
COMMIT;

