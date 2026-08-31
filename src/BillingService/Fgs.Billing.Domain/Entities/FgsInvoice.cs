using Fgs.Kernel.Entities;

namespace Fgs.Billing.Domain.Entities;

public class FgsInvoice : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public short InvoiceTypeId { get; set; }

    public long CustomerId { get; set; }

    public long ServiceLocationId { get; set; }

    public long? WorkOrderId { get; set; }

    public long? ProjectId { get; set; }

    public long? ServiceAgreementId { get; set; }

    public long? MaintenanceVisitId { get; set; }

    public string? ServiceJobNum { get; set; }

    public bool IsAgreementBilling { get; set; }

    public bool IsRecurringInvoice { get; set; }

    public long? RecurringScheduleId { get; set; }

    public string? WorkOrderNumber { get; set; }

    public int? JobTypeId { get; set; }

    public long? LeadEmployeeId { get; set; }

    public string? CustomerPONumber { get; set; }

    public DateOnly InvoiceDate { get; set; }

    public DateOnly AccountingDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public int? NetTermId { get; set; }

    public int? PreferredPaymentMethodId { get; set; }

    public long? LaborPricingMatrixId { get; set; }

    public long? MaterialPricingMatrixId { get; set; }

    public long? OtherPricingMatrixId { get; set; }

    public int? GLBreak1Id { get; set; }

    public int? GLBreak2Id { get; set; }

    public string? TaxingAuthorityJson { get; set; }

    public string? BillToAddressJson { get; set; }

    public string? ServiceLocationAddressJson { get; set; }

    public string? CompanyAddressJson { get; set; }

    public long? InvoiceTemplateId { get; set; }

    public bool IsSigned { get; set; }

    public DateTime? SignedOn { get; set; }

    public decimal InvoiceSubtotal { get; set; }

    public decimal TotalDiscount { get; set; }

    public decimal TaxableAmount { get; set; }

    public decimal TotalTax { get; set; }

    public decimal InvoiceTotal { get; set; }

    public decimal AppliedAmount { get; set; }

    public decimal BalanceDue { get; set; }

    public bool IsApproved { get; set; }

    public long? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public bool IsPosted { get; set; }

    public long? PostedBy { get; set; }

    public DateTime? PostedOn { get; set; }

    public long? InvoiceBatchId { get; set; }

    public long? TermsConditionVersionId { get; set; }

    public string? ExternalAccountingId { get; set; }

    public string? ExternalAccountingSyncToken { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }

    public long RowVersion { get; set; } = 1;
}
