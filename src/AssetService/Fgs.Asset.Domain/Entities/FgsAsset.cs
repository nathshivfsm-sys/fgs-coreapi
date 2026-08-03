using Fgs.Kernel.Entities;

namespace Fgs.Asset.Domain.Entities;

public class FgsAsset : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public Guid AssetGuid { get; set; }
    public string AssetNumber { get; set; } = null!;
    public long? ServiceLocationId { get; set; }
    public long? AssetTypeId { get; set; }
    public long? AssetManufacturerId { get; set; }
    public long? AssetModelId { get; set; }
    public string? AssetDescription { get; set; }
    public string? CustomerAssetNumber { get; set; }
    public string? CustomerAssetName { get; set; }
    public string? ManufacturerName { get; set; }
    public string? ModelNumber { get; set; }
    public string? SerialNumber { get; set; }
    public DateOnly? ManufactureDate { get; set; }
    public DateOnly? InstallDate { get; set; }
    public long? InstalledWorkOrderId { get; set; }
    public bool IsInstalledByCompany { get; set; }
    public long AssetStatusId { get; set; }
    public bool IsActive { get; set; } = true;
}
