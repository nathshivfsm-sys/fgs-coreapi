using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Queries.GetFgsInventorySubCategoryById;

public sealed class GetFgsInventorySubCategoryByIdQueryHandler(
    IFgsInventorySubCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventorySubCategoryByIdQuery, ApiResponse<FgsInventorySubCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventorySubCategoryDetailDto>> Handle(
        GetFgsInventorySubCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventorysubcategory",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventorySubCategoryDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventorySubCategoryDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventorySubCategoryDetailDto>.Fail(
                [$"Inventory sub-category '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventorySubCategoryDetailDto>.Ok(result);
    }
}
