namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global inventory category catalog scoped to a business type.
/// </summary>
public class GloInventoryCategory : GloEntityBase
{
    public int Id { get; set; }

    public int BusinessTypeId { get; set; }

    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
