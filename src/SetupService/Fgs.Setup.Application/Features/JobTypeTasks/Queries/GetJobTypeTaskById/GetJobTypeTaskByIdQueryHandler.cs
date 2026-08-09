using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.GetJobTypeTaskById;

public sealed class GetJobTypeTaskByIdQueryHandler(
    IJobTypeTaskReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetJobTypeTaskByIdQuery, ApiResponse<JobTypeTaskDetailDto>>
{
    public async Task<ApiResponse<JobTypeTaskDetailDto>> Handle(
        GetJobTypeTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobtypetask",
            request.Id.ToString());

        var cached = await cache.GetAsync<JobTypeTaskDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<JobTypeTaskDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<JobTypeTaskDetailDto>.Fail(
                [$"Job Type Task '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<JobTypeTaskDetailDto>.Ok(result);
    }
}
