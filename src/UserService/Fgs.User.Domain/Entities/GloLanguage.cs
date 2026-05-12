namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of supported languages / locales.
/// </summary>
public class GloLanguage : GloIntCatalogEntityBase
{
    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;
}
