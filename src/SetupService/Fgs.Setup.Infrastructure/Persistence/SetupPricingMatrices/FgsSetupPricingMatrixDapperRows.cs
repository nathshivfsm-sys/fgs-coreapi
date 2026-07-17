using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrices;

internal sealed class FgsSetupPricingMatrixHeaderRow
{
    public long Id { get; init; }

    public string Code { get; init; } = null!;

    public string Name { get; init; } = null!;

    public bool IsDefault { get; init; }

    public bool IsLaborTierStructure { get; init; }

    public bool IsLaborRateBySkillLevel { get; init; }

    public short PriceAdjustmentTypeId { get; init; }

    public DateOnly EffectiveFrom { get; init; }

    public DateOnly? EffectiveTo { get; init; }

    public bool IsMobileVisible { get; init; }

    public bool IsActive { get; init; }

    public FgsSetupPricingMatrixSummaryDto ToSummaryDto() =>
        new(Id, Code, Name, IsDefault, IsLaborTierStructure, IsLaborRateBySkillLevel,
            PriceAdjustmentTypeId, EffectiveFrom, EffectiveTo, IsMobileVisible, IsActive);

    public FgsSetupPricingMatrixLookupDto ToLookupDto() =>
        new(Id, Code, Name, IsDefault);
}

internal sealed class FgsSetupPricingMatrixLaborRow
{
    public long Id { get; init; }

    public long PricingMatrixId { get; init; }

    public int LaborRateTypeId { get; init; }

    public long? TechSkillLevelId { get; init; }

    public decimal BaseRate { get; init; }

    public decimal? OvertimeMultiplier { get; init; }

    public decimal? DoubleTimeMultiplier { get; init; }

    public decimal? DiscountPercent { get; init; }

    public bool IsActive { get; init; }
}

internal sealed class FgsSetupPricingMatrixLaborTierRow
{
    public long Id { get; init; }

    public long PricingMatrixLaborId { get; init; }

    public short SequenceOrder { get; init; }

    public int DurationMinutes { get; init; }

    public decimal Rate { get; init; }

    public long? TechSkillLevelId { get; init; }

    public bool IsActive { get; init; }

    public FgsSetupPricingMatrixLaborTierDetailDto ToDetailDto() =>
        new(Id, SequenceOrder, DurationMinutes, Rate, TechSkillLevelId, IsActive);
}

internal sealed class FgsSetupPricingMatrixMaterialTierRow
{
    public long Id { get; init; }

    public long PricingMatrixId { get; init; }

    public decimal FromCost { get; init; }

    public decimal? ToCost { get; init; }

    public decimal AdjustmentValue { get; init; }

    public bool IsActive { get; init; }

    public FgsSetupPricingMatrixMaterialTierDetailDto ToDetailDto() =>
        new(Id, FromCost, ToCost, AdjustmentValue, IsActive);
}

internal sealed class FgsSetupPricingMatrixOtherRow
{
    public long Id { get; init; }

    public long PricingMatrixId { get; init; }

    public string CategoryCode { get; init; } = null!;

    public string Name { get; init; } = null!;

    public decimal? AdjustmentValue { get; init; }

    public decimal? DiscountPercent { get; init; }

    public bool IsActive { get; init; }

    public FgsSetupPricingMatrixOtherItemDetailDto ToDetailDto() =>
        new(Id, CategoryCode, Name, AdjustmentValue, DiscountPercent, IsActive);
}

internal static class FgsSetupPricingMatrixDetailAssembler
{
    public static FgsSetupPricingMatrixDetailDto Assemble(
        FgsSetupPricingMatrixHeaderRow header,
        IReadOnlyList<FgsSetupPricingMatrixLaborRow> laborRows,
        IReadOnlyList<FgsSetupPricingMatrixLaborTierRow> tierRows,
        IReadOnlyList<FgsSetupPricingMatrixMaterialTierRow> materialRows,
        IReadOnlyList<FgsSetupPricingMatrixOtherRow> otherRows)
    {
        var tiersByLabor = tierRows
            .GroupBy(t => t.PricingMatrixLaborId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<FgsSetupPricingMatrixLaborTierDetailDto>)g.Select(x => x.ToDetailDto()).ToList());

        var laborLines = laborRows
            .Select(l => new FgsSetupPricingMatrixLaborLineDetailDto(
                l.Id,
                l.LaborRateTypeId,
                l.TechSkillLevelId,
                l.BaseRate,
                l.OvertimeMultiplier,
                l.DoubleTimeMultiplier,
                l.DiscountPercent,
                l.IsActive,
                tiersByLabor.TryGetValue(l.Id, out var tiers) ? tiers : Array.Empty<FgsSetupPricingMatrixLaborTierDetailDto>()))
            .ToList();

        return new FgsSetupPricingMatrixDetailDto(
            header.Id,
            header.Code,
            header.Name,
            header.IsDefault,
            header.IsLaborTierStructure,
            header.IsLaborRateBySkillLevel,
            header.PriceAdjustmentTypeId,
            header.EffectiveFrom,
            header.EffectiveTo,
            header.IsMobileVisible,
            header.IsActive,
            laborLines,
            materialRows.Select(m => m.ToDetailDto()).ToList(),
            otherRows.Select(o => o.ToDetailDto()).ToList());
    }
}
