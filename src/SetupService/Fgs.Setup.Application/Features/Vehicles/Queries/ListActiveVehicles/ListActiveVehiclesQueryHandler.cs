using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.ListActiveVehicles;

public sealed class ListActiveVehiclesQueryHandler(
    IFgsVehicleReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveVehiclesQuery, ApiResponse<PagedResult<FgsVehicleSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVehicleSummaryDto>>> Handle(
        ListActiveVehiclesQuery request,
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
        "vehicles",
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
                request.Filters ?? new FgsVehicleListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsVehicleSummaryDto>>.Ok(cached!);
    }
}
