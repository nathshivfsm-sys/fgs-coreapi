using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixMaterialTiers;

internal sealed class FgsSetupPricingMatrixMaterialTierRow
{
    public long Id { get; set; }
    public long PricingMatrixId { get; set; }
    public decimal FromCost { get; set; }
    public decimal? ToCost { get; set; }
    public decimal AdjustmentValue { get; set; }
    public bool IsActive { get; set; }
    public FgsSetupPricingMatrixMaterialTierDetailDto ToDetailDto() => new(Id, PricingMatrixId, FromCost, ToCost, AdjustmentValue, IsActive);
    public FgsSetupPricingMatrixMaterialTierSummaryDto ToSummaryDto() => new(Id, PricingMatrixId, FromCost, ToCost, AdjustmentValue, IsActive);
}

internal sealed class FgsSetupPricingMatrixMaterialTierLookupRow
{
    public long Id { get; set; }
    public long PricingMatrixId { get; set; }
    public decimal FromCost { get; set; }
    public decimal AdjustmentValue { get; set; }
    public FgsSetupPricingMatrixMaterialTierLookupDto ToDto() => new(Id, PricingMatrixId, FromCost, AdjustmentValue);
}
