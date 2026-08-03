using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Queries.LookupInventoryItems;

public sealed class LookupInventoryItemsQueryHandler(
    IFgsInventoryItemReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupInventoryItemsQuery, ApiResponse<IReadOnlyList<FgsInventoryItemLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryItemLookupDto>>> Handle(
        LookupInventoryItemsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventoryitem",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInventoryItemLookupDto>>.Ok(
            result ?? Array.Empty<FgsInventoryItemLookupDto>());
    }
}
