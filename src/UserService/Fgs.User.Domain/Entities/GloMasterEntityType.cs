namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of entity kinds that can own locations, attachments, etc.
/// </summary>
public class GloMasterEntityType : GloOptionalAuditEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public bool IsDocumentAllowed { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }
}
