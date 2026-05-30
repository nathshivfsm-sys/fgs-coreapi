namespace Fgs.Contracts.IntegrationEvents;

public sealed record CompanyCreatedEvent(
    long TenantId,
    long CompanyId,
    string CompanyName,
    string CompanyCode,
    string AdminEmail);
