namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global reusable service action / work type (install, repair, replace, etc.).
/// </summary>
public class GloSubCategory : GloEntityBase
{
    public short Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
