using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.DeleteJobTypeTask;

public sealed class DeleteJobTypeTaskCommandHandler(
    IJobTypeTaskWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteJobTypeTaskCommandHandler> logger)
    : IRequestHandler<DeleteJobTypeTaskCommand, ApiResponse<JobTypeTaskDetailDto>>
{
    public async Task<ApiResponse<JobTypeTaskDetailDto>> Handle(
        DeleteJobTypeTaskCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted job type task {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtypetask"),
                cancellationToken);
        return ApiResponse<JobTypeTaskDetailDto>.Ok(result);
    }
}
