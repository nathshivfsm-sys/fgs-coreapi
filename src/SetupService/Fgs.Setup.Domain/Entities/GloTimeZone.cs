namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global reference table containing time zone options available for customer and user selection.
/// </summary>
public class GloTimeZone
{
    public short Id { get; set; }

    public string TimeZoneCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}
