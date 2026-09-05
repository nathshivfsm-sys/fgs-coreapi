using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimate : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string EstimateNumber { get; set; } = null!;

    public long EstimateStatusId { get; set; }

    public long EstimateTypeId { get; set; }

    public long? EstimateSourceId { get; set; }

    public long? OpportunityId { get; set; }

    public long CustomerId { get; set; }

    public long ServiceLocationId { get; set; }

    public long? WorkOrderId { get; set; }

    public long? JobTypeId { get; set; }

    public long? PaymentTermId { get; set; }

    public long? PaymentMethodId { get; set; }

    public long? Break1Id { get; set; }

    public long? Break2Id { get; set; }

    public string EstimateName { get; set; } = null!;

    public string? EstimateDescription { get; set; }

    public string? InternalNotes { get; set; }

    public string? InstallationDescription { get; set; }

    public DateOnly EstimateDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public long? QuotedByEmployeeId { get; set; }

    public long? SoldByEmployeeId { get; set; }

    public long? RecommendedByEmployeeId { get; set; }

    public bool VerificationRequired { get; set; }

    public long? VerifiedByEmployeeId { get; set; }

    public DateTimeOffset? VerifiedOn { get; set; }

    public long? SelectedEstimateOptionId { get; set; }

    public string? SignedBy { get; set; }

    public DateTimeOffset? SignedOn { get; set; }

    public long? SignatureFileId { get; set; }

    public string? TaxAuthoritySnapshotJson { get; set; }

    public long? MaterialPricingMatrixId { get; set; }

    public long? LaborPricingMatrixId { get; set; }

    public long? OtherPricingMatrixId { get; set; }

    public long? TermsConditionVersionId { get; set; }

    public decimal SubtotalAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal GrossProfitAmount { get; set; }

    public decimal GrossProfitPercent { get; set; }
}
