using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixOthers;

internal sealed class FgsSetupPricingMatrixOtherRow
{
    public long Id { get; set; }
    public long PricingMatrixId { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal? AdjustmentValue { get; set; }
    public decimal? DiscountPercent { get; set; }
    public bool IsActive { get; set; }
    public FgsSetupPricingMatrixOtherDetailDto ToDetailDto() => new(Id, PricingMatrixId, CategoryCode, Name, AdjustmentValue, DiscountPercent, IsActive);
    public FgsSetupPricingMatrixOtherSummaryDto ToSummaryDto() => new(Id, PricingMatrixId, CategoryCode, Name, AdjustmentValue, DiscountPercent, IsActive);
}

internal sealed class FgsSetupPricingMatrixOtherLookupRow
{
    public long Id { get; set; }
    public long PricingMatrixId { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public FgsSetupPricingMatrixOtherLookupDto ToDto() => new(Id, PricingMatrixId, CategoryCode, Name);
}
