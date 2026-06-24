using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.ListActiveWarehouses;

public sealed class ListActiveWarehousesQueryHandler(
    IFgsWarehouseReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveWarehousesQuery, ApiResponse<PagedResult<FgsWarehouseSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsWarehouseSummaryDto>>> Handle(
        ListActiveWarehousesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {
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
                    "warehouses",
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
                            request.Filters ?? new FgsWarehouseListFilters(),
                            cancellationToken);
                    },
                    cancellationToken: cancellationToken);

                return ApiResponse<PagedResult<FgsWarehouseSummaryDto>>.Ok(cached!);
            }

            var listQuery = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                listQuery,
                request.Filters ?? new FgsWarehouseListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsWarehouseSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsWarehouseSummaryDto>>(ex);
        }
    }
}
