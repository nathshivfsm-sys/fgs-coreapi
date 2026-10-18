using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Features.TenantProvisioning;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TenantProvisioning.Commands.SyncTenantCatalog;

public sealed class SyncTenantCatalogCommandHandler(
    ITenantProvisioningOrchestrator orchestrator,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<SyncTenantCatalogCommandHandler> logger)
    : IRequestHandler<SyncTenantCatalogCommand, ApiResponse<TenantCatalogSyncResultDto>>
{
    public async Task<ApiResponse<TenantCatalogSyncResultDto>> Handle(
        SyncTenantCatalogCommand command,
        CancellationToken cancellationToken)
    {
        if (tenantContextAccessor.Current is not { TenantId: > 0, CompanyId: > 0 } scope)
        {
            return ApiResponse<TenantCatalogSyncResultDto>.Fail(
                ["Tenant and company context are required."],
                ApiStatusCodes.BadRequest);
        }

        var result = await orchestrator.SyncMissingCatalogAsync(
            scope.TenantId,
            scope.CompanyId,
            cancellationToken);

        if (result.HasFailures)
        {
            logger.LogWarning(
                "Tenant catalog sync completed with failures for tenant {TenantId}, company {CompanyId}: {Succeeded} succeeded, {Skipped} skipped, {Failed} failed",
                scope.TenantId,
                scope.CompanyId,
                result.SucceededCount,
                result.SkippedCount,
                result.FailedCount);

            return ApiResponse<TenantCatalogSyncResultDto>.Fail(
                FailureMessages(result),
                ApiStatusCodes.InternalServerError);
        }

        logger.LogInformation(
            "Tenant catalog sync completed for tenant {TenantId}, company {CompanyId}: {Succeeded} succeeded, {Skipped} skipped",
            scope.TenantId,
            scope.CompanyId,
            result.SucceededCount,
            result.SkippedCount);

        return ApiResponse<TenantCatalogSyncResultDto>.Ok(Map(result));
    }

    private static TenantCatalogSyncResultDto Map(TenantDataSeedResult result) =>
        new(
            result.SucceededCount,
            result.SkippedCount,
            result.FailedCount,
            result.TableResults
                .Select(table => new TenantCatalogSyncTableDto(
                    table.SeedCode,
                    table.Outcome.ToString(),
                    table.RowsInserted,
                    table.Message))
                .ToList());

    private static IReadOnlyList<string> FailureMessages(TenantDataSeedResult result)
    {
        var messages = result.TableResults
            .Where(table => table.Outcome == TenantSeedTableOutcome.Failed)
            .Select(table => string.IsNullOrWhiteSpace(table.Message)
                ? $"{table.SeedCode} failed."
                : table.Message!)
            .ToList();

        return messages.Count == 0
            ? ["Tenant catalog sync completed with failures."]
            : messages;
    }
}
