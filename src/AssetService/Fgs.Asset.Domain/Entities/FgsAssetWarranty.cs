using Fgs.Kernel.Entities;

namespace Fgs.Asset.Domain.Entities;

public class FgsAssetWarranty : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long AssetId { get; set; }
    public string WarrantyType { get; set; } = null!;
    public string? WarrantyProvider { get; set; }
    public string? WarrantyNumber { get; set; }
    public string? RegistrationNumber { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? CoverageDescription { get; set; }
}
