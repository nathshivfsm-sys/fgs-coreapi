namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of supported countries (no tenant scope).
/// </summary>
public class GloCountry : GloActiveOnlyEntityBase
{
    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public string? CurrencyCode { get; set; }
}
