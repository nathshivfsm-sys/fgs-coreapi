using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.ListActiveLeadSources;

public sealed class ListActiveLeadSourcesQueryHandler(
    ILeadSourceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveLeadSourcesQuery, ApiResponse<PagedResult<LeadSourceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadSourceSummaryDto>>> Handle(
        ListActiveLeadSourcesQuery request,
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
        "leadsource",
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
                request.Filters ?? new LeadSourceListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<LeadSourceSummaryDto>>.Ok(cached!);
    }
}
