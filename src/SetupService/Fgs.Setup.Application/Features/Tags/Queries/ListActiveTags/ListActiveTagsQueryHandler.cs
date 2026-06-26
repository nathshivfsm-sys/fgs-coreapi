using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.ListActiveTags;

public sealed class ListActiveTagsQueryHandler(
    IFgsTagReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveTagsQuery, ApiResponse<PagedResult<FgsTagSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTagSummaryDto>>> Handle(
        ListActiveTagsQuery request,
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
        "tags",
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
                request.Filters ?? new FgsTagListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsTagSummaryDto>>.Ok(cached!);
    }
}
