using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using TenantStatusIds = Fgs.Contracts.Clients.TenantStatusIds;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Features.TenantProvisioning;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Infrastructure.Provisioning;

public sealed class TenantProvisioningOrchestrator(
    IUserTenantClient userTenantClient,
    IFileTenantClient fileTenantClient,
    ITenantDataSeedingEngine seedingEngine,
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

        var tenantResponse = await userTenantClient.GetTenantAsync(request.TenantId, cancellationToken);
        var tenant = tenantResponse.EnsureSuccess();

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
            await userTenantClient.UpdateStatusAsync(
                request.TenantId,
                new UpdateTenantStatusRequest(TenantStatusIds.Provisioning),
                cancellationToken);

            var seedResult = await seedingEngine.SeedTenantDataAsync(
                request.TenantId,
                request.CompanyId,
                request.BusinessTypeIds,
                cancellationToken);

            if (seedResult.HasFailures)
            {
                logger.LogWarning(
                    "Tenant data seed completed with failures for tenant {TenantId}: {Succeeded} succeeded, {Skipped} skipped, {Failed} failed",
                    request.TenantId,
                    seedResult.SucceededCount,
                    seedResult.SkippedCount,
                    seedResult.FailedCount);
            }

            var companies = (await userTenantClient.GetCompaniesAsync(request.TenantId, cancellationToken))
                .EnsureSuccess();
            var companyNumbers = companies.Select(c => c.CompanyNumber).ToList();

            var bucketResponse = (await fileTenantClient.ProvisionBucketAsync(
                request.TenantId,
                new ProvisionTenantBucketRequest(request.TenantId, tenant.StorageBucketName, companyNumbers),
                cancellationToken)).EnsureSuccess();

            (await fileTenantClient.InitializeFoldersAsync(
                request.TenantId,
                new InitializeTenantFoldersRequest(bucketResponse.BucketName, request.TenantId, companyNumbers),
                cancellationToken)).ThrowIfFailed();

            (await userTenantClient.UpdateStorageBucketAsync(
                request.TenantId,
                new UpdateTenantStorageBucketRequest(bucketResponse.BucketName),
                cancellationToken)).ThrowIfFailed();

            await userTenantClient.UpdateStatusAsync(
                request.TenantId,
                new UpdateTenantStatusRequest(TenantStatusIds.Active),
                cancellationToken);

            logger.LogInformation(
                "Tenant provisioning completed for tenant {TenantId}, bucket {Bucket}",
                request.TenantId,
                bucketResponse.BucketName);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Tenant provisioning failed for tenant {TenantId}, correlation {CorrelationId}",
                request.TenantId,
                request.CorrelationId);

            await userTenantClient.UpdateStatusAsync(
                request.TenantId,
                new UpdateTenantStatusRequest(TenantStatusIds.ProvisioningFailed),
                cancellationToken);

            throw;
        }
    }
}
