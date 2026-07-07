namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global seeded list of services supported by the Universal Pricing Matrix.
/// </summary>
public class GloUniversalPricingService
{
    public short Id { get; set; }

    public string ServiceCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }
}
