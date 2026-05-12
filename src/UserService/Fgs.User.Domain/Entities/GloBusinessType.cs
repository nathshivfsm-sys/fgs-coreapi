namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of business / company categories (e.g. plumbing, HVAC).
/// </summary>
public class GloBusinessType : GloIntCatalogEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
