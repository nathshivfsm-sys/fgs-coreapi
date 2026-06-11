using Fgs.Kernel.Entities;

namespace Fgs.ServiceAgreement.Domain.Entities;

public class FgsServiceAgreementVisitItem : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long ServiceAgreementId { get; set; }
    public long ServiceAgreementVisitId { get; set; }
    public long? InventoryItemId { get; set; }
    public string? ItemName { get; set; }
    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }

    public FgsServiceAgreementVisit ServiceAgreementVisit { get; set; } = null!;
}
