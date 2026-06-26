using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.ListActiveSalesActivityOutcomes;

public sealed class ListActiveSalesActivityOutcomesQueryHandler(
    IFgsSalesActivityOutcomeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSalesActivityOutcomesQuery, ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>> Handle(
        ListActiveSalesActivityOutcomesQuery request,
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
        "salesactivityoutcomes",
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
                    request.Filters ?? new FgsSalesActivityOutcomeListFilters(),
                    cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>.Ok(cached!);
    }
}
