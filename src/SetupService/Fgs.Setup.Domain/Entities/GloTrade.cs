namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global technician trade catalog scoped to a business type (e.g. HVAC, Plumbing).
/// </summary>
public class GloTrade
{
    public short Id { get; set; }

    public int BusinessTypeId { get; set; }

    public string TradeCode { get; set; } = null!;

    public string TradeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
