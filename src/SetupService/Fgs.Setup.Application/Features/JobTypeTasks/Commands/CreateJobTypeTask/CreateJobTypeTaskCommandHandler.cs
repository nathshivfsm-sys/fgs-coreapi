using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.CreateJobTypeTask;

public sealed class CreateJobTypeTaskCommandHandler(
    IJobTypeTaskWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateJobTypeTaskCommandHandler> logger)
    : IRequestHandler<CreateJobTypeTaskCommand, ApiResponse<JobTypeTaskDetailDto>>
{
    public async Task<ApiResponse<JobTypeTaskDetailDto>> Handle(
        CreateJobTypeTaskCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created job type task {Id} with code {TaskName}", result.Id, result.TaskName);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtypetask"),
                cancellationToken);
        return ApiResponse<JobTypeTaskDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
