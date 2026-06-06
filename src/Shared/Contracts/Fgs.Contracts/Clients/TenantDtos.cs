namespace Fgs.Contracts.Clients;

public sealed record TenantDto(
    long Id,
    string Code,
    string Name,
    short FgsTenantStatusId,
    string? StorageBucketName);

public sealed record TenantCompanyDto(
    long Id,
    long TenantId,
    long CompanyNumber,
    Guid CompanyGuid,
    string Code,
    string Name,
    bool IsActive);

public sealed record UpdateTenantStatusRequest(short FgsTenantStatusId);

public sealed record UpdateTenantStorageBucketRequest(string StorageBucketName);

public sealed record ProvisionTenantBucketRequest(
    long TenantId,
    string? ExistingBucketName,
    IReadOnlyList<long> CompanyNumbers);

public sealed record ProvisionTenantBucketResponse(string BucketName);
