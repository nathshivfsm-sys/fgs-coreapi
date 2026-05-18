namespace Fgs.Platform.Application.IntegrationEvents;

public sealed record CompanyCreatedEvent(
    Guid TenantId,
    Guid CompanyId,
    string CompanyName,
    string AdminEmail);
