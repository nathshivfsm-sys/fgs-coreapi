namespace Fgs.Audit.Domain.Entities;

/// <summary>
/// Associates documents with audit events. Document metadata and storage are managed by the Document Service.
/// </summary>
public class FgsEventAttachment
{
    public long Id { get; set; }

    public long EventId { get; set; }

    public long DocumentId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedOn { get; set; }

    public FgsEvent? Event { get; set; }
}
