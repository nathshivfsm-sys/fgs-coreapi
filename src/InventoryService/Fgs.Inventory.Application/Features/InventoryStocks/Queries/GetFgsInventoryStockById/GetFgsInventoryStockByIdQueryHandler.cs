using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Queries.GetFgsInventoryStockById;

public sealed class GetFgsInventoryStockByIdQueryHandler(
    IFgsInventoryStockReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventoryStockByIdQuery, ApiResponse<FgsInventoryStockDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryStockDetailDto>> Handle(
        GetFgsInventoryStockByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventorystock",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventoryStockDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventoryStockDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventoryStockDetailDto>.Fail(
                [$"Inventory stock '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventoryStockDetailDto>.Ok(result);
    }
}
