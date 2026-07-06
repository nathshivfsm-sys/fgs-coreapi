using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Queries.GetFgsInventoryLocationById;

public sealed class GetFgsInventoryLocationByIdQueryHandler(
    IFgsInventoryLocationReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventoryLocationByIdQuery, ApiResponse<FgsInventoryLocationDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryLocationDetailDto>> Handle(
        GetFgsInventoryLocationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventory-location",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventoryLocationDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventoryLocationDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventoryLocationDetailDto>.Fail(
                [$"Inventory location '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventoryLocationDetailDto>.Ok(result);
    }
}
