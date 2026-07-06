using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.PatchJobType;

public sealed class PatchJobTypeCommandHandler(
    IJobTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchJobTypeCommandHandler> logger)
    : IRequestHandler<PatchJobTypeCommand, ApiResponse<JobTypeDetailDto>>
{
    public async Task<ApiResponse<JobTypeDetailDto>> Handle(
        PatchJobTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd job type {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtype"),
                cancellationToken);
        return ApiResponse<JobTypeDetailDto>.Ok(result);
    }
}
