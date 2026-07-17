using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixFrequencyDiscounts;

internal sealed class FgsUniversalMatrixFrequencyDiscountSummaryRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal DiscountPercent { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixFrequencyDiscountSummaryDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            DiscountPercent,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixFrequencyDiscountDetailRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal DiscountPercent { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixFrequencyDiscountDetailDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            DiscountPercent,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixFrequencyDiscountLookupRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalMatrixFrequencyDiscountLookupDto ToDto() => new(Id,
            UniversalPricingServiceId,
            Name,
            DisplayOrder);
}
