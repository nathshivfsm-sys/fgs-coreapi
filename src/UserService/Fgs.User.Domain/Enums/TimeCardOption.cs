namespace Fgs.User.Domain.Enums;

/// <summary>
/// Technician time tracking workflow. Stored as smallint on tenant.FgsTenantServiceSetup.
/// </summary>
public enum TimeCardOption : short
{
    None = 1,

    CheckInCheckOut = 2,

    DispatchArriveComplete = 3,

    DispatchArriveCompleteDocumentation = 4
}
