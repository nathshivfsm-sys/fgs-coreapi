using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsPayment : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string PaymentNumber { get; set; } = null!;

    public long CustomerId { get; set; }

    public long? ServiceLocationId { get; set; }

    public int PaymentTypeId { get; set; }

    public int PaymentMethodId { get; set; }

    public int PaymentStatusId { get; set; }

    public string? SourceType { get; set; }

    public long? SourceId { get; set; }

    public DateOnly PaymentDate { get; set; }

    public DateOnly AccountingDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public long? BankAccountId { get; set; }

    public decimal PaymentAmount { get; set; }

    public decimal AppliedAmount { get; set; }

    public DateOnly? DepositDate { get; set; }

    public string? PaymentNote { get; set; }

    public string? ExternalAccountingId { get; set; }

    public string? ExternalAccountingSyncToken { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
