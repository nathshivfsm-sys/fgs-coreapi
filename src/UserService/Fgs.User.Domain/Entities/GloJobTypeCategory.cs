namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global job type category catalog scoped to a business type (e.g. HVAC → AC).
/// </summary>
public class GloJobTypeCategory : GloEntityBase
{
    public short Id { get; set; }

    public int BusinessTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
