namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global unit-of-measure catalog (count, length, weight, volume, time, etc.).
/// </summary>
public class GloUnitOfMeasure
{
    public int Id { get; set; }

    public string UnitCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public string? Description { get; set; }

    public string UnitType { get; set; } = null!;

    public short DecimalPlaces { get; set; } = 2;

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
