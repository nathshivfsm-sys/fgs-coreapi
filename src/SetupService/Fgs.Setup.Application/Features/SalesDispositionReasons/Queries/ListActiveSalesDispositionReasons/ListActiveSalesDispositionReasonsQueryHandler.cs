using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.ListActiveSalesDispositionReasons;

public sealed class ListActiveSalesDispositionReasonsQueryHandler(
    IFgsSalesDispositionReasonReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSalesDispositionReasonsQuery, ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>> Handle(
        ListActiveSalesDispositionReasonsQuery request,
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
        "salesdispositionreason",
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
                request.Filters ?? new FgsSalesDispositionReasonListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>.Ok(cached!);
    }
}
