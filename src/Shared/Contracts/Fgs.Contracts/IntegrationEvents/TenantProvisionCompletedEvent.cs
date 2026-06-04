namespace Fgs.Contracts.IntegrationEvents;

public sealed record TenantProvisionCompletedEvent(
    long TenantId,
    long CompanyId,
    string TenantCode,
    Guid CorrelationId,
    string? StorageBucketName,
    bool SeedHadFailures);
