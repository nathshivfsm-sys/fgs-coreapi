using Fgs.Audit.Domain.Enums;

namespace Fgs.Audit.Domain.Entities;

/// <summary>
/// Stores detailed information associated with an event, including field changes,
/// calculations, validation results, workflow actions, messages, and exceptions.
/// </summary>
public class FgsEventDetail
{
    public long Id { get; set; }

    public long EventId { get; set; }

    public AuditEventDetailType EntryType { get; set; }

    public short Sequence { get; set; } = 1;

    public string ItemName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public DateTime CreatedOn { get; set; }

    public FgsEvent? Event { get; set; }
}
