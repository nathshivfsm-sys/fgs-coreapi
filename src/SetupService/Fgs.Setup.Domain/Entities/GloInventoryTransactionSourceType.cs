namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of business processes that generate inventory transactions.
/// </summary>
public class GloInventoryTransactionSourceType
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int SortOrder { get; set; } = 1;

    public bool IsSystem { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
