namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global service size tiers and their default pricing multipliers.
/// </summary>
public class GloUniversalMatrixSizeTier
{
    public int Id { get; set; }

    public short UniversalPricingServiceId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Multiplier { get; set; } = 1.0000m;

    public short DisplayOrder { get; set; } = 1;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }
}
