using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixOneTimeFees;

internal sealed class FgsUniversalMatrixOneTimeFeeSummaryRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixOneTimeFeeSummaryDto ToDto() =>
        new(Id, UniversalPricingServiceId, Name, Amount, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalMatrixOneTimeFeeDetailRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsUniversalMatrixOneTimeFeeDetailDto ToDto() =>
        new(Id, UniversalPricingServiceId, Name, Amount, DisplayOrder, IsActive);
}

internal sealed class FgsUniversalMatrixOneTimeFeeLookupRow
{
    public long Id { get; set; }
    public long UniversalPricingServiceId { get; set; }
    public string Name { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsUniversalMatrixOneTimeFeeLookupDto ToDto() =>
        new(Id, UniversalPricingServiceId, Name, DisplayOrder);
}
