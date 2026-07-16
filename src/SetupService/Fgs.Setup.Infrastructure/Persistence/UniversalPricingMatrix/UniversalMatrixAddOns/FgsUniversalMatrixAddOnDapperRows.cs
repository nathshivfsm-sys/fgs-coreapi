using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixAddOns;

internal sealed class FgsUniversalMatrixAddOnSummaryRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public string UnitType { get; set; } = null!;
    public decimal Price { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixAddOnSummaryDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            UnitType,
            Price,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixAddOnDetailRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public string UnitType { get; set; } = null!;
    public decimal Price { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixAddOnDetailDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            Name,
            UnitType,
            Price,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixAddOnLookupRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalMatrixAddOnLookupDto ToDto() => new(Id,
            UniversalPricingServiceId,
            Name,
            DisplayOrder);
}
