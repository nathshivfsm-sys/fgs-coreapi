using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.GetFgsInventoryItemTypeById;

public sealed class GetFgsInventoryItemTypeByIdQueryHandler(
    IFgsInventoryItemTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventoryItemTypeByIdQuery, ApiResponse<FgsInventoryItemTypeDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemTypeDetailDto>> Handle(
        GetFgsInventoryItemTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventoryitemtype",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventoryItemTypeDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventoryItemTypeDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventoryItemTypeDetailDto>.Fail(
                [$"Inventory item type '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventoryItemTypeDetailDto>.Ok(result);
    }
}
