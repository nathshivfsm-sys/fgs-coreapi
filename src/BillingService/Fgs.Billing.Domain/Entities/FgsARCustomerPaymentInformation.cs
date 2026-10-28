using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsARCustomerPaymentInformation : ITenantCompanyScoped, ISoftDeletable
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long ARCustomerId { get; set; }

    public string PaymentMethodType { get; set; } = null!;

    public string? CardType { get; set; }

    public string? WalletType { get; set; }

    public string PaymentToken { get; set; } = null!;

    public string? CustomerProfileId { get; set; }

    public string? CustomerPaymentProfileId { get; set; }

    public string? CardHolderName { get; set; }

    public string? CardBrand { get; set; }

    public string? Last4 { get; set; }

    public short? ExpirationMonth { get; set; }

    public short? ExpirationYear { get; set; }

    public long? BillingAddressId { get; set; }

    public string? BankName { get; set; }

    public string? AccountType { get; set; }

    public string? ReceiptEmail { get; set; }

    public string? ReceiptPhoneNumber { get; set; }

    public bool IsContractBilling { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
