using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Queries.GetFgsInventoryItemById;

public sealed class GetFgsInventoryItemByIdQueryHandler(
    IFgsInventoryItemReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventoryItemByIdQuery, ApiResponse<FgsInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemDetailDto>> Handle(
        GetFgsInventoryItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventoryitem",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventoryItemDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventoryItemDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventoryItemDetailDto>.Fail(
                [$"Inventory item '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventoryItemDetailDto>.Ok(result);
    }
}
