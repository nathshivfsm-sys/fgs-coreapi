using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Queries.ListActiveJobCategories;

public sealed class ListActiveJobCategoriesQueryHandler(
    IJobCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveJobCategoriesQuery, ApiResponse<PagedResult<JobCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobCategorySummaryDto>>> Handle(
        ListActiveJobCategoriesQuery request,
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
            "jobcategory",
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
                    request.Filters ?? new JobCategoryListFilters(),
                    cancellationToken);
            },
            cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<JobCategorySummaryDto>>.Ok(cached!);
    }
}
