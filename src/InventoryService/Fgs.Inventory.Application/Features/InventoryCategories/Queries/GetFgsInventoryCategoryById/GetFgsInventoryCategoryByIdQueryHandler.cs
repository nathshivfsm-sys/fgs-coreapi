using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.GetFgsInventoryCategoryById;

public sealed class GetFgsInventoryCategoryByIdQueryHandler(
    IFgsInventoryCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventoryCategoryByIdQuery, ApiResponse<FgsInventoryCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryCategoryDetailDto>> Handle(
        GetFgsInventoryCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventorycategory",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventoryCategoryDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventoryCategoryDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventoryCategoryDetailDto>.Fail(
                [$"Inventory category '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventoryCategoryDetailDto>.Ok(result);
    }
}
