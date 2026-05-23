namespace Fgs.User.Application.Abstractions.Provisioning;

public interface ITenantS3BucketProvisioner
{
    Task<string> EnsureTenantBucketAsync(
        long tenantId,
        string? existingBucketName,
        CancellationToken cancellationToken = default);

    Task InitializeFolderStructureAsync(
        string bucketName,
        long tenantId,
        IReadOnlyList<long> companyIds,
        CancellationToken cancellationToken = default);
}
