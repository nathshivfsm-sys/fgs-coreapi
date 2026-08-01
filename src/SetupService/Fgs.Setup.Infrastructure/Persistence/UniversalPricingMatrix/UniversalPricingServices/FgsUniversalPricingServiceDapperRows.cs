using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalPricingServices;

internal sealed class FgsUniversalPricingServiceSummaryRow
{
    public long Id { get; set; }
    public string UniversalPricingServiceCode { get; set; } = null!;
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalPricingServiceSummaryDto ToDto() =>
        new(Id, UniversalPricingServiceCode, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalPricingServiceHeaderRow
{
    public long Id { get; set; }
    public string UniversalPricingServiceCode { get; set; } = null!;
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

internal sealed class FgsUniversalMatrixTierRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixTierDetailDto ToDto() =>
        new(Id, Name, Multiplier, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalMatrixSizeTierRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixSizeTierDetailDto ToDto() =>
        new(Id, Name, Multiplier, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalMatrixItemRow
{
    public long Id { get; set; }
    public string ItemName { get; set; } = null!;
    public string UnitType { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixItemDetailDto ToDto() =>
        new(Id, ItemName, UnitType, BasePrice, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalMatrixFrequencyDiscountRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal DiscountPercent { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixFrequencyDiscountDetailDto ToDto() =>
        new(Id, Name, DiscountPercent, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalMatrixOneTimeFeeRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixOneTimeFeeDetailDto ToDto() =>
        new(Id, Name, Amount, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalMatrixAddOnRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string UnitType { get; set; } = null!;
    public decimal Price { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixAddOnDetailDto ToDto() =>
        new(Id, Name, UnitType, Price, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalPricingServiceLookupRow
{
    public long Id { get; set; }
    public string UniversalPricingServiceCode { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalPricingServiceLookupDto ToDto() =>
        new(Id, UniversalPricingServiceCode, DisplayOrder);
}
