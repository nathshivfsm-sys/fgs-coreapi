namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of accounting system / package choices (QuickBooks, Sage, none, etc.).
/// </summary>
public class GloAccountingIntegrationType : GloIntCatalogEntityBase
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
