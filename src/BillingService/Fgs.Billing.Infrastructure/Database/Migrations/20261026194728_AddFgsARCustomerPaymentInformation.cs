using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Billing.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsARCustomerPaymentInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsARCustomerPaymentInformation",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the tenant that owns the payment information."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the company within the tenant that owns the payment information."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the AR customer payment information record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ARCustomerId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the AR customer associated with the payment information."),
                    PaymentMethodType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Identifies the payment method type. Supported values are CARD and ACH."),
                    CardType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Identifies whether a card payment is a CREDIT or DEBIT card. This column is null for ACH payment methods."),
                    WalletType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Identifies the digital wallet used for a card payment. Supported values are APPLE_PAY and GOOGLE_PAY. Null indicates that no digital wallet is used."),
                    PaymentToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Tokenized reference to the payment method provided by the payment processor. Raw card numbers, CVV values, and bank account numbers must not be stored."),
                    CustomerProfileId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Identifies the customer profile associated with the AR customer at Authorize.Net."),
                    CustomerPaymentProfileId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Identifies the individual saved payment profile associated with the customer at Authorize.Net."),
                    CardHolderName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true, comment: "Name of the person or entity shown as the cardholder for the card payment method."),
                    CardBrand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Card network or brand, such as Visa, Mastercard, American Express, or Discover. Applies to card payment methods."),
                    Last4 = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true, comment: "Last four digits of the card or bank account used for display and identification purposes."),
                    ExpirationMonth = table.Column<short>(type: "smallint", nullable: true, comment: "Expiration month of the card payment method. This column is null for ACH payment methods."),
                    ExpirationYear = table.Column<short>(type: "smallint", nullable: true, comment: "Expiration year of the card payment method. This column is null for ACH payment methods."),
                    BillingAddressId = table.Column<long>(type: "bigint", nullable: true, comment: "Identifies the billing address associated with the payment information. The referenced address contains the street, city, state, county, postal code, and country information."),
                    BankName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Name of the financial institution associated with an ACH payment method."),
                    AccountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Type of bank account associated with an ACH payment method, such as CHECKING or SAVINGS."),
                    ReceiptEmail = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true, comment: "Email address to which payment receipts for charges using this payment information should be sent."),
                    ReceiptPhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true, comment: "Phone number to which payment receipt notifications for charges using this payment information should be sent."),
                    IsContractBilling = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the payment information is selected and authorized for contract billing charges."),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the payment information is the default payment information for the AR customer when no specific payment information is selected."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the payment information is currently active and available for payment processing."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time when the payment information was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the user who created the payment information."),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true, comment: "Date and time when the payment information was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "Identifies the user who last updated the payment information.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsARCustomerPaymentInformation", x => x.Id);
                    table.CheckConstraint("CK_FgsARCustomerPaymentInformation_CardType", "((\"PaymentMethodType\" = 'CARD' AND \"CardType\" IN ('CREDIT', 'DEBIT')) OR (\"PaymentMethodType\" = 'ACH' AND \"CardType\" IS NULL))");
                    table.CheckConstraint("CK_FgsARCustomerPaymentInformation_ExpirationMonth", "(\"ExpirationMonth\" IS NULL OR \"ExpirationMonth\" BETWEEN 1 AND 12)");
                    table.CheckConstraint("CK_FgsARCustomerPaymentInformation_Last4", "(\"Last4\" IS NULL OR \"Last4\" ~ '^[0-9]{4}$')");
                    table.CheckConstraint("CK_FgsARCustomerPaymentInformation_PaymentMethodType", "\"PaymentMethodType\" IN ('CARD', 'ACH')");
                    table.CheckConstraint("CK_FgsARCustomerPaymentInformation_WalletType", "((\"WalletType\" IS NULL) OR (\"PaymentMethodType\" = 'CARD' AND \"WalletType\" IN ('APPLE_PAY', 'GOOGLE_PAY')))");
                    table.ForeignKey(
                        name: "FK_FgsARCustomerPaymentInformation_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tokenized payment information associated with an AR customer, including credit cards, debit cards, ACH payment accounts, Apple Pay, and Google Pay payment methods.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsARCustomerPaymentInformation_ARCustomer",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId", "ARCustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsARCustomerPaymentInformation_ContractBilling",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId", "ARCustomerId", "IsContractBilling", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsARCustomerPaymentInformation_PaymentMethodType",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId", "PaymentMethodType" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsARCustomerPaymentInformation_TenantCompany",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsARCustomerPaymentInformation_WalletType",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId", "WalletType" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsARCustomerPaymentInformation_ContractBilling",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId", "ARCustomerId" },
                unique: true,
                filter: "\"IsContractBilling\" = true AND \"IsActive\" = true");

            migrationBuilder.CreateIndex(
                name: "UX_FgsARCustomerPaymentInformation_CustomerPaymentProfile",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId", "CustomerPaymentProfileId" },
                unique: true,
                filter: "\"CustomerPaymentProfileId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_FgsARCustomerPaymentInformation_Default",
                schema: "billing",
                table: "FgsARCustomerPaymentInformation",
                columns: new[] { "TenantId", "CompanyId", "ARCustomerId" },
                unique: true,
                filter: "\"IsDefault\" = true AND \"IsActive\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsARCustomerPaymentInformation",
                schema: "billing");
        }
    }
}
