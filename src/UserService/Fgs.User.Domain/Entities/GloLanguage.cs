namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of supported languages / locales.
/// </summary>
public class GloLanguage : GloActiveOnlyEntityBase
{
    public string LanguageCode { get; set; } = null!;

    public string LanguageName { get; set; } = null!;
}
