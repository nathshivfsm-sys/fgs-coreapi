using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Queries.LookupInventoryLocations;

public sealed class LookupInventoryLocationsQueryHandler(
    IFgsInventoryLocationReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupInventoryLocationsQuery, ApiResponse<IReadOnlyList<FgsInventoryLocationLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryLocationLookupDto>>> Handle(
        LookupInventoryLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventory-locations",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInventoryLocationLookupDto>>.Ok(result ?? Array.Empty<FgsInventoryLocationLookupDto>());
    }
}
