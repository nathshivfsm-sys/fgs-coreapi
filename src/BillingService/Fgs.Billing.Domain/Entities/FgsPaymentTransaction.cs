using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsPaymentTransaction : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long PaymentId { get; set; }

    public int TransactionTypeId { get; set; }

    public int TransactionMethodId { get; set; }

    public int PaymentProcessorId { get; set; }

    public string TransactionId { get; set; } = null!;

    public string? OriginalTransactionId { get; set; }

    public string? AuthorizationCode { get; set; }

    public string? ProcessorStatus { get; set; }

    public string? CardHolderName { get; set; }

    public string? CardLast4 { get; set; }

    public string? BankAccountLast4 { get; set; }

    public decimal TransactionAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public long? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Source { get; set; }

    public string? TransactionDataJson { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
