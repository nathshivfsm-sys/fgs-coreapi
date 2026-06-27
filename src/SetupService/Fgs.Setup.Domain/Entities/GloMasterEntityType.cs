namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of entity kinds that can own locations, attachments, etc.
/// </summary>
public class GloMasterEntityType : GloEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public bool IsDocumentAllowed { get; set; }

    public int SortOrder { get; set; }
}
