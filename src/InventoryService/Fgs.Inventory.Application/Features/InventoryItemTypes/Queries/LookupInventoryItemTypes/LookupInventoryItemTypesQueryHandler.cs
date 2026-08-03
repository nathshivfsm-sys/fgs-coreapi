using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.LookupInventoryItemTypes;

public sealed class LookupInventoryItemTypesQueryHandler(
    IFgsInventoryItemTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupInventoryItemTypesQuery, ApiResponse<IReadOnlyList<FgsInventoryItemTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryItemTypeLookupDto>>> Handle(
        LookupInventoryItemTypesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventoryitemtype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInventoryItemTypeLookupDto>>.Ok(result ?? Array.Empty<FgsInventoryItemTypeLookupDto>());
    }
}
