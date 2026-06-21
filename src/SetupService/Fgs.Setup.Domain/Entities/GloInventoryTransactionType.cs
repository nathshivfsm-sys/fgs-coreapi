namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of specific inventory ledger transaction types.
/// </summary>
public class GloInventoryTransactionType
{
    public int Id { get; set; }

    public int InventoryTransactionSourceTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int SortOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
