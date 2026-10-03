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
        var locationType = string.IsNullOrWhiteSpace(request.InventoryLocationType)
            ? null
            : request.InventoryLocationType.Trim().ToUpperInvariant();
        var typeSegment = locationType is null ? string.Empty : $":type={locationType}";
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventorylocation",
            CacheKeys.LookupSegment(request.ActiveOnly) + typeSegment);

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, locationType, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInventoryLocationLookupDto>>.Ok(result ?? Array.Empty<FgsInventoryLocationLookupDto>());
    }
}
