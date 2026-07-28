using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.UpdateJobTypeTask;

public sealed class UpdateJobTypeTaskCommandHandler(
    IJobTypeTaskWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateJobTypeTaskCommandHandler> logger)
    : IRequestHandler<UpdateJobTypeTaskCommand, ApiResponse<JobTypeTaskDetailDto>>
{
    public async Task<ApiResponse<JobTypeTaskDetailDto>> Handle(
        UpdateJobTypeTaskCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated job type task {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtypetask"),
                cancellationToken);
        return ApiResponse<JobTypeTaskDetailDto>.Ok(result);
    }
}
