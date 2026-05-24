namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global reusable job type sub-category (install, repair, replace, etc.).
/// </summary>
public class GloJobTypeSubCategory : GloEntityBase
{
    public short Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
