namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of states/provinces linked to <see cref="GloCountry"/>.
/// </summary>
public class GloStateProvince : GloEntityBase
{
    public int Id { get; set; }

    public string CountryCode { get; set; } = null!;

    public string StateProvinceCode { get; set; } = null!;

    public string StateProvinceName { get; set; } = null!;

    public GloCountry? Country { get; set; }
}
