using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixTiers;

internal sealed class FgsUniversalMatrixTierSummaryRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixTierSummaryDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            Multiplier,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixTierDetailRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixTierDetailDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            Multiplier,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixTierLookupRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalMatrixTierLookupDto ToDto() => new(Id,
            UniversalPricingServiceId,
            Name,
            DisplayOrder);
}
