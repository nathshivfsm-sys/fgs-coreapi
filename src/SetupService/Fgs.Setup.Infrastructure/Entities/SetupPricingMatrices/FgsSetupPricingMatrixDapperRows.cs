using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.SetupPricingMatrices;

internal sealed class FgsSetupPricingMatrixHeaderRow
{
    public long Id { get; init; }

    public string Code { get; init; } = null!;

    public string Name { get; init; } = null!;

    public bool IsDefault { get; init; }

    public bool IsLaborTierStructure { get; init; }

    public bool IsLaborRateBySkillLevel { get; init; }

    public short PriceAdjustmentTypeId { get; init; }

    public short Category { get; init; }

    public DateOnly EffectiveFrom { get; init; }

    public DateOnly? EffectiveTo { get; init; }

    public bool IsMobileVisible { get; init; }

    public bool IsUsedInPriceBook { get; init; }

    public bool IsActive { get; init; }

    public FgsSetupPricingMatrixSummaryDto ToSummaryDto() =>
        new(Id, Code, Name, IsDefault, IsLaborTierStructure, IsLaborRateBySkillLevel,
            PriceAdjustmentTypeId, Category, EffectiveFrom, EffectiveTo, IsMobileVisible, IsUsedInPriceBook, IsActive);

    public FgsSetupPricingMatrixDetailDto ToDetailDto() =>
        new(Id, Code, Name, IsDefault, IsLaborTierStructure, IsLaborRateBySkillLevel,
            PriceAdjustmentTypeId, Category, EffectiveFrom, EffectiveTo, IsMobileVisible, IsUsedInPriceBook, IsActive);

    public FgsSetupPricingMatrixLookupDto ToLookupDto() =>
        new(Id, Code, Name, IsDefault, Category, IsUsedInPriceBook);

    public FgsSetupPricingMatrixFlagsDto ToFlagsDto() =>
        new(Id, IsLaborTierStructure, IsLaborRateBySkillLevel, PriceAdjustmentTypeId, IsActive);
}
