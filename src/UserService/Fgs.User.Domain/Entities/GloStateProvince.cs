namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of states/provinces linked to <see cref="GloCountry"/>.
/// </summary>
public class GloStateProvince : GloGeoEntityBase
{
    public long Id { get; set; }

    public long GloCountryId { get; set; }

    public string RegionCode { get; set; } = null!;

    public string RegionName { get; set; } = null!;

    public GloCountry? Country { get; set; }
}
