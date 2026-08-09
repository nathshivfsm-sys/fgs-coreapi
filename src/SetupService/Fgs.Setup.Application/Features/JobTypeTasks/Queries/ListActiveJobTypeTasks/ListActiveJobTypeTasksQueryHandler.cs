using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.ListActiveJobTypeTasks;

public sealed class ListActiveJobTypeTasksQueryHandler(
    IJobTypeTaskReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveJobTypeTasksQuery, ApiResponse<PagedResult<JobTypeTaskSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeTaskSummaryDto>>> Handle(
        ListActiveJobTypeTasksQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(
            request.Page,
            request.PageSize,
            request.SortBy,
            request.SortDirection.ToString(),
            request.Search,
            CacheKeys.Fingerprint(request.Filters));

        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobtypetask",
            segment);

        var cached = await cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                var query = new SetupListQuery(
                    request.Page,
                    request.PageSize,
                    request.SortBy,
                    request.SortDirection,
                    request.Search,
                    IsActive: true);

                return await readRepository.ListAsync(
                    query,
                    request.Filters ?? new JobTypeTaskListFilters(),
                    cancellationToken);
            },
            cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<JobTypeTaskSummaryDto>>.Ok(cached!);
    }
}
