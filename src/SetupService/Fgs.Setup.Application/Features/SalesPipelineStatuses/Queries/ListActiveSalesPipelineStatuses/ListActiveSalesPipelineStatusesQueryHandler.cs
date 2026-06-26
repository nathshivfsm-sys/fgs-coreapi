using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.ListActiveSalesPipelineStatuses;

public sealed class ListActiveSalesPipelineStatusesQueryHandler(
    IFgsSalesPipelineStatusReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSalesPipelineStatusesQuery, ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>> Handle(
        ListActiveSalesPipelineStatusesQuery request,
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
        "salespipelinestatuses",
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
                    request.Filters ?? new FgsSalesPipelineStatusListFilters(),
                    cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>.Ok(cached!);
    }
}
