using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixSizeTiers;

internal sealed class FgsUniversalMatrixSizeTierSummaryRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixSizeTierSummaryDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            Multiplier,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixSizeTierDetailRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixSizeTierDetailDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            Multiplier,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixSizeTierLookupRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalMatrixSizeTierLookupDto ToDto() => new(Id,
            UniversalPricingServiceId,
            Name,
            DisplayOrder);
}
