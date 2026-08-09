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

internal sealed class FgsUniversalPricingServiceDetailRow
{
    public long Id { get; set; }
    public string UniversalPricingServiceCode { get; set; } = null!;
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalPricingServiceDetailDto ToDto() =>
        new(Id, UniversalPricingServiceCode, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalPricingServiceLookupRow
{
    public long Id { get; set; }
    public string UniversalPricingServiceCode { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalPricingServiceLookupDto ToDto() =>
        new(Id, UniversalPricingServiceCode, DisplayOrder);
}
