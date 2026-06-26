namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of business / company categories (e.g. plumbing, HVAC).
/// </summary>
public class GloBusinessType : GloEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
