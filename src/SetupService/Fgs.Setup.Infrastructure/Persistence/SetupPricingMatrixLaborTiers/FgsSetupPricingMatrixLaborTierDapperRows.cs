using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLaborTiers;

internal sealed class FgsSetupPricingMatrixLaborTierRow
{
    public long Id { get; set; }
    public long PricingMatrixLaborId { get; set; }
    public short SequenceOrder { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Rate { get; set; }
    public long? TechSkillLevelId { get; set; }
    public bool IsActive { get; set; }
    public FgsSetupPricingMatrixLaborTierDetailDto ToDetailDto() => new(Id, PricingMatrixLaborId, SequenceOrder, DurationMinutes, Rate, TechSkillLevelId, IsActive);
    public FgsSetupPricingMatrixLaborTierSummaryDto ToSummaryDto() => new(Id, PricingMatrixLaborId, SequenceOrder, DurationMinutes, Rate, TechSkillLevelId, IsActive);
}

internal sealed class FgsSetupPricingMatrixLaborTierLookupRow
{
    public long Id { get; set; }
    public long PricingMatrixLaborId { get; set; }
    public short SequenceOrder { get; set; }
    public FgsSetupPricingMatrixLaborTierLookupDto ToDto() => new(Id, PricingMatrixLaborId, SequenceOrder);
}
