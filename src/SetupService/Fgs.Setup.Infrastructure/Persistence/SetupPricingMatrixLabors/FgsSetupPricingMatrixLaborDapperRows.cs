using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLabors;

internal sealed class FgsSetupPricingMatrixLaborRow
{
    public long Id { get; set; }
    public long PricingMatrixId { get; set; }
    public int LaborRateTypeId { get; set; }
    public long? TechSkillLevelId { get; set; }
    public decimal BaseRate { get; set; }
    public decimal? OvertimeMultiplier { get; set; }
    public decimal? DoubleTimeMultiplier { get; set; }
    public decimal? DiscountPercent { get; set; }
    public bool IsActive { get; set; }
    public FgsSetupPricingMatrixLaborDetailDto ToDetailDto() => new(Id, PricingMatrixId, LaborRateTypeId, TechSkillLevelId, BaseRate, OvertimeMultiplier, DoubleTimeMultiplier, DiscountPercent, IsActive);
    public FgsSetupPricingMatrixLaborSummaryDto ToSummaryDto() => new(Id, PricingMatrixId, LaborRateTypeId, TechSkillLevelId, BaseRate, OvertimeMultiplier, DoubleTimeMultiplier, DiscountPercent, IsActive);
}

internal sealed class FgsSetupPricingMatrixLaborLookupRow
{
    public long Id { get; set; }
    public long PricingMatrixId { get; set; }
    public int LaborRateTypeId { get; set; }
    public FgsSetupPricingMatrixLaborLookupDto ToDto() => new(Id, PricingMatrixId, LaborRateTypeId);
}
