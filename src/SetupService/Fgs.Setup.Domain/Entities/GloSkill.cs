namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global technician skill catalog scoped to a business type and trade.
/// </summary>
public class GloSkill : GloEntityBase
{
    public short Id { get; set; }

    public int BusinessTypeId { get; set; }

    public short TradeId { get; set; }

    public string SkillCode { get; set; } = null!;

    public string SkillName { get; set; } = null!;

    public string? Description { get; set; }

    public bool RequiresCertification { get; set; }
}
