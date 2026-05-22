namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global service category catalog scoped to a business type (e.g. HVAC → AC).
/// </summary>
public class GloCategory : GloEntityBase
{
    public short Id { get; set; }

    public int BusinessTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
