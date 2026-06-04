namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of supported languages / locales.
/// </summary>
public class GloLanguage : GloEntityBase
{
    public string LanguageCode { get; set; } = null!;

    public string LanguageName { get; set; } = null!;

    public string CultureCode { get; set; } = null!;
}
