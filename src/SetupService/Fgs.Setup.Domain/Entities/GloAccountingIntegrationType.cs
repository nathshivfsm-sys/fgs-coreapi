namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of accounting system / package choices (QuickBooks, Sage, none, etc.).
/// </summary>
public class GloAccountingIntegrationType : GloEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
