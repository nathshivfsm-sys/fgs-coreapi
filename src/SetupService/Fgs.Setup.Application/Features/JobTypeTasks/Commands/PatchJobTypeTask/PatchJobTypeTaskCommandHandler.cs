using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.PatchJobTypeTask;

public sealed class PatchJobTypeTaskCommandHandler(
    IJobTypeTaskWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchJobTypeTaskCommandHandler> logger)
    : IRequestHandler<PatchJobTypeTaskCommand, ApiResponse<JobTypeTaskDetailDto>>
{
    public async Task<ApiResponse<JobTypeTaskDetailDto>> Handle(
        PatchJobTypeTaskCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd job type task {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtypetask"),
                cancellationToken);
        return ApiResponse<JobTypeTaskDetailDto>.Ok(result);
    }
}
