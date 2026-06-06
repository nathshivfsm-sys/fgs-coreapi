namespace Fgs.Contracts.Requests;

public sealed record ProvisionTenantRequest(
    long TenantId,
    long CompanyId,
    string TenantCode,
    Guid CorrelationId,
    Guid? UserId = null);
