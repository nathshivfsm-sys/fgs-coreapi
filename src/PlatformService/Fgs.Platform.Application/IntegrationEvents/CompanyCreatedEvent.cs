namespace Fgs.Platform.Application.IntegrationEvents;

public sealed record CompanyCreatedEvent(
    long TenantId,
    long CompanyId,
    string CompanyName,
    string AdminEmail);
