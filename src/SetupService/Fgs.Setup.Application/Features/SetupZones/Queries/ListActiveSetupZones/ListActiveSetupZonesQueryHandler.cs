using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.ListActiveSetupZones;

public sealed class ListActiveSetupZonesQueryHandler(
    IFgsSetupZoneReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSetupZonesQuery, ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>> Handle(
        ListActiveSetupZonesQuery request,
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
        "zones",
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
                    request.Filters ?? new FgsSetupZoneListFilters(),
                    cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>.Ok(cached!);
    }
}
