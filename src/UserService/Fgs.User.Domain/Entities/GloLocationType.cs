namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of location kinds (service site, bill-to, warehouse, etc.).
/// </summary>
public class GloLocationType : GloIntCatalogEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
