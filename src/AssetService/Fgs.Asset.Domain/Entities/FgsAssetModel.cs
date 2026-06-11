using Fgs.Kernel.Entities;

namespace Fgs.Asset.Domain.Entities;

public class FgsAssetModel : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long AssetTypeId { get; set; }
    public long AssetManufacturerId { get; set; }
    public string ModelNumber { get; set; } = null!;
    public string ModelDescription { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
