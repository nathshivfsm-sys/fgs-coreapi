using Fgs.Kernel.Entities;

namespace Fgs.Asset.Domain.Entities;

public class FgsAssetAttributeOption : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long AssetAttributeId { get; set; }
    public string OptionCode { get; set; } = null!;
    public string OptionName { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
