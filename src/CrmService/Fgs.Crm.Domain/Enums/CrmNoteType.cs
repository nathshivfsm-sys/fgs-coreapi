namespace Fgs.Crm.Domain.Enums;

/// <summary>
/// Classification stored on <c>CrmNote.NoteTypeId</c>.
/// </summary>
public enum CrmNoteType : short
{
    General = 1,
    Internal = 2,
    Alert = 3,
    Billing = 4,
    Service = 5
}
