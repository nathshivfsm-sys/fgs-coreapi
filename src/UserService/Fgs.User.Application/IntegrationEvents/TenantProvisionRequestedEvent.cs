namespace Fgs.User.Application.IntegrationEvents;

public sealed record TenantProvisionRequestedEvent(
    long TenantId,
    long CompanyId,
    string TenantCode,
    Guid CorrelationId,
    Guid? UserId = null);
