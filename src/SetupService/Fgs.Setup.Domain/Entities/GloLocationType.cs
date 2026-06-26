namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of location kinds (service site, bill-to, warehouse, etc.).
/// </summary>
public class GloLocationType : GloEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
