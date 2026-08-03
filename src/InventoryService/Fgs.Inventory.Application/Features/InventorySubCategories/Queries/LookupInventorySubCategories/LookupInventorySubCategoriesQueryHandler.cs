using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Queries.LookupInventorySubCategories;

public sealed class LookupInventorySubCategoriesQueryHandler(
    IFgsInventorySubCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupInventorySubCategoriesQuery, ApiResponse<IReadOnlyList<FgsInventorySubCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventorySubCategoryLookupDto>>> Handle(
        LookupInventorySubCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventorysubcategory",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInventorySubCategoryLookupDto>>.Ok(result ?? Array.Empty<FgsInventorySubCategoryLookupDto>());
    }
}
