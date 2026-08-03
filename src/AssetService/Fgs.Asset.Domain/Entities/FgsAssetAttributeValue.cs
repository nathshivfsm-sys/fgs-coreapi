using Fgs.Kernel.Entities;

namespace Fgs.Asset.Domain.Entities;

public class FgsAssetAttributeValue : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long AssetId { get; set; }
    public long AssetAttributeId { get; set; }
    public long? OptionId { get; set; }
    public string? ValueText { get; set; }
    public int? ValueInteger { get; set; }
    public decimal? ValueDecimal { get; set; }
    public DateOnly? ValueDate { get; set; }
    public bool? ValueBoolean { get; set; }
}
