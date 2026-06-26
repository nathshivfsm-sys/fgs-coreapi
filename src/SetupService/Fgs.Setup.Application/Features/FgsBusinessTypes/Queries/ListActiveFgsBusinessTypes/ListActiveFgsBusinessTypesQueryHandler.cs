using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.ListActiveFgsBusinessTypes;

public sealed class ListActiveFgsBusinessTypesQueryHandler(
    IFgsBusinessTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveFgsBusinessTypesQuery, ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>> Handle(
        ListActiveFgsBusinessTypesQuery request,
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
        "businesstypes",
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
                    request.Filters ?? new FgsBusinessTypeListFilters(),
                    cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>.Ok(cached!);
    }
}
