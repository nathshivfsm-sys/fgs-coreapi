using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Queries.LookupVendorInventoryItems;

public sealed class LookupVendorInventoryItemsQueryHandler(
    IFgsVendorInventoryItemReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupVendorInventoryItemsQuery, ApiResponse<IReadOnlyList<FgsVendorInventoryItemLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsVendorInventoryItemLookupDto>>> Handle(
        LookupVendorInventoryItemsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "vendorinventoryitem",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsVendorInventoryItemLookupDto>>.Ok(result ?? Array.Empty<FgsVendorInventoryItemLookupDto>());
    }
}
