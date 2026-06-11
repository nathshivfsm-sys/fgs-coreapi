using Fgs.Kernel.Entities;

namespace Fgs.ServiceAgreement.Domain.Entities;

public class FgsServiceAgreement : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string AgreementNumber { get; set; } = null!;
    public long CustomerId { get; set; }
    public long CustomerLocationId { get; set; }
    public long? EstimateId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public long Break1Id { get; set; }
    public long Break2Id { get; set; }
    public long JobTypeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public short ServiceAgreementStatusId { get; set; }
    public short VisitFrequencyId { get; set; }
    public short BillingFrequencyId { get; set; }
    public decimal ContractAmount { get; set; }
    public decimal LaborDiscountPercent { get; set; }
    public decimal MaterialDiscountPercent { get; set; }
    public bool AutoRenew { get; set; }
    public long? RenewedByServiceAgreementId { get; set; }
    public DateOnly? SoldDate { get; set; }
    public long? SoldByEmployeeId { get; set; }
    public DateTimeOffset? ActivatedOn { get; set; }
    public DateTimeOffset? CancelledOn { get; set; }
    public string? ExternalEntityId { get; set; }
    public string? ExternalVersion { get; set; }
}
