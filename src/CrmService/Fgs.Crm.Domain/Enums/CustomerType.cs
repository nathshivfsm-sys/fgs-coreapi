namespace Fgs.Crm.Domain.Enums;

/// <summary>
/// Customer classification for a service location.
/// </summary>
public enum CustomerType : long
{
    Residential = 1,
    Commercial = 2,
    PropertyManagement = 3,
    Builder = 4,
    Hoa = 5,
    Other = 6
}
