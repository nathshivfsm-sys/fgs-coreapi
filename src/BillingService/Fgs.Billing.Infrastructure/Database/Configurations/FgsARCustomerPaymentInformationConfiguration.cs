using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsARCustomerPaymentInformationConfiguration : IEntityTypeConfiguration<FgsARCustomerPaymentInformation>
{
    public void Configure(EntityTypeBuilder<FgsARCustomerPaymentInformation> entity)
    {
        entity.ToTable(
            "FgsARCustomerPaymentInformation",
            t =>
            {
                t.HasComment(
                    "Stores tokenized payment information associated with an AR customer, including credit cards, debit cards, ACH payment accounts, Apple Pay, and Google Pay payment methods.");
                t.HasCheckConstraint(
                    "CK_FgsARCustomerPaymentInformation_PaymentMethodType",
                    "\"PaymentMethodType\" IN ('CARD', 'ACH')");
                t.HasCheckConstraint(
                    "CK_FgsARCustomerPaymentInformation_CardType",
                    "((\"PaymentMethodType\" = 'CARD' AND \"CardType\" IN ('CREDIT', 'DEBIT')) OR (\"PaymentMethodType\" = 'ACH' AND \"CardType\" IS NULL))");
                t.HasCheckConstraint(
                    "CK_FgsARCustomerPaymentInformation_WalletType",
                    "((\"WalletType\" IS NULL) OR (\"PaymentMethodType\" = 'CARD' AND \"WalletType\" IN ('APPLE_PAY', 'GOOGLE_PAY')))");
                t.HasCheckConstraint(
                    "CK_FgsARCustomerPaymentInformation_ExpirationMonth",
                    "(\"ExpirationMonth\" IS NULL OR \"ExpirationMonth\" BETWEEN 1 AND 12)");
                t.HasCheckConstraint(
                    "CK_FgsARCustomerPaymentInformation_Last4",
                    "(\"Last4\" IS NULL OR \"Last4\" ~ '^[0-9]{4}$')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the AR customer payment information record.");

        entity.ConfigureTenantCompanyColumns();
        entity.Property(e => e.TenantId)
            .HasComment("Identifies the tenant that owns the payment information.");
        entity.Property(e => e.CompanyId)
            .HasComment("Identifies the company within the tenant that owns the payment information.");

        entity.Property(e => e.ARCustomerId)
            .HasComment("Identifies the AR customer associated with the payment information.");
        entity.Property(e => e.PaymentMethodType)
            .HasMaxLength(20)
            .IsRequired()
            .HasComment("Identifies the payment method type. Supported values are CARD and ACH.");
        entity.Property(e => e.CardType)
            .HasMaxLength(20)
            .HasComment("Identifies whether a card payment is a CREDIT or DEBIT card. This column is null for ACH payment methods.");
        entity.Property(e => e.WalletType)
            .HasMaxLength(20)
            .HasComment("Identifies the digital wallet used for a card payment. Supported values are APPLE_PAY and GOOGLE_PAY. Null indicates that no digital wallet is used.");
        entity.Property(e => e.PaymentToken)
            .HasMaxLength(500)
            .IsRequired()
            .HasComment("Tokenized reference to the payment method provided by the payment processor. Raw card numbers, CVV values, and bank account numbers must not be stored.");
        entity.Property(e => e.CustomerProfileId)
            .HasMaxLength(100)
            .HasComment("Identifies the customer profile associated with the AR customer at Authorize.Net.");
        entity.Property(e => e.CustomerPaymentProfileId)
            .HasMaxLength(100)
            .HasComment("Identifies the individual saved payment profile associated with the customer at Authorize.Net.");
        entity.Property(e => e.CardHolderName)
            .HasMaxLength(150)
            .HasComment("Name of the person or entity shown as the cardholder for the card payment method.");
        entity.Property(e => e.CardBrand)
            .HasMaxLength(50)
            .HasComment("Card network or brand, such as Visa, Mastercard, American Express, or Discover. Applies to card payment methods.");
        entity.Property(e => e.Last4)
            .HasMaxLength(4)
            .HasComment("Last four digits of the card or bank account used for display and identification purposes.");
        entity.Property(e => e.ExpirationMonth)
            .HasComment("Expiration month of the card payment method. This column is null for ACH payment methods.");
        entity.Property(e => e.ExpirationYear)
            .HasComment("Expiration year of the card payment method. This column is null for ACH payment methods.");
        entity.Property(e => e.BillingAddressId)
            .HasComment("Identifies the billing address associated with the payment information. The referenced address contains the street, city, state, county, postal code, and country information.");
        entity.Property(e => e.BankName)
            .HasMaxLength(100)
            .HasComment("Name of the financial institution associated with an ACH payment method.");
        entity.Property(e => e.AccountType)
            .HasMaxLength(20)
            .HasComment("Type of bank account associated with an ACH payment method, such as CHECKING or SAVINGS.");
        entity.Property(e => e.ReceiptEmail)
            .HasMaxLength(254)
            .HasComment("Email address to which payment receipts for charges using this payment information should be sent.");
        entity.Property(e => e.ReceiptPhoneNumber)
            .HasMaxLength(30)
            .HasComment("Phone number to which payment receipt notifications for charges using this payment information should be sent.");
        entity.Property(e => e.IsContractBilling)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the payment information is selected and authorized for contract billing charges.");
        entity.Property(e => e.IsDefault)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the payment information is the default payment information for the AR customer when no specific payment information is selected.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the payment information is currently active and available for payment processing.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the payment information was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("Identifies the user who created the payment information.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamp")
            .HasComment("Date and time when the payment information was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("Identifies the user who last updated the payment information.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ARCustomerId }, "IX_FgsARCustomerPaymentInformation_ARCustomer")
            .HasDatabaseName("IX_FgsARCustomerPaymentInformation_ARCustomer");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ARCustomerId, e.IsContractBilling, e.IsActive })
            .HasDatabaseName("IX_FgsARCustomerPaymentInformation_ContractBilling");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PaymentMethodType })
            .HasDatabaseName("IX_FgsARCustomerPaymentInformation_PaymentMethodType");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WalletType })
            .HasDatabaseName("IX_FgsARCustomerPaymentInformation_WalletType");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsARCustomerPaymentInformation_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ARCustomerId }, "UX_FgsARCustomerPaymentInformation_ContractBilling")
            .IsUnique()
            .HasFilter("\"IsContractBilling\" = true AND \"IsActive\" = true")
            .HasDatabaseName("UX_FgsARCustomerPaymentInformation_ContractBilling");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ARCustomerId }, "UX_FgsARCustomerPaymentInformation_Default")
            .IsUnique()
            .HasFilter("\"IsDefault\" = true AND \"IsActive\" = true")
            .HasDatabaseName("UX_FgsARCustomerPaymentInformation_Default");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerPaymentProfileId })
            .IsUnique()
            .HasFilter("\"CustomerPaymentProfileId\" IS NOT NULL")
            .HasDatabaseName("UX_FgsARCustomerPaymentInformation_CustomerPaymentProfile");
    }
}
