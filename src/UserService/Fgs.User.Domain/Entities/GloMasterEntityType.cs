namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of entity kinds that can own locations, attachments, etc.
/// </summary>
public class GloMasterEntityType
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public bool IsDocumentAllowed { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}
