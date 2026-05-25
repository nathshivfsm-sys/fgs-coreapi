using Fgs.User.Application.Abstractions.Provisioning;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.IntegrationEvents;
using Fgs.User.Application.TenantProvisioning;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Infrastructure.Provisioning;

public sealed class TenantProvisioningOrchestrator(
    FgsUserDbContext dbContext,
    ITenantDataSeedingEngine seedingEngine,
    ITenantS3BucketProvisioner s3Provisioner,
    IDateTimeProvider dateTime,
    ILogger<TenantProvisioningOrchestrator> logger) : ITenantProvisioningOrchestrator
{
    public async Task ProvisionAsync(
        TenantProvisionRequestedEvent request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Starting tenant provisioning for tenant {TenantId}, correlation {CorrelationId}",
            request.TenantId,
            request.CorrelationId);

        var tenant = await dbContext.FgsTenants
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken)
            ?? throw new InvalidOperationException(AuthErrorMessages.TenantNotFound);

        if (tenant.FgsTenantStatusId == TenantStatusIds.Active
            && !string.IsNullOrWhiteSpace(tenant.StorageBucketName))
        {
            logger.LogInformation(
                "Tenant {TenantId} is already active with bucket {Bucket}; skipping provisioning",
                request.TenantId,
                tenant.StorageBucketName);
            return;
        }

        try
        {
            tenant.FgsTenantStatusId = TenantStatusIds.Provisioning;
            tenant.UpdatedOn = dateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);

            await seedingEngine.SeedTenantDataAsync(
                request.TenantId,
                request.CompanyId,
                cancellationToken);

            var bucketName = await s3Provisioner.EnsureTenantBucketAsync(
                request.TenantId,
                tenant.StorageBucketName,
                cancellationToken);

            var companyIds = await dbContext.FgsTenantCompanies
                .AsNoTracking()
                .Where(c => c.TenantId == request.TenantId)
                .Select(c => c.CompanyNumber)
                .ToListAsync(cancellationToken);

            await s3Provisioner.InitializeFolderStructureAsync(
                bucketName,
                request.TenantId,
                companyIds,
                cancellationToken);

            tenant.StorageBucketName = bucketName;
            tenant.FgsTenantStatusId = TenantStatusIds.Active;
            tenant.IsActive = true;
            tenant.UpdatedOn = dateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Tenant provisioning completed for tenant {TenantId}, bucket {Bucket}",
                request.TenantId,
                bucketName);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Tenant provisioning failed for tenant {TenantId}, correlation {CorrelationId}",
                request.TenantId,
                request.CorrelationId);

            tenant.FgsTenantStatusId = TenantStatusIds.ProvisioningFailed;
            tenant.UpdatedOn = dateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
            throw;
        }
    }
}
