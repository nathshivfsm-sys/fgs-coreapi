namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global default service zones used for onboarding seed data.
/// </summary>
public class GloZone : GloEntityBase
{
    public short Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
