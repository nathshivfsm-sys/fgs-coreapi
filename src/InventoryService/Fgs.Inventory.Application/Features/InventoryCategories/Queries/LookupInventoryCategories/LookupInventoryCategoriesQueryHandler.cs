using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.LookupInventoryCategories;

public sealed class LookupInventoryCategoriesQueryHandler(
    IFgsInventoryCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupInventoryCategoriesQuery, ApiResponse<IReadOnlyList<FgsInventoryCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryCategoryLookupDto>>> Handle(
        LookupInventoryCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventorycategory",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInventoryCategoryLookupDto>>.Ok(result ?? Array.Empty<FgsInventoryCategoryLookupDto>());
    }
}
