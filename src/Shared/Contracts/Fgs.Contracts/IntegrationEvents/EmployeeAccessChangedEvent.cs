namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Published when an employee's status mapping to login access changes
/// (Active → login enabled; any other status → login disabled).
/// </summary>
public sealed record EmployeeAccessChangedEvent(
    long TenantId,
    long CompanyId,
    long EmployeeId,
    Guid UserId,
    bool IsActive);
