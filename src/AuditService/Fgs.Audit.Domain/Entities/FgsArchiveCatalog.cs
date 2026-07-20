namespace Fgs.Audit.Domain.Entities;

/// <summary>
/// Maintains an inventory of archived event partitions.
/// </summary>
public class FgsArchiveCatalog
{
    public long Id { get; set; }

    public DateOnly ArchiveMonth { get; set; }

    public string StoragePath { get; set; } = null!;

    public long FileSize { get; set; }

    public DateTime CreatedOn { get; set; }
}
