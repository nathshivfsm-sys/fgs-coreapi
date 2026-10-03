namespace Fgs.Crm.Domain.Enums;

/// <summary>
/// Channel stored on <c>CrmContactCommunication.CommunicationTypeId</c>.
/// The database check allows 1-7; only email and phone are assigned.
/// </summary>
public enum ContactCommunicationType : short
{
    Email = 1,
    Phone = 2
}
