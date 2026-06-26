namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of supported countries (no tenant scope).
/// </summary>
public class GloCountry : GloEntityBase
{
    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public string? CurrencyCode { get; set; }
}
