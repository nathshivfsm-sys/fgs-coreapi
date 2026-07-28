namespace Fgs.Setup.Domain.Enums;

/// <summary>
/// Business process for which a Job Type can be used.
/// </summary>
public enum JobTypeUsedFor : short
{
    Service = 1,
    Maintenance = 2,
    Warranty = 3,
    Installation = 4
}
