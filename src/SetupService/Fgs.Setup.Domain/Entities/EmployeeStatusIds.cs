namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Allowed <see cref="FgsEmployee.StatusId"/> values.
/// </summary>
public static class EmployeeStatusIds
{
    public const short Active = 1;

    public const short Inactive = 2;

    public const short LeaveOfAbsence = 3;

    public const short Terminated = 4;
}
