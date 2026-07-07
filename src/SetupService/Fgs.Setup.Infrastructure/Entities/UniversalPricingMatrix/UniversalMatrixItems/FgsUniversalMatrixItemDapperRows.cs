using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.UniversalPricingMatrix.UniversalMatrixItems;

internal sealed class FgsUniversalMatrixItemSummaryRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string ItemName { get; set; } = null!;
    public string UnitType { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixItemSummaryDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            ItemName,
            UnitType,
            BasePrice,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixItemDetailRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string ItemName { get; set; } = null!;
    public string UnitType { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixItemDetailDto ToDto() =>
        new(
            Id,
            UniversalPricingServiceId,
            ItemName,
            UnitType,
            BasePrice,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsUniversalMatrixItemLookupRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string ItemName { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalMatrixItemLookupDto ToDto() => new(Id,
            UniversalPricingServiceId,
            ItemName,
            DisplayOrder);
}
