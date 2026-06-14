using Fgs.Kernel.Entities;

namespace Fgs.Asset.Domain.Entities;

public class FgsAssetAttribute : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long AssetTypeId { get; set; }
    public string AttributeCode { get; set; } = null!;
    public string AttributeName { get; set; } = null!;
    public string InputType { get; set; } = null!;
    public long? DefaultOptionId { get; set; }
    public string? DefaultValueText { get; set; }
    public int? DefaultValueInteger { get; set; }
    public decimal? DefaultValueDecimal { get; set; }
    public DateOnly? DefaultValueDate { get; set; }
    public bool? DefaultValueBoolean { get; set; }
    public bool IsRequired { get; set; }
    public bool IsSearchable { get; set; } = true;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
