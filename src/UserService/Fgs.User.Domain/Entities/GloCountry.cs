namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of supported countries (no tenant scope).
/// </summary>
public class GloCountry : GloGeoEntityBase
{
    public long Id { get; set; }

    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;
}
